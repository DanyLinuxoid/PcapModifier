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
using PcapPacketModifier.Logic.Factories.Interfaces;

namespace UnitTests.PacketTests
{
    public class PacketManagerTests
    {
        private Mock<ILayerExtractor> _layerExtractor;
        private Mock<ILayerModifier >_layerModifier;
        private Mock<IPacketSender> _packetSenderMock;
        private Mock<ILayerExchanger> _layerExchangerMock;
        private Mock<IPacketFactory> _packetFactoryMock;
        private IDummyPacketBuilder _dummyPacketBuilder;
        private IPacketManager _target;

        [SetUp]
        public void Setup()
        {
            _layerExtractor = new Mock<ILayerExtractor>();
            _layerModifier = new Mock<ILayerModifier>();
            _packetSenderMock = new Mock<IPacketSender>();
            _layerExchangerMock = new Mock<ILayerExchanger>();
            _packetFactoryMock = new Mock<IPacketFactory>();
            _dummyPacketBuilder = new DummyPacketBuilder(new DummyLayerCreator());
            _target = new PacketManager(_packetSenderMock.Object, _packetFactoryMock.Object);
        }
    }
}