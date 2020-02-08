using PcapPacketModifier.Logic.Factories.Interfaces;
using PcapPacketModifier.Logic.Helpers;
using PcapPacketModifier.Logic.Logger.Interfaces;
using System;

namespace PcapPacketModifier.Logic.Factories
{
    /// <summary>
    /// Responsible for simple instance creation of provided type and with provided values
    /// </summary>
    public class GenericInstanceCreator : IGenericInstanceCreator
    {
        private readonly ISimpleLogger _simpleLogger;

        public GenericInstanceCreator(ISimpleLogger simpleLogger)
        {
            _simpleLogger = simpleLogger;
        }

        /// <summary>
        /// Creates instance of any type with provided values
        /// </summary>
        /// <typeparam name="T">Type of instance to create</typeparam>
        /// <param name="input">Input parameters</param>
        /// <returns>New object of provided type</returns>
        public T TryCreateNewInstance<T>(params object[] input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            T instance = default(T);
            try
            {
                instance = (T)Activator.CreateInstance(typeof(T), input);
            }
            catch (Exception ex)
            {
                _simpleLogger.WriteLog(ErrorConstructor.ConstructErrorMessageFromException(ex));
                switch (ex)
                {
                    case ArgumentException ax:
                    case FormatException fx:
                    case TypeLoadException tx:
                    case NotSupportedException nx:
                    case MissingMethodException mme:
                    case MemberAccessException mae:
                    case OverflowException oe:
                    case IndexOutOfRangeException iore:
                    case System.Reflection.TargetInvocationException tie:
                        Console.WriteLine(ex.Message);
                        return default(T);

                    default:
                        throw;
                }
            }

            return instance;
        }
    }
}