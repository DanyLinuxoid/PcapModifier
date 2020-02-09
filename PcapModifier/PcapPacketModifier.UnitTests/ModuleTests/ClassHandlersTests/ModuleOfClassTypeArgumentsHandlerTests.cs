using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PcapPacketModifier.Logic.Modules.ClassHandlers;
using PcapPacketModifier.Logic.Modules.ClassHandlers.Interfaces;

namespace UnitTests.ModuleTests.ClassHandlersTests
{
    [TestClass]
    public class ModuleOfClassTypeArgumentsHandlerTests
    {
        private readonly IModuleClassTypeArgumentsHandler _target;
        
        public ModuleOfClassTypeArgumentsHandlerTests()
        {
            _target = new ModuleClassTypeArgumentsHandler();
        }

        [TestMethod]
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
