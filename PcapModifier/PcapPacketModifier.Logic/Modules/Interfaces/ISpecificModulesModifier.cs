using System;

namespace PcapPacketModifier.Logic.Modules.Interfaces
{
    /// <summary>
    /// Provides functions to modify specific modules
    /// </summary>
    public interface ISpecificModulesModifier
    {
        /// <summary>
        /// Handles specific modules of any type
        /// </summary>
        /// <param name="type">Type of module</param>
        /// <param name="userInput">User input</param>
        /// <returns>Object of module type with user values</returns>
        object HandleSpecificModule(Type type, string userInput);
    }
}