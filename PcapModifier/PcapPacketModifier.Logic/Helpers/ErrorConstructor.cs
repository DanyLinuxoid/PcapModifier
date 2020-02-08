using System;

namespace PcapPacketModifier.Logic.Helpers
{
    /// <summary>
    /// Responsible for error handling and building error messages
    /// </summary>
    public static class ErrorConstructor
    {
        /// <summary>
        /// Simply constructs error message 
        /// </summary>
        /// <param name="exception">Exception to work with</param>
        /// <returns>String error woth all data neded</returns>
        public static string ConstructErrorMessageFromException(Exception exception)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return $"\nDate: {DateTime.Now}\nError Message: {exception.Message}\nStack Trace: {exception.StackTrace}\nEND";
        }
    }
}
