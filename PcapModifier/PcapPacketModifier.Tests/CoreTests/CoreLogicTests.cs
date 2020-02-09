using System;
using Moq;
using FluentAssertions;
using PcapDotNet.Packets;
using PcapPacketModifier.Logic.Packets.Interfaces;
using PcapPacketModifier.Logic.Tools.Interfaces;
using PcapPacketModifier.Logic.Core;
using PcapPacketModifier.Userdata.User;
using NUnit.Framework;

namespace UnitTests.CoreTests
{
    public class CoreLogicTests
    {
        private Mock<IPacketManager> _packetManagerMock;
        private Mock<IFileHandler> _fileHandlerMock;
        private ICoreLogic _target;

        [SetUp]
        public void Setup()
        {
            _packetManagerMock = new Mock<IPacketManager>();
            _fileHandlerMock = new Mock<IFileHandler>();
            _target = new CoreLogic(_packetManagerMock.Object, _fileHandlerMock.Object);
        }

        [Test]
        public void StartLogic_InputDataIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.ProcessLogic(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
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
