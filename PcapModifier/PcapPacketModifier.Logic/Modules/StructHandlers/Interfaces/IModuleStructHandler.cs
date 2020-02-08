using System;

namespace PcapPacketModifier.Logic.Modules.StructHandlers.Interfaces
{
    /// <summary>
    /// Provides functions to modify struct modules
    /// </summary>
    public interface IModuleStructHandler
    {
        /// <summary>
        /// Checks if type is comon struct, that does not requires any parameters, returns
        /// parsed value if type is common
        /// </summary>
        /// <param name="type">Type to parse</param>
        /// <param name="input">String to parse</param>
        /// <returns>Parsed value</returns>
        object CheckIfTypeIsCommonStructAndReturnObject(Type type, string input);

        /// <summary>
        /// Checks if struct must be with arguments, returns parsed object wwith arguments
        /// if type is foundand if conversion was successfull
        /// </summary>
        /// <param name="type">Type to check and parse</param>
        /// <param name="input">String to parse</param>
        /// <returns></returns>
        object CheckIfStructMustHaveArgumentsAndReturnObject(Type type, string input);
    }
}