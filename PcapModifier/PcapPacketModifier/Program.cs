using SimpleInjector;

namespace PcapPacketModifier
{
    /// <summary>
    /// Starting point of program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">User arguments</param>
        static void Main(string[] args)
        {
            args = new string[] { "-p=C:\\Users\\MSi\\Desktop\\udppacket.pcap", "-s", "-c=5", "-m", "-W", "-h" }; // testing purposes
            Container container = new Startup().ConfigureStartupAndReturndReadyContainer();
            container.Register<IProgramManager, ProgramManager>();
            container.Verify();
            container.GetInstance<IProgramManager>().StartProgram(args);
        }
    }
}
