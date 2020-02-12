using PcapPacketModifier.Logic.Tools.Interfaces;

namespace PcapPacketModifier.Logic.Tools
{
    public class PathProvider : PathProviderBase, IPathProvider
    {
        /// <summary>
        /// Gets default path for log file
        /// </summary>
        /// <returns>Path for log file</returns>
        public string GetDefaultPathForLog()
        {
            return GetPathToSolution() + "\\log.txt";
        }

        /// <summary>
        /// Gets default path to solution
        /// </summary>
        /// <returns>Path to solution as string</returns>
        public string GetDefaultPathToSolution()
        {
            return base.GetPathToSolution();
        }

        /// <summary>
        /// Gets default path for dumpfile, where to load download files    
        /// </summary>
        /// <returns>Path to dumpfile</returns>
        public string GetDefaultPathForDumpFile()
        {
            return GetPathToSolution() + "\\DumpFile.pcap";
        }
    }
}
