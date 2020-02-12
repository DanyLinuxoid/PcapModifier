using System;
using Moq;
using FluentAssertions;
using System.Reflection;
using UnitTests.Shared.DummyObjects.Classes;
using PcapPacketModifier.Logic.UserExperience.Interfaces;
using PcapPacketModifier.Logic.UserExperience;
using NUnit.Framework;

namespace UnitTests.UserExperienceTests
{
    public class UserInputHandlerTests
    {
        private Mock<ITextDisplayer> _textDisplayerMock;
        private Mock<IConsoleWrapper> _consoleWrapperMock;
        private IUserInputHandler _target;

        [SetUp]
        public void Setup()
        {
            _textDisplayerMock = new Mock<ITextDisplayer>();
            _consoleWrapperMock = new Mock<IConsoleWrapper>();
            _target = new UserInputHandler(_textDisplayerMock.Object, _consoleWrapperMock.Object);
        }

        [Test]
        public void YesNoAbort_YIsPressed_ReturnsTrue()
        {
            // Arrange
            _consoleWrapperMock.Setup(x => x.GetConsolePressedKey()).Returns(ConsoleKey.Y);

            // Act
            bool result = _target.IsUserWantsToContinue();

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void YesNoAbort_AIsPressed_ReturnsTrue()
        {
            // Arrange
            _consoleWrapperMock.Setup(x => x.GetConsolePressedKey()).Returns(ConsoleKey.A);

            // Act
            bool result = _target.IsUserWantsToContinue();

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void AskUserInputWhileInputContainsPatterns_PropertyIsNull_ErrorThrows()
        {
            // Act
            Action action = () => _target.AskUserInputWhileInputContainsPatterns(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AskUserInputWhileInputContainsPatterns_UserInputDoesNotContainsPatterns_NoLoop()
        {
            // Arrange
            string userInput = "test";
            DummyClass dummyClass = new DummyClass();
            PropertyInfo propertyInfo = dummyClass.GetType().GetProperty("DummyEnumType");

            _consoleWrapperMock.Setup(x => x.GetUserInputFromConsole()).Returns(userInput);

            // Act
            string result = _target.AskUserInputWhileInputContainsPatterns(propertyInfo);

            // Assert
            result.Should().BeEquivalentTo(userInput);
        }

        [Test]
        public void GetUserInput_SomeInputIsProvided_InputIsReturned()
        {
            // Arrange
            string userInput = "test";

            _consoleWrapperMock.Setup(x => x.GetUserInputFromConsole()).Returns(userInput);

            // Act
            string result = _target.GetUserInput();

            // Assert
            result.Should().BeEquivalentTo(userInput);
        }

        [Test]
        public void IsUserInputContainingSpecificPattern_PropertyIsNull_ExceptionThrows()
        {
            // Act
            Action action = () => _target.IsUserInputContainingSpecificPattern("", null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void IsUserInputContainingSpecificPattern_UserInputContainsH_ReturnsTrue()
        {
            // Arrange
            DummyClass dummyClass = new DummyClass();
            PropertyInfo propertyInfo = dummyClass.GetType().GetProperty("DummyEnumType");
            string userInput = "-h";

            // Act
            bool result = _target.IsUserInputContainingSpecificPattern(userInput, propertyInfo);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsUserInputContainingSpecificPattern_UserInputDoesNotContainsPatterns_ReturnsFalse()
        {
            // Arrange
            DummyClass dummyClass = new DummyClass();
            PropertyInfo propertyInfo = dummyClass.GetType().GetProperty("DummyEnumType");
            string userInput = "test";

            // Act
            bool result = _target.IsUserInputContainingSpecificPattern(userInput, propertyInfo);

            // Assert
            result.Should().BeFalse();
        }
    }
}