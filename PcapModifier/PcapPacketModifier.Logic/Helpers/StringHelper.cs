using PcapPacketModifier.Logic.Helpers.Interfaces;
using System;
using System.Linq;

namespace PcapPacketModifier.Logic.Helpers
{
    /// <summary>
    /// Responsible for manipulations with strings
    /// </summary>
    public class StringHelper : IStringHelper
    {
        private static readonly char[] _separators = { ',', '.', ' ' };

        /// <summary>
        /// Trims text values with commas to array of values, so later each array value can be validated, checked etc
        /// </summary>
        /// <param name="text">Text with values</param>
        /// <returns>Array of text separated values</returns>
        public string[] StringWithSignSeparatorsToArrayOfValues(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            text = TrimSeparatorsInBeginningAndEndOfText(text);
            char sign = GetSeparatorFromText(text);

            return TrimTextToArray(text, sign);
        }

        /// <summary>
        /// Trims text based on separator, and makes array of values
        /// </summary>
        /// <param name="text">Text to trim</param>
        /// <param name="sign">Separator to trim on</param>
        /// <returns>Array of separated values</returns>
        private string[] TrimTextToArray(string text, char sign)
        {
            return text.Split(sign)
                            .Select(x => x.Trim())
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .ToArray();
        }

        /// <summary>
        /// Checks separator in text
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>Separator</returns>
        private char GetSeparatorFromText(string text)
        {
            return text.Where(x => _separators.Contains(x)).FirstOrDefault();
        }

        /// <summary>
        /// Trims text from beginning and end, so there would be no garbage
        /// </summary>
        /// <param name="text">Text to trim</param>
        /// <returns>Text without separators in beginning and end</returns>
        private string TrimSeparatorsInBeginningAndEndOfText(string text)
        {
            text = text.TrimStart(_separators);
            text = text.TrimEnd(_separators);

            return text;
        }

        /// <summary>
        /// Checks if input is valid ip address by doing comparison with regex
        /// </summary>
        /// <param name="ip">Provided ip in string format</param>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValidIpAddress(string ip)
        {
            return ip == @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
        }

        /// <summary>
        /// Checks if input is valid mac address by doing comparison with regex
        /// </summary>
        /// <param name="mac">Provided mac address in string format</param>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValidMacAddress(string mac)
        {
            return mac == @"^(?:[0 - 9a - fA - F]{ 2}:){ 5}[0-9a-fA-F]{2}|(?:[0-9a-fA-F]{2}-){5}[0-9a-fA-F]{2}|(?:[0-9a-fA-F]{2}){5}[0-9a-fA-F]{2}$";
        }
    }
}