using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapPacketModifier.Logic.Tools.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace PcapPacketModifier.Logic.Tools
{
    /// <summary>
    /// Responsible for file handling
    /// </summary>
    public class FileHandler : IFileHandler
    {
        public IPathProvider PathProvider { get; }

        public FileHandler(IPathProvider pathProvider)
        {
            PathProvider = pathProvider;
        }

        /// <summary>
        /// Tries to open packet on provided path
        /// </summary>
        /// <param name="path">Path to packet</param>
        /// <returns>Packet object with data</returns>
        public Packet TryOpenUserPacketFromFile(string path)
        {
            try
            {
                var providedPacket = new OfflinePacketDevice(path);
                using (PacketCommunicator communicator =
                         providedPacket.Open(65536,
                         PacketDeviceOpenAttributes.Promiscuous,
                         1000))
                {
                    Packet savedPacket;
                    communicator.ReceivePacket(out savedPacket);
                    return savedPacket;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Checks if specific file exists
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>True if file exists</returns>
        public bool IsFileExisting(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Creates file on provided path
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>True if file was created successfully, false if error occured</returns>
        public bool TryCreateSimpleEmptyFile(string path)
        {
            try
            {
                File.Create(path);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case UnauthorizedAccessException uax:
                    case ArgumentException ae:
                    case PathTooLongException ptle:
                    case DirectoryNotFoundException dnfe:
                    case IOException ioe:
                    case NotSupportedException nse:
                        Console.WriteLine(ex.Message);
                        return false;
                }

                throw;
            }

            return true;
        }

        /// <summary>
        /// Tries to open file on provided path and write any string
        /// </summary>
        /// <param name="message">Message to write in file</param>
        /// <param name="path">Path to open file</param>
        public void TryWriteMessageToFile(string message, string path)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(path, append: true))
                {
                    streamWriter.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case ObjectDisposedException ode:
                    case NotSupportedException nse:
                    case IOException ioe:
                        Console.WriteLine(ex.Message);
                        return;
                }

                throw;
            }
        }

        /// <summary>
        /// Saves one packet to disk on default path 
        /// </summary>
        /// <param name="packet"></param>
        public void TrySaveOnePacketToDisk(Packet packet, string fileName = "savedpacket")
        {
            if (packet is null)
            {
                throw new ArgumentNullException(nameof(packet));
            }

            string path = PathProvider.GetDefaultPathToSolution();
            fileName = ConstructFileNameUntilThereIsNoFileWithSameName(fileName, path, ".pcap");

            PacketDumpFile.Dump(path + fileName,
                                              packet.DataLink.Kind,
                                              packet.Length,
                                              new List<Packet> { packet });
        }

        /// <summary>
        /// Constructs new filename so there would be no copies and no rewriting of old files.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ConstructFileNameUntilThereIsNoFileWithSameName(string fileName, string path, string extension)
        {
            int? fileEndNumber = 1;
            var oldName = fileName;
            while (IsFileExisting(path + fileName + extension))
            {
                fileName = oldName + fileEndNumber;
                fileEndNumber++;
            }

            return fileName + extension;
        }
    }
}