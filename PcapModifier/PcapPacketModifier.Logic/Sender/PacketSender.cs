using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Logic.Factories.Interfaces;
using PcapPacketModifier.Logic.Sender.Interfaces;
using PcapPacketModifier.Logic.Tools.Interfaces;
using PcapPacketModifier.Logic.UserExperience.Interfaces;
using PcapPacketModifier.Userdata.Packets.Interfaces;
using PcapPacketModifier.Userdata.User;
using System;
using System.Collections.Generic;
using System.IO;

namespace PcapPacketModifier.Logic.Sender
{
    public class PacketSender : IPacketSender
    {
        private readonly IUserExperience _userExperience;
        private readonly IFileHandler _fileHandler;
        private readonly IPacketFactory _packetFactory;

        /// <summary>
        /// List of devices registered on local machine
        /// </summary>
        private readonly IList<LivePacketDevice> _allDevices;

        public PacketSender(IFileHandler fileHandler,
                                      IUserExperience userExperience,
                                      IPacketFactory packetFactory)
        {
            _fileHandler = fileHandler;
            _userExperience = userExperience;
            _packetFactory = packetFactory;

            _allDevices = LivePacketDevice.AllLocalMachine;
        }

        /// <summary>
        /// Sends packet by calling core functions
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="countToSend">Count to send</param>
        /// <param name="timeToWaitBeforeNextPacketToSend">Time to wait until sending next packet in milliseconds</param>
        public void SendPacket(INewPacket packet, int countToSend = 1, int timeToWaitBeforeNextPacketToSend = 0)
        {
            if (packet is null)
            {
                throw new ArgumentNullException(nameof(packet));
            }

            if (_allDevices == null ||
                _allDevices.Count == 0)
            {
                throw new InvalidOperationException("No devices found on local machine to send packets with");
            }

            int userChosenDevice = LetUserChooseInterfaceBeforeWorkingWithPackets();
            PacketDevice selectedDevice = _allDevices[userChosenDevice - 1];
            using (PacketCommunicator communicator = selectedDevice.Open(65535,
                                                                                                            PacketDeviceOpenAttributes.NoCaptureLocal,
                                                                                                            1000))
            {
                for (uint i = 0; i < countToSend; i++)
                {
                    communicator.SendPacket(packet.BuildPacket(true, i));
                    _userExperience.UserTextDisplayer.PrintText($"Sended packet nr {i + 1}...");
                    PauseBeforeSendingPacket(timeToWaitBeforeNextPacketToSend);
                }
            }
        }

        /// <summary>
        /// Intercepts and forwards packets to web
        /// </summary>
        public void InterceptAndForwardPackets(UserInputData userInput)
        {
            INewPacket packetToCopyFrom = default;
            bool isAutoModifyPackets = false;
            bool isFilterEnabled = false;
            int packetCountGoneThroughDevice = 0;
            int selectedOutputDevice = LetUserChooseInterfaceBeforeWorkingWithPackets();

            PacketDevice device = _allDevices[selectedOutputDevice - 1]; // - 1 because of array
            using (PacketCommunicator inputCommunicator = device.Open(65535, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                if (inputCommunicator.DataLink.Kind != DataLinkKind.Ethernet)
                {
                    _userExperience.UserTextDisplayer.PrintTextAndExit("Only ethernet networks are supported");
                }

                using (PacketCommunicator outputCommunicator = device.Open(65535, PacketDeviceOpenAttributes.Promiscuous, 1000))
                {
                    if (!string.IsNullOrWhiteSpace(userInput.PacketFilterProtocol))
                    {
                        inputCommunicator.SetFilter(userInput.PacketFilterProtocol.ToLower());
                        outputCommunicator.SetFilter(userInput.PacketFilterProtocol.ToLower());
                        isFilterEnabled = true;
                    }

                    while (inputCommunicator.ReceivePacket(out Packet packet) != PacketCommunicatorReceiveResult.Eof)
                    {
                        if (packet == null)
                        {
                            continue;
                        }

                        if (packetCountGoneThroughDevice > 10) // for hint to be seen always, and not to cause too much mess in console
                        {
                            _userExperience.UserTextDisplayer.ClearConsole();
                            _userExperience.UserTextDisplayer.PrintText("-M to modify one packet");
                            _userExperience.UserTextDisplayer.PrintText("-A to automatically modify packets");
                            _userExperience.UserTextDisplayer.PrintText("-P to pause");
                            packetCountGoneThroughDevice = 0;
                        }

                        _userExperience.UserTextDisplayer.ShowPacketBaseInfo(packet);
                        if (Console.KeyAvailable) // if user pressed key, then does manipulation with packets
                        {
                            ConsoleKeyInfo choice = Console.ReadKey(true);
                            switch (choice.Key)
                            {
                                case ConsoleKey.A: // modify every packet using user saved settings if filter is enabled
                                    if (isFilterEnabled)
                                    {
                                        packetToCopyFrom = GetPacketFilledWithUserValuesToCopyFrom(packet.Ethernet.IpV4.Protocol, userInput.IsSendPacket);
                                        isAutoModifyPackets = (packetToCopyFrom != null);
                                    }
                                    else
                                    {
                                        _userExperience.UserTextDisplayer.PrintText("NOTE: You can't automodify packets without protocol filtering");
                                    }
                                    break;

                                case ConsoleKey.M: // modify one packet in normal way
                                    packet = _packetFactory.GetPacketByProtocol(packet.Ethernet.IpV4.Protocol).ModifyLayers().BuildPacket(true, 0);
                                    break;

                                case ConsoleKey.P:
                                    _userExperience.UserTextDisplayer.PrintText("PAUSED");
                                    _userExperience.UserInputHandler.WaitForUserToPressKey();
                                    break;
                            }
                        }

                        if (isAutoModifyPackets)
                        {
                            Packet autoModifiedPacket = AutoModifyPacket(packet, packetToCopyFrom);
                            if (autoModifiedPacket != null)
                            {
                                packet = autoModifiedPacket;
                            }
                        }

                        if (userInput.IsUserWantsToSavePacket)
                        {
                            _fileHandler.TrySaveOnePacketToDisk(packet);
                        }

                        if (userInput.IsSendPacket)
                        {
                            outputCommunicator.SendPacket(packet);
                        }

                        packetCountGoneThroughDevice++;
                    }
                }
            }
        }

        /// <summary>
        /// Automodifies packet for further sending
        /// </summary>
        /// <param name="toCopyTo">Packet to copy to user values</param>
        /// <param name="packetToCopyFrom">Packet to copy from</param>
        /// <returns>Packet with user values, that can be sended to web</returns>
        private Packet AutoModifyPacket(Packet toCopyTo, INewPacket packetToCopyFrom)
        {
            // replaces 'Packet' with modifiable custom packet, so it would be possible to swap layers/modules
            INewPacket packetToCopyTo = _packetFactory.GetPacketByProtocol(toCopyTo.Ethernet.IpV4.Protocol).ExtractLayers(toCopyTo);

            return packetToCopyTo != null
                    ? packetToCopyTo.CopyModulesFrom(packetToCopyFrom).BuildPacket(false)
                    : null;
        }

        /// <summary>
        /// Gets user packet with values to copy from to other packets
        /// </summary>
        /// <param name="isFilteringByProtocolEnabled">If filtering is enabled, then autosending is possible</param>
        /// <returns>New packet with ONLY user values in it, all other values are set to default state</returns>
        private INewPacket GetPacketFilledWithUserValuesToCopyFrom(IpV4Protocol ipV4Protocol, bool isSendingEnabled)
        {
            INewPacket packetWithUserValuesToModify = _packetFactory.GetPacketByProtocol(ipV4Protocol)
                .ModifyLayers(); // Give user default packet to fill with his values, so we would know, what values to modify in all other packets

            _userExperience.UserTextDisplayer.PrintText("Now all packets will be modified according to changed values in new packet, press any key to continue...");
            if (!isSendingEnabled)
            {
                _userExperience.UserTextDisplayer.PrintText("NOTE: Packet sending is disabled, modified packets will NOT be sended!");
            }

            Console.ReadKey();

            return packetWithUserValuesToModify;
        }

        /// <summary>
        /// Display information about local interfaces and gives user possibility to chose device to work with
        /// Is obligatory before starting work with packets
        /// </summary>
        /// <returns></returns>
        private int LetUserChooseInterfaceBeforeWorkingWithPackets()
        {
            _userExperience.UserTextDisplayer.DisplayAllLocalMachineNetworkInterfaces(_allDevices);
            int userChosenDevice = _userExperience.UserInputHandler.GetUserChoosenLocalMachineInternetDevice(_allDevices);
            if (userChosenDevice == 0)
            {
                _userExperience.UserTextDisplayer.PrintTextAndExit("Invalid interface");
            }

            return userChosenDevice;
        }

        /// <summary>
        /// Do pause in milliseconds before sending any packet again 
        /// </summary>
        private void PauseBeforeSendingPacket(int timeToWait)
        {
            DateTime timeUntilToWait = DateTime.Now.AddMilliseconds(timeToWait);
            while(timeUntilToWait > DateTime.Now) { }
        }
    }
}