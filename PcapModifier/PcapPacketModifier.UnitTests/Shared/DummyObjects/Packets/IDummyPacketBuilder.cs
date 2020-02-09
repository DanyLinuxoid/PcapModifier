using PcapDotNet.Packets;

namespace UnitTests.Shared.DummyObjects.Packets
{
    public interface IDummyPacketBuilder
    {
        /// <summary>
        /// Gets dummy tcp packet
        /// </summary>
        /// <returns>Dummy tcp packet prefilled</returns>
        Packet GetDummyTcpPacket();

        /// <summary>
        /// Gets dummy udp packet
        /// </summary>
        /// <returns>Dummy udp packet prefilled</returns>
        Packet GetDummyUdpPacket();

        /// <summary>
        /// Gets dummy icmp request packet
        /// </summary>
        /// <returns>Prefilled icmp echo request packet</returns>
        Packet GetDummyIcmpEchoRequestPacket();

        /// <summary>
        /// Gets dummy icmp echo reply packet
        /// </summary>
        /// <returns>Dummy icmp echo reply packet</returns>
        Packet GetDummyIcmpEchoReplyPacket();

    }
}
