using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using PcapDotNet.Packets;
using UnitTests.Shared.PathProvider;
using PcapPacketModifier.Logic.Tools.Interfaces;
using PcapPacketModifier.Logic.Tools;
using Moq;

namespace UnitTests.ToolsTests
{
    [TestClass]
    public class FileHandlerTests
    {
        private readonly Mock<IPathProvider>_pathProviderMock;
        private readonly IFileHandler _target;
        private readonly ITestingPathProvider _testingPathProvider;

        private string FullPathToFile;
        private string FullPathToTextFile;

        public FileHandlerTests()
        {
            _testingPathProvider = new TestingPathProvider();
            _pathProviderMock = new Mock<IPathProvider>();
            _target = new FileHandler(_pathProviderMock.Object);
            FullPathToFile = _testingPathProvider.GetPathToTestPacket();
            FullPathToTextFile = _testingPathProvider.GetPathToTestLog();
        }

        [TestMethod]
        public void FileHandler_InvalidPathToFile_ReturnsNull()
        {
            // Act
            var result = _target.TryOpenUserPacketFromFile("test");

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void FileHandler_CorrectPathToFile_SuccessfullyOpensFile()
        {
            // Act
            Packet result = _target.TryOpenUserPacketFromFile(FullPathToFile);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Packet>();
        }

        [TestMethod]
        public void CheckIfFileExists_DoesNotExist_ReturnsFalse()
        {
            // Act
            var result = _target.IsFileExisting("somepath");

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void CheckIfFileExists_DoesNotExist_ReturnsTrue()
        {
            // Act
            var result = _target.IsFileExisting(FullPathToFile);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void TryCreateFile_ValidPath_FileIsCreated()
        {
            // Act
            var result = _target.TryCreateSimpleEmptyFile("anypath");

            // Assert
            result.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("../../../../../../../")]
        [DataRow("........")]
        public void TryCreateFile_InvalidPath_FileIsNotCreated(string path)
        {
            // Act
            var result = _target.TryCreateSimpleEmptyFile(path);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void TryWriteMessageToFile_BadPath_ErrorIsPrinted()
        {
            // Act
            Action action = () => _target.TryWriteMessageToFile("any message", "some invalid path");

            // Assert
            action.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void TryWriteMessageToFile_PathIsOk_WritesMessageToFile()
        {
            // Arrange
            var messageToWrite = "any message";

            // Act
            Action action = () => _target.TryWriteMessageToFile(messageToWrite, FullPathToTextFile);

            // Assert
            action.Should().NotThrow<Exception>();
            var stringFromFile = GetTextFromFile(FullPathToTextFile);
            stringFromFile.Should().NotBeNullOrEmpty();
            stringFromFile.Should().Contain(messageToWrite);
            FlushTextFile();
        }

        private string GetTextFromFile(string path)
        {
            string textToReturn = null;
            try
            {
                using (StreamReader sr = new StreamReader(FullPathToTextFile))
                {
                    textToReturn = sr.ReadToEnd();
                }
            }
            catch(Exception ex) { }

            return textToReturn;
        }

        [TestCategory("File Cleanup")]
        private void FlushTextFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FullPathToTextFile))
                {
                    sw.Flush();
                }
            }
            catch (Exception ex) { }
        }
    }
}