using System;

namespace PcapPacketModifier.Logic.Helpers
{
    /// <summary>
    /// Responsible for error handling and building error messages
    /// </summary>
    public static class ErrorConstructor
    {
        /// <summary>
        /// Constructs error message exception, primarly for log
        /// </summary>
        /// <param name="exception">Exception to work with</param>
        /// <returns>String error with all data neded</returns>
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
