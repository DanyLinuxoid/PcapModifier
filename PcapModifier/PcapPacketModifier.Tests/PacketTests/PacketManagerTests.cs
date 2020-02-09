using System;
using Moq;
using FluentAssertions;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using UnitTests.Shared.DummyObjects.Packets;
using UnitTests.Shared.DummyObjects.Layers;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Logic.Sender.Interfaces;
using PcapPacketModifier.Logic.Packets.Interfaces;
using PcapPacketModifier.Logic.Packets;
using PcapPacketModifier.Logic.Packets.Models;
using NUnit.Framework;

namespace UnitTests.PacketTests
{
    public class PacketManagerTests
    {
        private Mock<ILayerManager> _layerManagerMock;
        private Mock<ILayerExtractor> _layerExtractor;
        private Mock<ILayerModifier >_layerModifier;
        private Mock<IPacketSender> _packetSenderMock;
        private IDummyPacketBuilder _dummyPacketBuilder;
        private IPacketManager _target;

        [SetUp]
        public void Setup()
        {
            _layerManagerMock = new Mock<ILayerManager>();
            _layerExtractor = new Mock<ILayerExtractor>();
            _layerModifier = new Mock<ILayerModifier>();
            _packetSenderMock = new Mock<IPacketSender>();
            _dummyPacketBuilder = new DummyPacketBuilder(new DummyLayerCreator());
            _target = new PacketManager(_layerManagerMock.Object, _packetSenderMock.Object);
        }

        [Test]
        public void ExtractLayersFromPacket_PacketIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.ExtractLayersFromPacket(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ExtractLayersFromPacket_PacketIsOk_ReturnsCustomPacket()
        {
            // Arrange
            Packet packet = _dummyPacketBuilder.GetDummyTcpPacket();
            CustomTcpPacket customTcpPacket = new CustomTcpPacket(_layerModifier.Object, _layerExtractor.Object)
            {
                EthernetLayer = new EthernetLayer(),
                TcpLayer = new PcapDotNet.Packets.Transport.TcpLayer(),
                IpV4Layer = new PcapDotNet.Packets.IpV4.IpV4Layer(),
                PayloadLayer = new PayloadLayer(),
            };

            _layerManagerMock.Setup(x => x.ExtractLayersFromPacketAndReturnNewPacket(packet, packet.Ethernet.IpV4.Protocol))
                .Returns(customTcpPacket);

            // Act
            var result = _target.ExtractLayersFromPacket(packet);

            // Assert
            result.Should().BeOfType<CustomTcpPacket>();
        }
    }
}