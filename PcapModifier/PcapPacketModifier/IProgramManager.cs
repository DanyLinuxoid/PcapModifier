namespace PcapPacketModifier
{
    /// <summary>
    /// Top layer of program, provides single function for start
    /// </summary>
    public interface IProgramManager
    {
        /// <summary>
        /// Starts program
        /// </summary>
        /// <param name="args"></param>
        void StartProgram(string[] args);
    }
}