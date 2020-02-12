using System;
using PcapDotNet.Packets;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Logic.Packets.Interfaces;
using PcapPacketModifier.Logic.Sender.Interfaces;
using PcapPacketModifier.Userdata.Packets.Interfaces;

namespace PcapPacketModifier.Logic.Packets
{
    /// <summary>
    /// Top level Manager, responsible for providing simple TOP level functions to user
    /// </summary>
    public class PacketManager : IPacketManager
    {
        private readonly ILayerManager _layerManager;
        private readonly IPacketSender _packetSender;

        public PacketManager(ILayerManager layerManager,
                                        IPacketSender packetSender)
        {
            _layerManager = layerManager;
            _packetSender = packetSender;
        }

        /// <summary>
        /// Extracts data from provided packet
        /// </summary>
        /// <param name="packet">Packet to extract data from</param>
        /// <returns>Returns new Packet object with extracted layers</returns>
        public INewPacket ExtractLayersFromPacket(Packet packet)
        {
            if (packet is null)
            {
                throw new ArgumentNullException(nameof(packet));
            }

            return _layerManager.ExtractLayersFromPacketAndReturnNewPacket(packet);
        }

        /// <summary>
        /// Sends provided packet by ip and mac address which is set in packet
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="countToSend">Count to send</param>
        public void SendPacket(INewPacket packet, int countToSend, int timeToWaitUntilNextPacketSend)
        {
            if (packet is null)
            {
                throw new ArgumentNullException(nameof(packet));
            }

            _packetSender.SendPacket(packet, countToSend, timeToWaitUntilNextPacketSend);
        }

        /// <summary>
        /// Is able to intercept and forward these packets, or forward after modifying
        /// </summary>
        /// <param name="packet">Packet to forward</param>
        public void InterceptAndForwardPackets(Userdata.User.UserInputData userInput)
        {
            _packetSender.InterceptAndForwardPackets(userInput);
        }
    }
}