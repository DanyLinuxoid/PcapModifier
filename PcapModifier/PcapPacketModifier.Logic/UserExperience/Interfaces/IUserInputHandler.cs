using PcapDotNet.Core;
using PcapPacketModifier.Userdata.User;
using System.Collections.Generic;
using System.Reflection;

namespace PcapPacketModifier.Logic.UserExperience.Interfaces
{
    /// <summary>
    /// Handles user input
    /// </summary>
    public interface IUserInputHandler
    {
        /// <summary>
        /// Checks user input and tries to parse it 
        /// </summary>
        /// <param name="args">User input args</param>
        /// <returns>Class with input data, that stores user input information</returns>
        UserInputData ParseUserConsoleArguments(string[] args);

        /// <summary>
        /// Gets user chosen internet device
        /// </summary>
        /// <param allDevices="Local machine devices"></param>
        /// <returns>User chosen device</returns>
        int GetUserChoosenLocalMachineInternetDevice(IList<LivePacketDevice> allDevices);

        /// <summary>
        /// Asks user whether not he wants to proceed
        /// </summary>
        /// <returns></returns>
        bool IsUserWantsToContinue();

        /// <summary>
        /// Asks for user input
        /// </summary>
        /// <returns>String, that contains user input data</returns>
        string GetUserInput();

        /// <summary>
        /// Asks ser input while it contains patterns (-h gor help, -p for print, etc)
        /// </summary>
        /// <param name="property">Property/Module to print help for</param>
        /// <returns>String if user input contains no patterns</returns>
        string AskUserInputWhileInputContainsPatterns(PropertyInfo property);

        /// <summary>
        /// Checks if user input contains some patterns (-h gor help, -p for print, etc)
        /// </summary>
        /// <param name="userInput">User input data</param>
        /// <param name="property">Property information</param>
        /// <returns>True if input contains pattern, false otherwise</returns>
        bool IsUserInputContainingSpecificPattern(string userInput, PropertyInfo property);
    }
}