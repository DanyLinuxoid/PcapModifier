using System;
using FluentAssertions;
using System.IO;
using PcapDotNet.Packets;
using UnitTests.Shared.PathProvider;
using PcapPacketModifier.Logic.Tools.Interfaces;
using PcapPacketModifier.Logic.Tools;
using Moq;
using NUnit.Framework;

namespace UnitTests.ToolsTests
{
    public class FileHandlerTests
    {
        private Mock<IPathProvider>_pathProviderMock;
        private IFileHandler _target;
        private ITestingPathProvider _testingPathProvider;

        private string FullPathToFile;
        private string FullPathToTextFile;

        [SetUp]
        public void Setup()
        {
            _testingPathProvider = new TestingPathProvider();
            _pathProviderMock = new Mock<IPathProvider>();
            _target = new FileHandler(_pathProviderMock.Object);
            FullPathToFile = _testingPathProvider.GetPathToTestPacket();
            FullPathToTextFile = _testingPathProvider.GetPathToTestLog();
        }

        [Test]
        public void FileHandler_InvalidPathToFile_ReturnsNull()
        {
            // Act
            var result = _target.TryOpenUserPacketFromFile("test");

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void FileHandler_CorrectPathToFile_SuccessfullyOpensFile()
        {
            // Act
            Packet result = _target.TryOpenUserPacketFromFile(FullPathToFile);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Packet>();
        }

        [Test]
        public void CheckIfFileExists_DoesNotExist_ReturnsFalse()
        {
            // Act
            var result = _target.IsFileExisting("somepath");

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void CheckIfFileExists_DoesNotExist_ReturnsTrue()
        {
            // Act
            var result = _target.IsFileExisting(FullPathToFile);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void TryCreateFile_ValidPath_FileIsCreated()
        {
            // Act
            var result = _target.TryCreateSimpleEmptyFile("anypath");

            // Assert
            result.Should().BeTrue();
        }

        //[DataTestMethod]
        //[DataRow("../../../../../../../")]
        //[DataRow("........")]
        //public void TryCreateFile_InvalidPath_FileIsNotCreated(string path)
        //{
        //    // Act
        //    var result = _target.TryCreateSimpleEmptyFile(path);

        //    // Assert
        //    result.Should().BeFalse();
        //}

        [Test]
        public void TryWriteMessageToFile_BadPath_ErrorIsPrinted()
        {
            // Act
            Action action = () => _target.TryWriteMessageToFile("any message", "some invalid path");

            // Assert
            action.Should().NotThrow<Exception>();
        }

        [Test]
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