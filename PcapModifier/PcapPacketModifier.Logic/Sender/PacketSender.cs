using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Logic.Modules.Interfaces;
using PcapPacketModifier.Logic.Sender.Interfaces;
using PcapPacketModifier.Logic.Tools.Interfaces;
using PcapPacketModifier.Logic.UserExperience.Interfaces;
using PcapPacketModifier.Userdata.Packets.Interfaces;
using System;
using System.Collections.Generic;

namespace PcapPacketModifier.Logic.Sender
{
    public class PacketSender : IPacketSender
    {
        private readonly IUserExperience _userExperience;
        private readonly IFileHandler _fileHandler;
        private readonly IModuleModifier _moduleModifier;
        private readonly ILayerManager _layerManager;

        /// <summary>
        /// List of devices registered on local machine
        /// </summary>
        private readonly IList<LivePacketDevice> _allDevices;

        /// <summary>
        /// Default path to dump file
        /// </summary>
        private readonly string _defaultPathToDumpFile;

        public PacketSender(IFileHandler fileHandler,
                                      IUserExperience userExperience,
                                      IModuleModifier moduleModifier,
                                      ILayerManager layerManager)
        {
            _fileHandler = fileHandler;
            _userExperience = userExperience;
            _moduleModifier = moduleModifier;
            _layerManager = layerManager;

            _allDevices = LivePacketDevice.AllLocalMachine;
            _defaultPathToDumpFile = _fileHandler.PathProvider.GetDefaultPathForDumpFile();
            if (!_fileHandler.IsFileExisting(_defaultPathToDumpFile))
            {
                _fileHandler.TryCreateSimpleEmptyFile(_defaultPathToDumpFile);
            }
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
                throw new InvalidOperationException("No devices found on local machine");
            }

            int userChosenDevice = LetUserChooseInterfaceBeforeWorkingWithPackets();
            PacketDevice selectedDevice = _allDevices[userChosenDevice - 1];
            using (PacketCommunicator communicator = selectedDevice.Open(65535,
                                                                                                            PacketDeviceOpenAttributes.Promiscuous,
                                                                                                            1000))
            {
                for (uint i = 0; i < countToSend; i++)
                {
                    communicator.SendPacket(packet.BuildPacket(0));
                    _userExperience.UserTextDisplayer.PrintText($"Sended packet nr {i + 1}...");
                    PauseBeforeSendingPacket(timeToWaitBeforeNextPacketToSend);
                }
            }
        }

        /// <summary>
        /// Intercepts and forwards packets to web
        /// </summary>
        public void InterceptAndForwardPackets(Userdata.User.UserInputData userInput)
        {
            int selectedOutputDevice = LetUserChooseInterfaceBeforeWorkingWithPackets();
            PacketDevice device = _allDevices[selectedOutputDevice - 1]; // - 1 because of array
            using (PacketCommunicator inputCommunicator = device.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                using (PacketCommunicator outputCommunicator = device.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
                {
                    while (inputCommunicator.ReceivePacket(out Packet packet) != PacketCommunicatorReceiveResult.Eof)
                    {
                        if (packet == null)
                        {
                            continue;
                        }

                        if (Console.KeyAvailable) // Checks if user pressed key, and if user pressed, then does manipulation with packets
                        {
                            ConsoleKeyInfo choice = Console.ReadKey(true);
                            switch (choice.Key)
                            {
                                case ConsoleKey.A:
                                    // In progress
                                    break;
                                case ConsoleKey.M:
                                    packet = _layerManager.ExtractLayersFromPacketAndReturnNewPacket(packet).ModifyLayers().BuildPacket();
                                    break;
                            }
                        }

                        if (userInput.PacketFilterProtocol != default)
                        {
                            if (userInput.PacketFilterProtocol == packet.Ethernet.IpV4.Protocol)
                            {
                                _userExperience.UserTextDisplayer.ShowPacketBaseInfo(packet);
                            }
                        }
                        else // if filter is disabled
                        {
                            _userExperience.UserTextDisplayer.ShowPacketBaseInfo(packet);
                        }

                        if (userInput.IsSendOnePacket)
                        {
                            outputCommunicator.SendPacket(packet);
                        }
                    }
                }
            }
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
        /// Do pause before sending any packet again (if needed)
        /// </summary>
        private void PauseBeforeSendingPacket(int timeToWait)
        {
            DateTime timeUntilToWait = DateTime.Now.AddMilliseconds(timeToWait);
            while(timeUntilToWait > DateTime.Now) { }
        }
    }
}