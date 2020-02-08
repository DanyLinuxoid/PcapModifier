namespace PcapPacketModifier.Logic.Modules.ClassHandlers.Interfaces
{
    public interface IModuleClassTypeArgumentsHandler
    {
        /// <summary>
        /// Gets arguments from input for Datagram class creation 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Parameter for class constructor</returns>
        byte[] GetArgumentsForDatagramFromUserInput(string input);
    }
}
