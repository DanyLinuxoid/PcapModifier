namespace PcapPacketModifier.Logic.Logger.Interfaces
{
    /// <summary>
    /// Interfaces for loggers
    /// </summary>
    public interface ISimpleLogger
    {
        /// <summary>
        /// Writes log with some message
        /// </summary>
        /// <param name="message"></param>
        void WriteLog(string message, string path = null);
    }
}