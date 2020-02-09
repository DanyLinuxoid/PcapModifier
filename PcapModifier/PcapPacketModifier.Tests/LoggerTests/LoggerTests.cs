using System;
using Moq;
using FluentAssertions;
using UnitTests.Shared.PathProvider;
using PcapPacketModifier.Logic.Tools.Interfaces;
using PcapPacketModifier.Logic.Logger;
using NUnit.Framework;

namespace UnitTests.LoggerTests
{
    public class LoggerTests
    {
        private Mock<IFileHandler> _fileHandlerMock;
        private Mock<ITestingPathProvider> _pathProviderMock;
        private SimpleFileLogger _target;
        private string _filePath;

        [SetUp]
        public void Setup()
        {
            _fileHandlerMock = new Mock<IFileHandler>();
            _target = new SimpleFileLogger(_fileHandlerMock.Object);
            _pathProviderMock = new Mock<ITestingPathProvider>();
            _filePath = _pathProviderMock.Object.GetPathToTestLog();
        }

        [Test]
        public void WriteLog_EmptyPath_CreatesPathForFile()
        {
            // Arrange
            _fileHandlerMock.Setup(x => x.IsFileExisting("path")).Returns(true);
            _fileHandlerMock.Setup(p => p.PathProvider.GetDefaultPathForLog()).Returns("path");

            // Act
            _target.WriteLog("test");

            // Assert
            _target.Path.Should().NotBeNullOrWhiteSpace();
            _target.Path.Should().BeEquivalentTo("path");
        }

        [Test]
        public void WriteLog_EmptyMessage_ThrowsError()
        {
            // Assert
            _fileHandlerMock.Setup(x => x.PathProvider.GetDefaultPathForLog()).Returns(_filePath);

            // Act
            Action action = () => _target.WriteLog("");

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void WriteLog_NoLogFile_LogFileIsCreated()
        {
            // Arrange
            _fileHandlerMock.Setup(x => x.TryCreateSimpleEmptyFile("path")).Returns(true);
            _fileHandlerMock.Setup(p => p.PathProvider.GetDefaultPathForLog()).Returns("path");

            // Act
            _target.WriteLog("test");

            _target.Path.Should().NotBeNullOrWhiteSpace();
            _target.Path.Should().BeEquivalentTo("path");
        }

        [Test]
        public void WriteLog_NoLogFileAndFailedCreatingLogFile_ExceptionIsThrown()
        {
            // Assert
            _fileHandlerMock.Setup(p => p.PathProvider.GetDefaultPathForLog()).Returns(_filePath);

            // Act
            Action action = () => _target.WriteLog("test");

            // Assert
            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("Path - Error creating file");
        }
    }
}