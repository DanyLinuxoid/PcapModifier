using System;
using Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PcapDotNet.Packets;
using PcapPacketModifier.Logic.Packets.Interfaces;
using PcapPacketModifier.Logic.Tools.Interfaces;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Logic.Core;
using PcapPacketModifier.Userdata.User;

namespace UnitTests.CoreTests
{
    [TestClass]
    public class CoreLogicTests
    {
        private readonly Mock<IPacketManager> _packetManagerMock;
        private readonly Mock<IFileHandler> _fileHandlerMock;
        private readonly ICoreLogic _target;

        public CoreLogicTests()
        {
            _packetManagerMock = new Mock<IPacketManager>();
            _fileHandlerMock = new Mock<IFileHandler>();
            _target = new CoreLogic(_packetManagerMock.Object, _fileHandlerMock.Object);
        }

        [TestMethod]
        public void StartLogic_InputDataIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.ProcessLogic(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void StartLogic_BadPacketWasReturned_ThrowsError()
        {
            // Arrange
            _fileHandlerMock.Setup(x => x.TryOpenUserPacketFromFile(It.IsAny<string>()))
                .Returns((Packet)null);

            // Act
            Action action = () => _target.ProcessLogic(new UserInputData());

            // Assert
            action.Should().ThrowExactly<InvalidOperationException>();
        }
    }
}
