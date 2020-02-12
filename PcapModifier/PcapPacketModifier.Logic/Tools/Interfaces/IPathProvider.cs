namespace PcapPacketModifier.Logic.Tools.Interfaces
{
    public interface IPathProvider
    {
        /// <summary>
        /// Gets default path for log file
        /// </summary>
        /// <returns>Path for log file</returns>
        string GetDefaultPathForLog();

        /// <summary>
        /// Gets default path to solution
        /// </summary>
        /// <returns>Path to solution as string</returns>
        string GetDefaultPathToSolution();

        /// <summary>
        /// Gets default path for dumpfile, where to load download files    
        /// </summary>
        /// <returns>Path to dumpfile</returns>
        string GetDefaultPathForDumpFile();
    }
}
