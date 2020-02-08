using PcapPacketModifier.Logic.Modules.ClassHandlers.Interfaces;
using System.Text;

namespace PcapPacketModifier.Logic.Modules.ClassHandlers
{
    /// <summary>
    /// Responsible for arguments handling in instance creation process
    /// </summary>
    public class ModuleClassTypeArgumentsHandler : IModuleClassTypeArgumentsHandler
    {
        /// <summary>
        /// Gets arguments from input for Datagram class creation 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Parameter for class constructor</returns>
        public byte[] GetArgumentsForDatagramFromUserInput(string input)
        {
            return Encoding.ASCII.GetBytes(input);
        }
    }
}
