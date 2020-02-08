using System;

namespace PcapPacketModifier.Logic.Modules.ClassHandlers.Interfaces
{
    /// <summary>
    /// Provides function for instance creation on modules
    /// </summary>
    public interface IModuleClassTypeHandler
    {
        /// <summary>
        /// Creates class for specified module
        /// </summary>
        /// <param name="type">Type of class to create</param>
        /// <param name="userInput">Parameters for class</param>
        /// <returns></returns>
        object HandleModuleClases(Type type, string userInput);
    }
}
