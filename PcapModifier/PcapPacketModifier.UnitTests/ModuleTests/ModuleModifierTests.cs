using System;
using Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PcapDotNet.Packets.Transport;
using UnitTests.Shared.DummyObjects.Layers;
using PcapPacketModifier.Logic.UserExperience.Interfaces;
using PcapPacketModifier.Logic.Modules.Interfaces;
using PcapPacketModifier.Logic.Modules;

namespace UnitTests.ModuleTests
{
    [TestClass]
    public class ModuleModifierTests
    {
        private readonly Mock<IUserInputHandler> _userInputHandlerMock;
        private readonly Mock<ITextDisplayer> _textDisplayerMock;
        private readonly Mock<ISpecificModulesModifier> _specificModulesModifierMock;
        private readonly IDummyLayerCreator _dummyLayerCreator;
        private readonly IModuleModifier _target;

        public ModuleModifierTests()
        {
            _userInputHandlerMock = new Mock<IUserInputHandler>();
            _textDisplayerMock = new Mock<ITextDisplayer>();
            _specificModulesModifierMock = new Mock<ISpecificModulesModifier>();
            _dummyLayerCreator = new DummyLayerCreator();
            _target = new ModuleModifier(_userInputHandlerMock.Object,
                                                        _textDisplayerMock.Object,
                                                        _specificModulesModifierMock.Object);
        }

        [TestMethod]
        public void ModifyLayerModules_LayerIsNull_ExceptionThrown()
        {
            // Act
            Action action = () => _target.ChangeLayerModulesBasedOnUserInput<TcpLayer>(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void ModifyLayerModules_UserInputIsEmpty_LayerPropertiesAreSame()
        {
            // Arrange
            var layer = _dummyLayerCreator.GetDummyTcpLayer();

            var propertyInfo = layer.GetType().GetProperty("DestinationPort");

            _userInputHandlerMock.Setup(x => x.AskUserInputWhileInputContainsPatterns(propertyInfo))
                .Returns("");

            // Act
            var result = _target.ChangeLayerModulesBasedOnUserInput(layer);

            // Assert
            result.DestinationPort.Should().Be(80);
            result.Checksum.Should().Be(11010);
        }

        [TestMethod]
        public void ModifyLayerModules_LayerIsTcpLayer_LayerPropertyIsChangedToValue()
        {
            // Arrange
            var layer = _dummyLayerCreator.GetDummyTcpLayer();
            var propertyInfo = layer.GetType().GetProperty("Checksum");
            var userInput = "5";
            ushort parsedValue = 5;

            _userInputHandlerMock.Setup(x => x.AskUserInputWhileInputContainsPatterns(propertyInfo))
                .Returns(userInput);
            _specificModulesModifierMock.Setup(x => x.HandleSpecificModule(propertyInfo.PropertyType, userInput))
                .Returns(parsedValue);

            // Act
            var result = _target.ChangeLayerModulesBasedOnUserInput(layer);

            // Assert
            result.Checksum.Should().Be(parsedValue);
        }

        [TestMethod]
        public void ModifyLayerModules_ValuesWereRestoredToDefault_LayerHasDefaultValues()
        {
            // Arrange
            var layer = _dummyLayerCreator.GetDummyTcpLayer();
            var propertyInfo = layer.GetType().GetProperty("Checksum");
            var userInput = "0";
            ushort parsedValue = 0;

            _userInputHandlerMock.Setup(x => x.AskUserInputWhileInputContainsPatterns(propertyInfo))
                .Returns(userInput);
            _specificModulesModifierMock.Setup(x => x.HandleSpecificModule(propertyInfo.PropertyType, userInput))
                .Returns(parsedValue);

            // Act
            var result = _target.ChangeLayerModulesBasedOnUserInput(layer);

            // Assert
            result.Checksum.Should().Be(0);
        }

        [TestMethod]
        public void ModifyLayerModules_ErrorAccuredDuringObjectCalculation_ValuesAreLeftAsTheyWere()
        {
            // Arrange
            var layer = _dummyLayerCreator.GetDummyTcpLayer();
            var propertyInfo = layer.GetType().GetProperty("Checksum");
            var userInput = "asa";

            _userInputHandlerMock.Setup(x => x.AskUserInputWhileInputContainsPatterns(propertyInfo))
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