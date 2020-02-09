using System;
using System.Text;
using Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Logic.Modules.ClassHandlers.Interfaces;
using PcapPacketModifier.Logic.Modules.StructHandlers.Interfaces;
using PcapPacketModifier.Logic.Modules.Interfaces;
using PcapPacketModifier.Logic.Modules;

namespace UnitTests.ModuleTests
{
    [TestClass]
    public class SpecificModulesModifierTests
    {
        private readonly Mock<IModuleClassTypeHandler> _moduleOfClassTypeHandlerMock;
        private readonly Mock<IModuleStructHandler> _moduleStructHandlerMock;
        private readonly ISpecificModulesModifier _target;

        public SpecificModulesModifierTests()
        {
            _moduleOfClassTypeHandlerMock = new Mock<IModuleClassTypeHandler>();
            _moduleStructHandlerMock = new Mock<IModuleStructHandler>();
            _target = new SpecificModulesModifier(_moduleStructHandlerMock.Object,
                                                                    _moduleOfClassTypeHandlerMock.Object);
        }

        [TestMethod]
        public void HandleSpecificModule_TypeIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.HandleSpecificModule(null, "test");

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void HandleSpecificModule_UserInputIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.HandleSpecificModule(It.IsAny<Type>(), "");

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void HandleSpecificModule_IsValidStructWIthoutArguments_ReturnsParsedValue()
        {
            // Arrange
            string userInput = "1";
            uint number = 1;
            Type boxedUint = number.GetType();

            _moduleStructHandlerMock.Setup(x => x.CheckIfTypeIsCommonStructAndReturnObject(boxedUint, userInput))
                .Returns(number);

            // Act
            var result = _target.HandleSpecificModule(boxedUint, userInput);

            // Assert
            result.Should().Be(1);
            result.Should().BeOfType<uint>();
        }

        [TestMethod]
        public void HandleSpecificModule_IsValidStructWIthArguments_ReturnsParsedValue()
        {
            // Arrange
            string userInput = "None, 80";
            string stringNumber = "80";
            string enumOption = "None";
            string[] arrayOfUserValues = { enumOption, stringNumber };
            ushort number = 80;

            IpV4FragmentationOptions ipV4FragmentationOptions = IpV4FragmentationOptions.MoreFragments;
            Type typeOfEnum = typeof(IpV4Fragmentation);
            IpV4Fragmentation ipV4Fragmentation = new IpV4Fragmentation(ipV4FragmentationOptions, number);

            object[] parameters = new object[2] { ipV4FragmentationOptions, number };

            _moduleStructHandlerMock.Setup(x => x.CheckIfStructMustHaveArgumentsAndReturnObject(typeOfEnum, userInput))
                .Returns(ipV4Fragmentation);

            // Act
            var result = _target.HandleSpecificModule(typeOfEnum, userInput);

            // Assert
            result.Should().BeOfType<IpV4Fragmentation>();
            result.GetType().GetProperty("Offset").GetValue(result).Should().Be(80);
            result.GetType().GetProperty("Options").GetValue(result).Should().Be(IpV4FragmentationOptions.MoreFragments);
        }

        [TestMethod]
        public void HandleSpecificModule_IsValidClass_ReturnsValidClass()
        {
            // Arrange
            string userInput = "some random data";
            byte[] data = Encoding.ASCII.GetBytes(userInput); 
            Datagram dataGram = new Datagram(data);
            Type dataType = dataGram.GetType();

            _moduleOfClassTypeHandlerMock.Setup(x => x.HandleModuleClases(dataType, userInput))
                .Returns(dataGram);

            // Act
            var result = _target.HandleSpecificModule(dataType, userInput);

            // Assert
            result.Should().BeOfType<Datagram>();
        }
    }
}