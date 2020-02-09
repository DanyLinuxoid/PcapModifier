using PcapDotNet.Packets;
using System;
using UnitTests.Shared.DummyObjects.Layers;

namespace UnitTests.Shared.DummyObjects.Packets
{
    /// <summary>
    /// Creates dummy packets
    /// </summary>
    public class DummyPacketBuilder : IDummyPacketBuilder
    {
        public readonly IDummyLayerCreator _dummyLayerCreator;

        public DummyPacketBuilder(IDummyLayerCreator dummyLayerCreator)
        {
            _dummyLayerCreator = dummyLayerCreator;
        }

        /// <summary>
        /// Creates dummy tcp packet 
        /// </summary>
        /// <returns>Dummy tcp packett</returns>
        public Packet GetDummyTcpPacket()
        {
            return new PacketBuilder(_dummyLayerCreator.GetDummyEthernetLayer(),
                                                 _dummyLayerCreator.GetDummyIpV4Layer(),
                                                 _dummyLayerCreator.GetDummyTcpLayer(),
                                                 _dummyLayerCreator.GetDummyPayloadLayer()).Build(DateTime.Now);
        }

        /// <summary>
        /// Gets dummy udp packet
        /// </summary>
        /// <returns>Dummy udp packet prefilled</returns>
        public Packet GetDummyUdpPacket()
        {
            return new PacketBuilder(_dummyLayerCreator.GetDummyEthernetLayer(),
                                                 _dummyLayerCreator.GetDummyUdpLayer(),
                                                 _dummyLayerCreator.GetDummyIpV4Layer(),
                                                 _dummyLayerCreator.GetDummyPayloadLayer()).Build(DateTime.Now);
        }

        /// <summary>
        /// Gets dummy icmp request packet
        /// </summary>
        /// <returns>Prefilled icmp echo request packet</returns>
        public Packet GetDummyIcmpEchoRequestPacket()
        {
            return new PacketBuilder(_dummyLayerCreator.GetDummyIpV4Layer(),
                                                 _dummyLayerCreator.GetDummyIcmpEchoLayer(),
                                                 _dummyLayerCreator.GetDummyEthernetLayer()).Build(DateTime.Now);
        }

        /// <summary>
        /// Gets dummy icmp echo reply packet
        /// </summary>
        /// <returns>Dummy icmp echo reply packet</returns>
        public Packet GetDummyIcmpEchoReplyPacket()
        {
            return new PacketBuilder(_dummyLayerCreator.GetDummyIcmpReplyLayer(),
                                                 _dummyLayerCreator.GetDummyIpV4Layer(),
                                                 _dummyLayerCreator.GetDummyEthernetLayer()).Build(DateTime.Now);
        }
    }
}
