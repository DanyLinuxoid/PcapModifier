using PcapDotNet.Packets.Ethernet;
using SimpleInjector;
using System;

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
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            args = new string[] { "-p=C:\\Users\\MSi\\Desktop\\syn.pcap", "-I", "-f=tcp", "-S"}; // testing purposes
            Container container = new Startup().ConfigureStartupAndReturndReadyContainer();
            container.Register<IProgramManager, ProgramManager>();
            container.Verify();
            container.GetInstance<IProgramManager>().StartProgram(args);
        }

        /// <summary>
        /// Catches exceptions globally if they were throwed, prints error message by default and exits application
        /// </summary>
        /// <param name="s">Sender</param>
        /// <param name="e">Arguments</param>
        static void UnhandledExceptionTrapper(object s, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("Error occured during program runtime");
            Environment.Exit(0);
        }
    }
}
