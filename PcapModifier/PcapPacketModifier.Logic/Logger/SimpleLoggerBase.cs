using PcapPacketModifier.Logic.Logger.Interfaces;

namespace PcapPacketModifier.Logic.Logger
{
    /// <summary>
    /// Base class for all logger clases
    /// </summary>
    public abstract class SimpleLoggerBase : ISimpleLogger
    {
        /// <summary>
        /// Writes message to log in any place
        /// </summary>
        /// <param name="message">Message to write</param>
        public abstract void WriteLog(string message, string path = null);
    }
}
