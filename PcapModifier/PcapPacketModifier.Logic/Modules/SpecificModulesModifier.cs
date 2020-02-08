using PcapPacketModifier.Logic.Extensions;
using PcapPacketModifier.Logic.Modules.ClassHandlers.Interfaces;
using PcapPacketModifier.Logic.Modules.Interfaces;
using PcapPacketModifier.Logic.Modules.StructHandlers.Interfaces;
using System;

namespace PcapPacketModifier.Logic.Modules
{
    /// <summary>
    /// Responsible for modifying specific modules
    /// </summary>
    public class SpecificModulesModifier : ISpecificModulesModifier
    {
        private readonly IModuleStructHandler _moduleStructHandler;
        private readonly IModuleClassTypeHandler _moduleClassHandler;

        public SpecificModulesModifier(IModuleStructHandler moduleStructHandler,
                                                    IModuleClassTypeHandler moduleClassHandler)
        {
            _moduleStructHandler = moduleStructHandler;
            _moduleClassHandler = moduleClassHandler;
        }

        /// <summary>
        /// Determines type of modulle and redirects to method to handle that module
        /// </summary>
        /// <param name="type">Type of module</param>
        /// <param name="userInput">User input</param>
        /// <returns>New object with user values</returns>
        public object HandleSpecificModule(Type type, string userInput)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (userInput == null)
            {
                throw new ArgumentNullException(nameof(userInput));
            }

            if (type.IsEnum)
            {
                return userInput.ToEnum(type);
            }

            if (type.IsValueType)
            {
                return HandleStructs(type, userInput);
            }

            if (type.IsClass)
            {
                return _moduleClassHandler.HandleModuleClases(type, userInput);
            }

            return null;
        }

        /// <summary>
        /// Handles structs, creates instance of struct or just parses to type, depends on struct type
        /// </summary>
        /// <param name="type">Type of struct</param>
        /// <param name="userInput">User input</param>
        /// <returns>Returns new struct of same type with user input in it, or parsed user input to provided type</returns>
        private object HandleStructs(Type type, string userInput)
        {
            var structWithArguments = _moduleStructHandler.CheckIfStructMustHaveArgumentsAndReturnObject(type, userInput);
            if (structWithArguments == null)
            {
                return _moduleStructHandler.CheckIfTypeIsCommonStructAndReturnObject(type, userInput);
            }

            return structWithArguments;
        }
    }
}