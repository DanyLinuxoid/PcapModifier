using System;
using Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Shared.PathProvider;
using PcapPacketModifier.Logic.Tools.Interfaces;
using PcapPacketModifier.Logic.Logger;

namespace UnitTests.LoggerTests
{
    [TestClass]
    public class LoggerTests
    {
        private readonly Mock<IFileHandler> _fileHandlerMock;
        private readonly Mock<ITestingPathProvider> _pathProviderMock;
        private readonly SimpleFileLogger _target;
        private string _filePath;

        public LoggerTests()
        {
            _fileHandlerMock = new Mock<IFileHandler>();
            _target = new SimpleFileLogger(_fileHandlerMock.Object);
            _pathProviderMock = new Mock<ITestingPathProvider>();
            _filePath = _pathProviderMock.Object.GetPathToTestLog();
        }

        [TestMethod]
        public void WriteLog_EmptyPath_PathShouldBeEmpty()
        {
            // Act
            bool path = string.IsNullOrWhiteSpace(_target.Path);

            // Assert
            path.Should().BeTrue();
        }

        [TestMethod]
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

        [TestMethod]
        public void WriteLog_EmptyMessage_ThrowsError()
        {
            // Assert
            _fileHandlerMock.Setup(x => x.PathProvider.GetDefaultPathForLog()).Returns(_filePath);

            // Act
            Action action = () => _target.WriteLog("");

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
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

        [TestMethod]
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