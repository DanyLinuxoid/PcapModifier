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
            Container container = new Startup().ConfigureStartupAndReturndReadyContainer();

            // If program is failing on this step with BadImageException
            // Then set x86 architecture to be used in project settings (for all projects)
            container.Verify();
            try
            {
                container.GetInstance<IProgramManager>().StartProgram(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured during program runtime");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
