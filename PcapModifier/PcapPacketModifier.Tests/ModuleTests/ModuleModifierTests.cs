using System;
using Moq;
using FluentAssertions;
using PcapDotNet.Packets.Transport;
using UnitTests.Shared.DummyObjects.Layers;
using PcapPacketModifier.Logic.UserExperience.Interfaces;
using PcapPacketModifier.Logic.Modules.Interfaces;
using PcapPacketModifier.Logic.Modules;
using NUnit.Framework;

namespace UnitTests.ModuleTests
{
    public class ModuleModifierTests
    {
        private Mock<IUserExperience> _userExperienceMock;
        private Mock<ISpecificModulesModifier> _specificModulesModifierMock;
        private Mock<ITextDisplayer> _textDisplayerMock;
        private Mock<IUserInputHandler> _userInputHandlerMock;
        private IDummyLayerCreator _dummyLayerCreator;
        private IModuleModifier _target;

        [SetUp]
        public void Setup()
        {
            _userExperienceMock = new Mock<IUserExperience>();
            _textDisplayerMock = new Mock<ITextDisplayer>();
            _userInputHandlerMock = new Mock<IUserInputHandler>();
            _userExperienceMock.Setup(x => x.UserTextDisplayer).Returns(_textDisplayerMock.Object);
            _userExperienceMock.Setup(x => x.UserInputHandler).Returns(_userInputHandlerMock.Object);
            _specificModulesModifierMock = new Mock<ISpecificModulesModifier>();
            _dummyLayerCreator = new DummyLayerCreator();
            _target = new ModuleModifier(_specificModulesModifierMock.Object,
                                                        _userExperienceMock.Object);
        }

        [Test]
        public void ModifyLayerModules_LayerIsNull_ExceptionThrown()
        {
            // Act
            Action action = () => _target.ChangeLayerModulesBasedOnUserInput<TcpLayer>(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ModifyLayerModules_UserInputIsEmpty_LayerPropertiesAreSame()
        {
            // Arrange
            var layer = _dummyLayerCreator.GetDummyTcpLayer();

            var propertyInfo = layer.GetType().GetProperty("DestinationPort");

            _userExperienceMock.Setup(x => x.UserInputHandler.AskUserInputWhileInputContainsPatterns(propertyInfo))
                .Returns("");

            // Act
            var result = _target.ChangeLayerModulesBasedOnUserInput(layer);

            // Assert
            result.DestinationPort.Should().Be(80);
            result.Checksum.Should().Be(11010);
            result.AcknowledgmentNumber.Should().Be(321);
        }

        [Test]
        public void ModifyLayerModules_LayerIsTcpLayer_LayerPropertyIsChangedToValue()
        {
            // Arrange
            var layer = _dummyLayerCreator.GetDummyTcpLayer();
            var propertyInfo = layer.GetType().GetProperty("Checksum");
            var userInput = "10";
            ushort parsedValue = 10;

            _userExperienceMock.Setup(x => x.UserInputHandler.AskUserInputWhileInputContainsPatterns(propertyInfo))
                .Returns(userInput);
            _specificModulesModifierMock.Setup(x => x.HandleSpecificModule(propertyInfo.PropertyType, userInput))
                .Returns(parsedValue);

            // Act
            var result = _target.ChangeLayerModulesBasedOnUserInput(layer);

            // Assert
            result.Checksum.Should().Be(parsedValue);
        }

        [Test]
        public void ModifyLayerModules_ValuesWereRestoredToDefault_LayerHasDefaultValues()
        {
            // Arrange
            var layer = _dummyLayerCreator.GetDummyTcpLayer();
            var propertyInfo = layer.GetType().GetProperty("Checksum");
            var userInput = "0";
            ushort parsedValue = 0;

            _userExperienceMock.Setup(x => x.UserInputHandler.AskUserInputWhileInputContainsPatterns(propertyInfo))
                .Returns(userInput);
            _specificModulesModifierMock.Setup(x => x.HandleSpecificModule(propertyInfo.PropertyType, userInput))
                .Returns(parsedValue);

            // Act
            var result = _target.ChangeLayerModulesBasedOnUserInput(layer);

            // Assert
            result.Checksum.Should().Be(0);
        }

        [Test]
        public void ModifyLayerModules_ErrorAccuredDuringObjectCalculation_ValuesAreLeftAsTheyWere()
        {
            // Arrange
            var layer = _dummyLayerCreator.GetDummyTcpLayer();
            var propertyInfo = layer.GetType().GetProperty("Checksum");
            var userInput = "asa";

            _userExperienceMock.Setup(x => x.UserInputHandler.AskUserInputWhileInputContainsPatterns(propertyInfo))
                .Returns(userInput);
            _specificModulesModifierMock.Setup(x => x.HandleSpecificModule(propertyInfo.PropertyType, userInput))
                .Returns(null);

            // Act
            var result = _target.ChangeLayerModulesBasedOnUserInput(layer);

            // Assert
            result.Checksum.Should().Be(11010);
        }
    }
}