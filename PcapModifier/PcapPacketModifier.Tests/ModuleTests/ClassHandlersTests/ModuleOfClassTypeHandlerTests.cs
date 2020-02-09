using System.Text;
using Moq;
using FluentAssertions;
using UnitTests.Shared.DummyObjects.Classes;
using PcapDotNet.Packets;
using PcapPacketModifier.Logic.Modules.ClassHandlers.Interfaces;
using PcapPacketModifier.Logic.Modules.ClassHandlers;
using NUnit.Framework;

namespace UnitTests.ModuleTests.ClassHandlersTests
{
    public class ModuleOfClassTypeHandlerTests
    {
        private Mock<IModuleClassTypeArgumentsHandler> _moduleOfClassTypeArgumentHandlerMock;
        private IModuleClassTypeHandler _target;

        [SetUp]
        public void Setup()
        {
            _moduleOfClassTypeArgumentHandlerMock = new Mock<IModuleClassTypeArgumentsHandler>();
            _target = new ModuleClassTypeHandler(_moduleOfClassTypeArgumentHandlerMock.Object);
        }

        [Test]
        public void HandleModuleClases_InvalidClass_ReturnsNull()
        {
            // Act
            var result = _target.HandleModuleClases(typeof(DummyClass), "any");

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void HandleModuleClases_ValidClass_ReturnsObject()
        {
            // Arrange
            string userInput = "someInput";
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes(userInput);

            _moduleOfClassTypeArgumentHandlerMock.Setup(x => x.GetArgumentsForDatagramFromUserInput(userInput))
                .Returns(buffer);

            // Act
            var result = _target.HandleModuleClases(typeof(Datagram), userInput);

            // Assert
            result.Should().BeOfType<Datagram>();
        }
    }
}
