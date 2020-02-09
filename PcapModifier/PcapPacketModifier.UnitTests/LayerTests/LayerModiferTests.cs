using System;
using Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PcapDotNet.Packets.Transport;
using PcapPacketModifier.Logic.Modules.Interfaces;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Logic.Layers;

namespace UnitTests.LayerTests
{
    [TestClass]
    public class LayerModifierTests
    {
        private readonly Mock<IModuleModifier> _moduleModifierMock;
        private readonly ILayerModifier _target;

        public LayerModifierTests()
        {
            _moduleModifierMock = new Mock<IModuleModifier>();
            _target = new LayerModifier(_moduleModifierMock.Object);
        }

        [TestMethod]
        public void ModifyLayer_LayerIsNull_ExceptionIsThrown()
        {
            // Act
            Action action = () => _target.ModifyLayer<TcpLayer>(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
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