using PcapDotNet.Packets;
using PcapPacketModifier.Logic.Modules.ClassHandlers.Interfaces;
using System;

namespace PcapPacketModifier.Logic.Modules.ClassHandlers
{
    /// <summary>
    /// Provides functions to handle class creation on module modification
    /// </summary>
    public class ModuleClassTypeHandler : IModuleClassTypeHandler
    {
        private readonly IModuleClassTypeArgumentsHandler _moduleClassArguments;

        public ModuleClassTypeHandler(IModuleClassTypeArgumentsHandler moduleClassArguments)
        {
            _moduleClassArguments = moduleClassArguments;
        }

        /// <summary>
        /// Creates instance of specified class with arguments provided from lower level
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public object HandleModuleClases(Type type, string userInput)
        {
            if (type == typeof(Datagram))
            {
                return new Datagram(_moduleClassArguments.GetArgumentsForDatagramFromUserInput(userInput));
            }

            return null;
        }
    }
}
