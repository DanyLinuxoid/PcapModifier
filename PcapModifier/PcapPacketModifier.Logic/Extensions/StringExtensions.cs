using System;

namespace PcapPacketModifier.Logic.Extensions
{
    /// <summary>
    /// Extension methods for string
    /// </summary>
    public static class StringExtenstions
    {
        /// <summary>
        /// Parses string to specified type (struct)
        /// </summary>
        /// <typeparam name="T">Type to parse to</typeparam>
        /// <param name="input">String to parse</param>
        /// <returns>Parsed value of specified type</returns>
        public static T? ToType<T>(this string input) where T : struct
        {
            T data = default(T);
            try
            {
                data = (T)Convert.ChangeType(input, typeof(T));
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case ArgumentException ae:
                    case InvalidCastException ice:
                    case FormatException fe:
                    case OverflowException oe:
                        Console.WriteLine(ex.Message);
                        return default(T);

                    default:
                        throw;
                }
            }

            return data;
        }

        /// <summary>
        /// Parses string to enumof specified type
        /// </summary>
        /// <param name="someEnum">Enum to parse to</param>
        /// <param name="input">String to parse</param>
        /// <returns>Parsed object</returns>
        public static Enum ToEnum(this string input, Type someEnum)
        {
            if (someEnum == null)
            {
                throw new ArgumentNullException(nameof(someEnum));
            }

            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            Enum newValue = null;
            try
            {
                newValue = (Enum)Enum.Parse(someEnum, input);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case ArgumentException ae:
                    case OverflowException oe:
                        Console.WriteLine(ex.Message);
                        return null;

                    default:
                        throw;
                }
            }

            return newValue;
        }
    }
}
