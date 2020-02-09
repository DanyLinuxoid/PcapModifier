using System;
using Moq;
using FluentAssertions;
using PcapDotNet.Packets.IpV4;
using UnitTests.Shared.DummyObjects.Layers;
using PcapPacketModifier.Logic.Logger.Interfaces;
using PcapPacketModifier.Logic.Factories.Interfaces;
using PcapPacketModifier.Logic.Factories;
using NUnit.Framework;

namespace UnitTests.FactoryTests
{
    public class GenericInstanceCreatorTests
    {
        private Mock<ISimpleLogger> _simpleLoggerMock;
        private IGenericInstanceCreator _target;

        [SetUp]
        public void Setup()
        {
            _simpleLoggerMock = new Mock<ISimpleLogger>();
            _target = new GenericInstanceCreator(_simpleLoggerMock.Object);
        }

        [Test]
        public void TryCreateNewInstance_CreateObjectWithoutParameters_ReturnsCreatedObject()
        {
            // Act 
            var newObject = _target.TryCreateNewInstance<DummyLayerCreator>();

            // Assert
            newObject.Should().BeOfType<DummyLayerCreator>();
            newObject.Should().NotBeNull();
        }

        [Test]
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

        [Test]
        public void TryCreateNewInstance_BadObjectTypeNotAStructOrClass_NotThrowingError()
        {
            // Act
            Action createdObject = () => _target.TryCreateNewInstance<Enum>();

            // Assert
            createdObject.Should().NotThrow<Exception>();
        }

        [Test]
        public void TryCreateNewInstance_BadObjectTypeNotAStructOrClass_ReturnsDefaultValueForType()
        {
            // Act
            var createdObject = _target.TryCreateNewInstance<Enum>();

            // Assert
            createdObject.Should().BeNull();
        }
    }
}
