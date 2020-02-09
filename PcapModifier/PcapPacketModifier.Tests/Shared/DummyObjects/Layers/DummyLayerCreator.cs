using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace UnitTests.Shared.DummyObjects.Layers
{
    /// <summary>
    /// Creates dummy layer for tests
    /// </summary>
    public class DummyLayerCreator : IDummyLayerCreator
    {
        public TcpLayer GetDummyTcpLayer()
        {
            return new TcpLayer()
            {
                SourcePort = 80,
                DestinationPort = 80,
                Checksum = 11010,
                SequenceNumber = 123,
                AcknowledgmentNumber = 321,
                ControlBits = TcpControlBits.NonceSum,
                Window = 777,
                UrgentPointer = 90,
                Options = TcpOptions.None,
            };
        }

        /// <summary>
        /// Creates ethernet layer dummy object for tests
        /// </summary>
        /// <returns>Ethernet layer with some values</returns>
        public EthernetLayer GetDummyEthernetLayer()
        {
            return new EthernetLayer()
            {
                Source = new MacAddress("00:11:22:33:44:55"),
                Destination = new MacAddress("FF:FF:FF:FF:FF:FF"),
                EtherType = EthernetType.IpV4,
            };
        }

        /// <summary>
        /// Creates dummy Ipv4 layer object
        /// </summary>
        /// <returns>Ipv4layer with some values</returns>
        public IpV4Layer GetDummyIpV4Layer()
        {
            return new IpV4Layer()
            {
                Source = new IpV4Address("127.0.0.1"),
                CurrentDestination = new IpV4Address("127.0.0.1"),
                Fragmentation = IpV4Fragmentation.None,
                HeaderChecksum = 2121,
                Identification = 19191,
                Options = IpV4Options.None,
                Protocol = IpV4Protocol.Tcp,
                Ttl = 100,
                TypeOfService = 122,
            };
        }

        /// <summary>
        /// Creates dummy Payload layer object
        /// </summary>
        /// <returns>Payload layer with some values</returns>
        public PayloadLayer GetDummyPayloadLayer()
        {
            return new PayloadLayer()
            {
                Data = new Datagram(new byte[80]),
            };
        }

        /// <summary>
        /// Dummy udp layer
        /// </summary>
        /// <returns>Udp layer object prefilled</returns>
        public UdpLayer GetDummyUdpLayer()
        {
            return new UdpLayer()
            {
                Checksum = 1111,
                DestinationPort = 80,
                SourcePort = 80,
            };
        }

        /// <summary>
        /// Dummy Icmp echo layer
        /// </summary>
        /// <returns>Prefilled dummy icmp echo layer</returns>
        public IcmpLayer GetDummyIcmpEchoLayer()
        {
            return new IcmpEchoLayer
            {
                Checksum = 111,
                SequenceNumber = 1111,
                Identifier = 0,
            };
        }

        /// <summary>
        /// Dummy icmp reply layer
        /// </summary>
        /// <returns>Prefilled dummy icmp reply layer</returns>
        public IcmpLayer GetDummyIcmpReplyLayer()
        {
            return new IcmpEchoReplyLayer
            {
                Checksum = 111,
                Identifier = 1111,
                SequenceNumber = 322,
            };
        }
    }
}