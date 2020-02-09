using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace UnitTests.Shared.DummyObjects.Layers
{
    public interface IDummyLayerCreator
    {
        /// <summary>
        /// Dummy tcp layer
        /// </summary>
        /// <returns>Dummy tcp layer prefilled</returns>
        TcpLayer GetDummyTcpLayer();

        /// <summary>
        /// Creates dummy ethernet layer
        /// </summary>
        /// <returns>Dummy ethernet layer prefilled</returns>
        EthernetLayer GetDummyEthernetLayer();

        /// <summary>
        /// Creates dummy Ipv4 layer object
        /// </summary>
        /// <returns>Ipv4 layer with some values</returns>
        IpV4Layer GetDummyIpV4Layer();

        /// <summary>
        /// Creates dummy Payload layer object
        /// </summary>
        /// <returns>Payload layer with some values</returns>
        PayloadLayer GetDummyPayloadLayer();

        /// <summary>
        /// Dummy udp layer
        /// </summary>
        /// <returns>Udp layer object prefilled</returns>
        UdpLayer GetDummyUdpLayer();

        /// <summary>
        /// Dummy Icmp echo layer
        /// </summary>
        /// <returns>Prefilled dummy icmp echo layer</returns>
        IcmpLayer GetDummyIcmpEchoLayer();

        /// <summary>
        /// Dummy icmp reply layer
        /// </summary>
        /// <returns>Prefilled dummy icmp reply layer</returns>
        IcmpLayer GetDummyIcmpReplyLayer();
    }
}
