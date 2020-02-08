using PcapPacketModifier.Logic.Tools.Interfaces;
using System;

namespace PcapPacketModifier.Logic.Logger
{
    /// <summary>
    /// Respresents very simple file logger, basic functionality
    /// </summary>
    public class SimpleFileLogger : SimpleLoggerBase
    {
        /// <summary>
        /// Path to log file
        /// </summary>
        public string Path { get; private set; }

        private readonly IFileHandler _fileHandler;

        public SimpleFileLogger(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        /// <summary>
        /// Checks if log exists, if log does not exists - creates it and writes message in it.
        /// </summary>
        /// <param name="message">Message to write in log</param>
        public override void WriteLog(string message, string path = null)
        {
            Path = path;
            if (string.IsNullOrWhiteSpace(path))
            {
                Path = _fileHandler.PathProvider.GetDefaultPathForLog();
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            CheckIfFileExistsAndTryToCreateIfNot();

            _fileHandler.TryWriteMessageToFile(message, Path);
        }

        /// <summary>
        /// Before writing to log, call this method, to ensure, that file exists, or that it can be created
        /// </summary>
        /// <param name="path">Path to file</param>
        private void CheckIfFileExistsAndTryToCreateIfNot()
        {
            if (!_fileHandler.IsFileExisting(Path))
            {
                if (!_fileHandler.TryCreateSimpleEmptyFile(Path))
                {
                    throw new InvalidOperationException(nameof(Path) + " Error creating file");
                }
            }
        }
    }
}