namespace PcapPacketModifier.Logic.Modules.StructHandlers.Interfaces
{
    /// <summary>
    /// Provides functions for modules to handle struct arguments, in case if struct requires some
    /// </summary>
    public interface IModuleStructArgumentsHandler
    {
        /// <summary>
        /// Returns arguments for IpV4Fragmentation struct
        /// </summary>
        /// <param name="userInput">String to parse on struct parameters</param>
        /// <returns>Parameters for IpV4Fragmentation</returns>
        object[] InputToParametersForIpV4Fragmentation(string userInput);

        /// <summary>
        /// Checks if input is valid ipv4 address
        /// </summary>
        /// <param name="input">Input to check</param>
        /// <returns>Created struct with ipV4 address</returns>
        string CheckIfInputIsValidIpV4AddressAndReturnInput(string input);

        /// <summary>
        /// Checks if input is valid ipv4 address
        /// </summary>
        /// <param name="input">Input to check</param>
        /// <returns>Created struct with ipV4 address</returns>
        string CheckIfInputIsValidMacAddressAndReturnInput(string input);
    }
}
