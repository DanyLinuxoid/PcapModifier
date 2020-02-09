﻿using System;
using Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Ethernet;
using PcapPacketModifier.Logic.Factories.Interfaces;
using PcapPacketModifier.Logic.Modules.StructHandlers.Interfaces;
using PcapPacketModifier.Logic.Modules.StructHandlers;

namespace UnitTests.ModuleTests.StructHandlersTests
{
    [TestClass]
    public class ModuleStructHandlerTests
    {
        private readonly Mock<IGenericInstanceCreator> _genericInstanceCreator;
        private readonly Mock<IModuleStructArgumentsHandler> _moduleStructArgumentsHandler;
        private readonly IModuleStructHandler _target;

        public ModuleStructHandlerTests()
        {
            _genericInstanceCreator = new Mock<IGenericInstanceCreator>();
            _moduleStructArgumentsHandler = new Mock<IModuleStructArgumentsHandler>();
            _target = new ModuleStructHandler(_genericInstanceCreator.Object,
                                                                _moduleStructArgumentsHandler.Object);
        }

        [TestMethod]
        public void CheckIfTypeIsCommonStructAndReturnObject_IsValidType_ReturnsParsedValue()
        {
            // Arrange
            Type numberType = typeof(int);

            // Act
            var result = _target.CheckIfTypeIsCommonStructAndReturnObject(numberType, "1");

            // Assert
            result.Should().BeOfType<int>();
        }

        [TestMethod]
        public void CheckIfTypeIsCommonStructAndReturnObject_IsValidNullableType_ReturnsParsedValue()
        {
            // Arrange
            Type numberType = typeof(int?);

            // Act
            var result = _target.CheckIfTypeIsCommonStructAndReturnObject(numberType, "1");

            // Assert
            result.Should().BeOfType<int>();
        }

        [TestMethod]
        public void CheckIfTypeIsCommonStructAndReturnObject_IsInvalidStruct_ReturnsNull()
        {
            // Arrange
            Type badNumberType = typeof(IpV4Fragmentation);

            // Act
            var result = _target.CheckIfTypeIsCommonStructAndReturnObject(badNumberType, "1");

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void CheckIfStructMustHaveArgumentsAndReturnObject_IsInvalidStruct_ReturnsNull()
        {
            // Arrange
            Type badNumberType = new int().GetType();

            // Act
            var result = _target.CheckIfStructMustHaveArgumentsAndReturnObject(badNumberType, "1");

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void CheckIfStructMustHaveArgumentsAndReturnObject_IsValidStructWithOneArgument_ReturnsObject()
        {
            // Arrange
            string input = "FF:FF:FF:FF:FF:FF";
            MacAddress macAddress = new MacAddress(input);
            Type macType = macAddress.GetType();

            _genericInstanceCreator.Setup(x => x.TryCreateNewInstance<MacAddress?>(input))
                .Returns(macAddress);

            // Act
            var result = _target.CheckIfStructMustHaveArgumentsAndReturnObject(macType, input);

            // Assert
            result.Should().BeOfType<MacAddress>();
        }

        [TestMethod]
        public void CheckIfStructMustHaveArgumentsAndReturnObject_IsValidStructWithManyArguments_ReturnsObject()
        {
            // Arrange
            ushort offset = 80;
            IpV4FragmentationOptions options = IpV4FragmentationOptions.MoreFragments;
            IpV4Fragmentation ipV4Fragmentation = new IpV4Fragmentation(options, offset);
            string input = $"{options}, {offset}";
            object[] array = new object[2] { options, offset };

            _genericInstanceCreator.Setup(x => x.TryCreateNewInstance<IpV4Fragmentation?>(array))
                .Returns(ipV4Fragmentation);
            _moduleStructArgumentsHandler.Setup(x => x.InputToParametersForIpV4Fragmentation(input))
                .Returns(array);

            // Act
            var result = _target.CheckIfStructMustHaveArgumentsAndReturnObject(typeof(IpV4Fragmentation), input);

            // Assert
            result.Should().BeOfType<IpV4Fragmentation>();
            var pp = result.GetType().GetProperty("Offset").GetValue(result).Should().Be(offset);
            result.GetType().GetProperty("Options").GetValue(result).Should().Be(options);
        }

        [TestMethod]
        public void CheckIfStructMustHaveArgumentsAndReturnObject_InvalidType_ReturnsObject()
        {
            // Act
            var result = _target.CheckIfTypeIsCommonStructAndReturnObject(typeof(int), "any");

            // Assert
            result.Should().BeNull();
        }
    }
}
