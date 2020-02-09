using System.Text;
using FluentAssertions;
using NUnit.Framework;
using PcapPacketModifier.Logic.Modules.ClassHandlers;
using PcapPacketModifier.Logic.Modules.ClassHandlers.Interfaces;

namespace UnitTests.ModuleTests.ClassHandlersTests
{
    public class ModuleOfClassTypeArgumentsHandlerTests
    {
        private IModuleClassTypeArgumentsHandler _target;

        [SetUp]
        public void Setup()
        {
            _target = new ModuleClassTypeArgumentsHandler();
        }

        [Test]
        public void GetArgumentsForDatagramFromUserInput_IsValidEncoding_ReturnsBytes()
        {
            // Arrange
            string input = "some"; 
            var bytes = Encoding.ASCII.GetBytes(input);

            // Act
            var result = _target.GetArgumentsForDatagramFromUserInput(input);

            // Assert
            result.Should().BeEquivalentTo(bytes);
        }
    }
}
