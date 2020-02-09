using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Transport;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using UnitTests.Shared.DummyObjects.Packets;
using UnitTests.Shared.DummyObjects.Layers;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Logic.Layers;
using PcapDotNet.Packets.Icmp;

namespace UnitTests.LayerTests
{
    [TestClass]
    public class LayerExtractorTests
    {
        private readonly ILayerExtractor _target;
        private readonly IDummyPacketBuilder _dummyPacketBuilder;
        private readonly Packet _dummyPacket;
        private readonly Packet _udpDummyPacket;
        private readonly Packet _icmpDummyPacket;

        public LayerExtractorTests()
        {
            _target = new LayerExtractor();
            _dummyPacketBuilder = new DummyPacketBuilder(new DummyLayerCreator());
            _dummyPacket = _dummyPacketBuilder.GetDummyTcpPacket();
            _udpDummyPacket = _dummyPacketBuilder.GetDummyUdpPacket();
            _icmpDummyPacket = _dummyPacketBuilder.GetDummyIcmpEchoRequestPacket();
        }

        [TestMethod]
        public void ExtractTcpLayerFromPacket_PacketIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.ExtractTcpLayerFromPacket(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void ExtractTcpLayerFromPacket_TcpLayerIsNull_ThrowsError()
        {
            // Arrange
            Packet packet = new Packet(new byte[0], DateTime.Now, DataLinkKind.IpV4);

            // Act
            Action action = () => _target.ExtractTcpLayerFromPacket(packet);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void ExtractTcpLayerFromPacket_PacketIsOk_ReturnsTcpLayer()
        {
            // Act
            var result = _target.ExtractTcpLayerFromPacket(_dummyPacket);

            // Assert
            result.Should().BeOfType<TcpLayer>();

            result.Checksum.Should().Be(_dummyPacket.Ethernet.IpV4.Tcp.Checksum);
            result.DestinationPort.Should().Be(_dummyPacket.Ethernet.IpV4.Tcp.DestinationPort);
        }

        [TestMethod]
        public void ExtractEthernetLayerFromPacket_PacketIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.ExtractEthernetLayerFromPacket(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void ExtractEthernetLayerFromPacket_PacketIsOk_ReturnsEthernetLayer()
        {
            // Act
            var result = _target.ExtractEthernetLayerFromPacket(_dummyPacket);

            // Assert
            result.Should().BeOfType<EthernetLayer>();

            result.Source.Should().Be(_dummyPacket.Ethernet.Source);
            result.Destination.Should().Be(_dummyPacket.Ethernet.Destination);
            result.EtherType.Should().Be(_dummyPacket.Ethernet.EtherType);
        }

        [TestMethod]
        public void ExtractIpV4LayerFromPacket_PacketIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.ExtractIpv4LayerFromPacket(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void ExtractIpv4LayerFromPacket_PacketIsOk_ReturnsIpv4Layer()
        {
            // Act
            var result = _target.ExtractIpv4LayerFromPacket(_dummyPacket);

            // Assert
            result.Should().BeOfType<IpV4Layer>();

            result.Source.Should().Be(_dummyPacket.Ethernet.IpV4.Source);
            result.CurrentDestination.Should().Be(_dummyPacket.Ethernet.IpV4.CurrentDestination);
            result.Fragmentation.Should().Be(_dummyPacket.Ethernet.IpV4.Fragmentation);
            result.HeaderChecksum.Should().Be(_dummyPacket.Ethernet.IpV4.HeaderChecksum);
            result.Identification.Should().Be(_dummyPacket.Ethernet.IpV4.Identification);
            result.Protocol.Should().Be(_dummyPacket.Ethernet.IpV4.Protocol);
            result.Ttl.Should().Be(_dummyPacket.Ethernet.IpV4.Ttl);
            result.TypeOfService.Should().Be(_dummyPacket.Ethernet.IpV4.TypeOfService);
        }

        [TestMethod]
        public void ExtractPayloadLayerFromPacket_PacketIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.ExtractPayloadLayerFromPacket(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void ExtractPayloadLayerFromPacket_PacketIsOk_ReturnsPayloadLayer()
        {
            // Act
            var result = _target.ExtractPayloadLayerFromPacket(_dummyPacket);

            // Assert
            result.Should().BeOfType<PayloadLayer>();
            result.Data.Should().BeEquivalentTo(_dummyPacket.Ethernet.Payload);
        }

        [TestMethod]
        public void ExtractUdpLayerFromPacket_PacketIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.ExtractUdpLayerFromPacket(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void ExtractUdpLayerFromPacket_PacketIsOk_ReturnsUdpLayer()
        {
            // Act
            var result = _target.ExtractUdpLayerFromPacket(_udpDummyPacket);

            // Assert
            result.Should().BeOfType<UdpLayer>();

            result.Checksum.Should().Be(_udpDummyPacket.Ethernet.IpV4.Udp.Checksum);
            result.DestinationPort.Should().Be(_udpDummyPacket.Ethernet.IpV4.Udp.DestinationPort);
            result.SourcePort.Should().Be(_udpDummyPacket.Ethernet.IpV4.Udp.SourcePort);
            result.DestinationPort.Should().Be(_udpDummyPacket.Ethernet.IpV4.Udp.DestinationPort);
        }

        [TestMethod]
        public void ExtractIcmpLayerFromPacket_PacketIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.ExtractIcmpLayerFromPacket(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void ExtractIcmpLayerFromPacket_PacketIsOk_ReturnsIcmpLayer()
        {
            // Act
            var result = _target.ExtractIcmpLayerFromPacket(_icmpDummyPacket);

            // Assert
            result.Should().BeOfType<IcmpEchoReplyLayer>();

            result.Checksum.Should().Be(_icmpDummyPacket.Ethernet.IpV4.Icmp.Checksum);
        }
    }
}
