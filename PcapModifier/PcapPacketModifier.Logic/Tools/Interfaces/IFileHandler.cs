using PcapDotNet.Packets;

namespace PcapPacketModifier.Logic.Tools.Interfaces
{
    /// <summary>
    /// Interface for file handling
    /// </summary>
    public interface IFileHandler
    {
        /// <summary>
        /// Tries to open packet with data from file
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Packet which was opened</returns>
        Packet TryOpenUserPacketFromFile(string path);

        /// <summary>
        /// Checks if file exists on provided path
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>True if file exists</returns>
        bool IsFileExisting(string path);

        /// <summary>
        /// Tries to create file on provided path
        /// </summary>
        /// <param name="path">Path to file with name</param>
        /// <returns>True if file was successfully created</returns>
        bool TryCreateSimpleEmptyFile(string path);

        /// <summary>
        /// Tries to write message to file
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="path">Path to file</param>
        /// <returns>True if write was successfull</returns>
        void TryWriteMessageToFile(string message, string path);

        /// <summary>
        /// Saves packet to disk on default path in solution
        /// </summary>
        /// <param name="packet"></param>
        void TrySaveOnePacketToDisk(Packet packet, string fileName = "savedpacket");

        /// <summary>
        /// Gets length of the file provided on path
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Size of file on path</returns>
        long GetFileLength(string path);

        /// <summary>
        /// Responsible for path providing to files
        /// </summary>
        IPathProvider PathProvider { get; }
    }
}