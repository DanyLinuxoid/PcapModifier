using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using FluentAssertions;
using PcapDotNet.Packets.IpV4;
using UnitTests.Shared.DummyObjects.Layers;
using PcapPacketModifier.Logic.Logger.Interfaces;
using PcapPacketModifier.Logic.Factories.Interfaces;
using PcapPacketModifier.Logic.Factories;

namespace UnitTests.FactoryTests
{
    [TestClass]
    public class GenericInstanceCreatorTests
    {
        private readonly Mock<ISimpleLogger> _simpleLoggerMock;
        private readonly IGenericInstanceCreator _target;

        public GenericInstanceCreatorTests()
        {
            _simpleLoggerMock = new Mock<ISimpleLogger>();
            _target = new GenericInstanceCreator(_simpleLoggerMock.Object);
        }

        [TestMethod]
        public void TryCreateNewInstance_CreateObjectWithoutParameters_ReturnsCreatedObject()
        {
            // Act 
            var newObject = _target.TryCreateNewInstance<DummyLayerCreator>();

            // Assert
            newObject.Should().BeOfType<DummyLayerCreator>();
            newObject.Should().NotBeNull();
        }

        [TestMethod]
        public void TryCreateNewInstance_CreateObjectWithParameters_ReturnsCreatedObjectWithParameters()
        {
            // Arrange 
            var firstParam = IpV4FragmentationOptions.MoreFragments;
            byte secondParam = 80;
            var parameters = new object[] { firstParam, secondParam };

            // Act
            var createdObject = _target.TryCreateNewInstance<IpV4Fragmentation>(parameters);

            // Assert
            createdObject.Should().BeOfType<IpV4Fragmentation>();
            createdObject.Should().NotBeNull();
        }

        [TestMethod]
        public void TryCreateNewInstance_BadObjectTypeNotAStructOrClass_NotThrowingError()
        {
            // Act
            Action createdObject = () => _target.TryCreateNewInstance<Enum>();

            // Assert
            createdObject.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void TryCreateNewInstance_BadObjectTypeNotAStructOrClass_ReturnsDefaultValueForType()
        {
            // Act
            var createdObject = _target.TryCreateNewInstance<Enum>();

            // Assert
            createdObject.Should().BeNull();
        }
    }
}
