using PcapDotNet.Core;
using PcapPacketModifier.Logic.Sender.Interfaces;
using PcapPacketModifier.Logic.UserExperience.Interfaces;
using PcapPacketModifier.Userdata.Packets;
using System;
using System.Collections.Generic;

namespace PcapPacketModifier.Logic.Sender
{
    public class PacketSender : IPacketSender
    {
        private readonly ITextDisplayer _textDisplayer;
        private readonly IUserInputHandler _userInputHandler;

        /// <summary>
        /// List of devices registered on local machine
        /// </summary>
        private readonly IList<LivePacketDevice> _allDevices;

        public PacketSender(ITextDisplayer textDisplayer,
                                      IUserInputHandler userInputHandler)
        {
            _allDevices = LivePacketDevice.AllLocalMachine;
            _textDisplayer = textDisplayer;
            _userInputHandler = userInputHandler;
        }

        /// <summary>
        /// Sends packet by calling core functions
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="countToSend">Count to send</param>
        /// <param name="timeToWaitBeforeNextPacketToSend">Time to wait until sending next packet in milliseconds</param>
        public void SendPacket(CustomBasePacket packet, int countToSend = 1, int timeToWaitBeforeNextPacketToSend = 0)
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

            _textDisplayer.DisplayAllLocalMachineNetworkInterfaces(_allDevices);

            int userChosenDevice = 0;
            int.TryParse(_userInputHandler.GetUserInput(), out userChosenDevice);
            int correctedDeviceNumber = userChosenDevice - 1; // -1 because array
            if (!(correctedDeviceNumber < 0) && !(correctedDeviceNumber > _allDevices.Count))
            {
                PacketDevice selectedDevice = _allDevices[correctedDeviceNumber];
                using (PacketCommunicator communicator = selectedDevice.Open(65535,
                                                                                                                 PacketDeviceOpenAttributes.None,
                                                                                                                 1000))
                {
                    for (uint i = 0; i < countToSend; i++)
                    {
                        communicator.SendPacket(packet.BuildPacket());
                        _textDisplayer.PrintText($"Sended packet nr {i + 1}...");
                        PauseBeforeSendingPacket(timeToWaitBeforeNextPacketToSend);
                    }
                }
            }
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