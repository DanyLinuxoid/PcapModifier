namespace PcapPacketModifier.Logic.Helpers.Interfaces
{
    /// <summary>
    /// Provides some function to work with strings, mostly user input
    /// </summary>
    public interface IStringHelper
    {
        /// <summary>
        /// Trims user input values with commas to array of values, so later each array value can be 
        /// validated or parsed later
        /// </summary>
        /// <param name="userInput">User input with values</param>
        /// <returns>Array of input values</returns>
        string[] StringWithSignSeparatorsToArrayOfValues(string userInput);

        /// <summary>
        /// Checks if input is valid ip address by doing comparison with regex
        /// </summary>
        /// <param name="ip">Provided ip in string format</param>
        /// <returns>True if valid, false otherwise</returns>
        bool IsValidIpAddress(string ip);

        /// <summary>
        /// Checks if input is valid mac address by doing comparison with regex
        /// </summary>
        /// <param name="mac">Provided mac address in string format</param>
        /// <returns>True if valid, false otherwise</returns>
        bool IsValidMacAddress(string mac);
    }
}
