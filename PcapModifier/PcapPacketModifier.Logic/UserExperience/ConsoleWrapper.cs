using PcapPacketModifier.Logic.UserExperience.Interfaces;
using System;

namespace PcapPacketModifier.Logic.UserExperience
{
    /// <summary>
    /// Provides functions to standart system console
    /// </summary>
    public class ConsoleWrapper : IConsoleWrapper
    {
        /// <summary>
        /// Waits for user input
        /// </summary>
        /// <returns>User input as string</returns>
        public string GetUserInputFromConsole()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Asks for user key
        /// </summary>
        /// <returns>Key that user pressed</returns>
        public ConsoleKey GetConsolePressedKey()
        {
            return Console.ReadKey().Key;
        }

        /// <summary>
        /// Exits application/console
        /// </summary>
        public void ExitConsole()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Writes text to console
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <param name="newLine">If new line is needed, or not, true by default</param>
        public void WriteToConsole(string text, bool newLine = true)
        {
            if (newLine)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }
        }

        /// <summary>
        /// Changes color of console text
        /// </summary>
        /// <param name="color">Color to change on</param>
        public void ChangeConsoleTextColor(ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Clears console
        /// </summary>
        public void ClearConsole()
        {
            Console.Clear();
        }
    }
}
