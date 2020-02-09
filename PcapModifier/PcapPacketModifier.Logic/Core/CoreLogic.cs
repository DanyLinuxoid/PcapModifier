﻿using PcapDotNet.Packets;
using PcapPacketModifier.Logic.Packets.Interfaces;
using PcapPacketModifier.Logic.Tools.Interfaces;
using PcapPacketModifier.Userdata.Packets;
using PcapPacketModifier.Userdata.User;
using System;

namespace PcapPacketModifier.Logic.Core
{
    /// <summary>
    /// Responsible for core computations and logic branching
    /// </summary>
    public class CoreLogic : ICoreLogic
    {
        private readonly IPacketManager _packetManager;
        private readonly IFileHandler _fileHandler;

        public CoreLogic(IPacketManager packetManager,
                                IFileHandler fileHandler)
        {
            _packetManager = packetManager;
            _fileHandler = fileHandler;
        }

        /// <summary>
        /// Starts program core logic, goes through all steps, one by one
        /// </summary>
        /// <param name="inputData">User input data to work with during logic</param>
        public void ProcessLogic(UserInputData inputData)
        {
            if (inputData is null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }

            Packet packet = _fileHandler.TryOpenUserPacketFromFile(inputData.PathToFile);
            if (packet == null)
            {
                throw new InvalidOperationException(nameof(packet) + " was null");
            }

            CustomBasePacket customPacket = _packetManager.ExtractLayersFromPacket(packet);

            if (inputData.IsModifyPacket)
            {
                customPacket.ModifyLayers();
            }

            if (inputData.IsSendPacket)
            {
                _packetManager.SendPacket(customPacket, inputData.PacketCountToSend);
            }

            if (inputData.IsUserWantsToSavePacketAfterModifying)
            {
                _fileHandler.TrySaveOnePacketToDisk(customPacket.BuildPacket());
            }
        }
    }
}