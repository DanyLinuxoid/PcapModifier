using System;
using Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using UnitTests.Shared.DummyObjects.Packets;
using UnitTests.Shared.DummyObjects.Layers;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Logic.Sender.Interfaces;
using PcapPacketModifier.Logic.Packets.Interfaces;
using PcapPacketModifier.Logic.Packets;
using PcapPacketModifier.Logic.Packets.Models;

namespace UnitTests.PacketTests
{
    [TestClass]
    public class PacketManagerTests
    {
        private readonly Mock<ILayerManager> _layerManagerMock;
        private readonly Mock<ILayerExtractor> _layerExtractor;
        private readonly Mock<ILayerModifier >_layerModifier;
        private readonly Mock<IPacketSender> _packetSenderMock;
        private readonly IDummyPacketBuilder _dummyPacketBuilder;
        private readonly IPacketManager _target;

        public PacketManagerTests()
        {
            _layerManagerMock = new Mock<ILayerManager>();
            _layerExtractor = new Mock<ILayerExtractor>();
            _layerModifier = new Mock<ILayerModifier>();
            _packetSenderMock = new Mock<IPacketSender>();
            _dummyPacketBuilder = new DummyPacketBuilder(new DummyLayerCreator());
            _target = new PacketManager(_layerManagerMock.Object, _packetSenderMock.Object);
        }

        [TestMethod]
        public void ExtractLayersFromPacket_PacketIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.ExtractLayersFromPacket(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
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