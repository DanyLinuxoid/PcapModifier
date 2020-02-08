using System;

namespace PcapPacketModifier.Logic.UserExperience.Interfaces
{
    /// <summary>
    /// Provides function which are calling untestable code (calls directly console)
    /// </summary>
    public interface IConsoleWrapper
    {
        /// <summary>
        /// Asks for user input 
        /// </summary>
        /// <returns>String of user input</returns>
        string GetUserInputFromConsole();

        /// <summary>
        /// Asks for user key, waits until user will enters any
        /// </summary>
        /// <returns>User pressed key</returns>
        ConsoleKey GetConsolePressedKey();

        /// <summary>
        /// Exits application/console
        /// </summary>
        void ExitConsole();

        /// <summary>
        /// Writes text to console
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <param name="newLine">If new line is needed, or not, true by default</param>
        void WriteToConsole(string text, bool newLine = true);

        /// <summary>
        /// Changes console text color
        /// </summary>
        /// <param name="color">Color to change on</param>
        void ChangeConsoleTextColor(ConsoleColor color = ConsoleColor.White);

        /// <summary>
        /// Clears console
        /// </summary>
        void ClearConsole();
    }
}
