using System.Text;
using Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Shared.DummyObjects.Classes;
using PcapDotNet.Packets;
using PcapPacketModifier.Logic.Modules.ClassHandlers.Interfaces;
using PcapPacketModifier.Logic.Modules.ClassHandlers;

namespace UnitTests.ModuleTests.ClassHandlersTests
{
    [TestClass]
    public class ModuleOfClassTypeHandlerTests
    {
        private readonly Mock<IModuleClassTypeArgumentsHandler> _moduleOfClassTypeArgumentHandlerMock;
        private readonly IModuleClassTypeHandler _target;

        public ModuleOfClassTypeHandlerTests()
        {
            _moduleOfClassTypeArgumentHandlerMock = new Mock<IModuleClassTypeArgumentsHandler>();
            _target = new ModuleClassTypeHandler(_moduleOfClassTypeArgumentHandlerMock.Object);
        }

        [TestMethod]
        public void HandleModuleClases_InvalidClass_ReturnsNull()
        {
            // Act
            var result = _target.HandleModuleClases(typeof(DummyClass), "any");

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
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
