using System;
using Moq;
using FluentAssertions;
using PcapDotNet.Packets.Transport;
using PcapPacketModifier.Logic.Modules.Interfaces;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Logic.Layers;
using NUnit.Framework;

namespace UnitTests.LayerTests
{
    public class LayerModifierTests
    {
        private Mock<IModuleModifier> _moduleModifierMock;
        private ILayerModifier _target;

        [SetUp]
        public void Setup()
        {
            _moduleModifierMock = new Mock<IModuleModifier>();
            _target = new LayerModifier(_moduleModifierMock.Object);
        }

        [Test]
        public void ModifyLayer_LayerIsNull_ExceptionIsThrown()
        {
            // Act
            Action action = () => _target.ModifyLayer<TcpLayer>(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ModifyLayer_LayerIsOk_ReturnsModifiedLayer()
        {
            // Arrange
            TcpLayer tcpLayer = new TcpLayer();

            _moduleModifierMock.Setup(x => x.ChangeLayerModulesBasedOnUserInput(tcpLayer)).Returns(tcpLayer);

            // Act
            var layer =_target.ModifyLayer(tcpLayer);

            // Assert
            layer.Should().BeOfType<TcpLayer>();
            layer.Should().BeEquivalentTo(tcpLayer);
        }
    }
}