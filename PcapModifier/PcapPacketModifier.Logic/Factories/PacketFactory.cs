using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Logic.Packets.Models;
using PcapPacketModifier.Userdata.Packets;

namespace PcapPacketModifier.Logic.Factories
{
    /// <summary>
    /// Simple static factory for packet creation
    /// </summary>
    public static class PacketFactory
    {
        /// <summary>
        /// Creates packet by protocol and provided input to constructor
        /// </summary>
        /// <param name="protocol">Packet protocol</param>
        /// <param name="layerExtractor">Object that responsible for layer extraction</param>
        /// <param name="layerModifier">Object that responsible for layer modification</param>
        /// <returns>New Custom packet of provided protocol with provided values in it</returns>
        public static CustomBasePacket GetPacket(IpV4Protocol protocol,
                                                                      ILayerExtractor layerExtractor,
                                                                      ILayerModifier layerModifier)
        {
            switch (protocol)
            {
                case IpV4Protocol.Tcp:
                    return new CustomTcpPacket(layerModifier, layerExtractor);
                case IpV4Protocol.Udp:
                    return new CustomUdpPacket(layerModifier, layerExtractor);
                case IpV4Protocol.InternetControlMessageProtocol:
                    return new CustomIcmpPacket(layerModifier, layerExtractor);

                default:
                    return null;
            }
        }
    }
}
