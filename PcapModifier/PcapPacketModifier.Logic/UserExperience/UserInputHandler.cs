using System;
using System.Reflection;
using NDesk.Options;
using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Logic.UserExperience.Interfaces;
using PcapPacketModifier.Userdata.User;

namespace PcapPacketModifier.Logic.UserExperience
{
    /// <summary>
    /// Responsible for user input handling
    /// </summary>
    public class UserInputHandler : IUserInputHandler
    {
        private readonly ITextDisplayer _textDisplayer;
        private readonly IConsoleWrapper _consoleWrapper;

        public UserInputHandler(ITextDisplayer textDisplayer, IConsoleWrapper consoleWrapper)
        {
            _textDisplayer = textDisplayer;
            _consoleWrapper = consoleWrapper;
        }

        /// <summary>
        /// Checks user args
        /// </summary>
        /// <param name="args">User args</param>
        /// <returns>UserInputData class with user preferences/chosen options</returns>
        public UserInputData ParseUserConsoleArguments(string[] args)
        {
            if (args is null || 
                args.Length < 2)
            {
                _textDisplayer.ShowUsage();
                _consoleWrapper.ExitConsole();
            }

            string pathToFile = null;
            int packetCountToSend = 1;
            bool isModifyPacket = false;
            bool isUserWantsToSavePacketAfterModifying = false;
            bool isSendPacket = false;
            bool isHelpRequired = false;
            OptionSet optionsSet = new OptionSet()
            {
                { "p|path=", "--{PATH} to packet", p => pathToFile = p },
                { "c|count=", "--Packet {COUNT} to send", c => int.TryParse(c, out packetCountToSend) },
                { "m", "--Modify packet", m => { isModifyPacket = (m != null); } },
                { "s|save", "--Save packet to .pcap file", s=> { isUserWantsToSavePacketAfterModifying = (s != null); } },
                { "S|send", "--Send packet to web", W => { isSendPacket = (W != null); } },
                { "h|help", "--Show help", h => {isHelpRequired = (h != null); } },
            };

            try
            {
                optionsSet.Parse(args);
            }
            catch (OptionException ex)
            {
                _consoleWrapper.WriteToConsole(ex.Message);
                _consoleWrapper.ExitConsole();
            }

            UserInputData userInputData = new UserInputData()
            {
                PathToFile = pathToFile,
                PacketCountToSend = packetCountToSend,
                IsModifyPacket = isModifyPacket,
                IsUserWantsToSavePacketAfterModifying = isUserWantsToSavePacketAfterModifying,
                IsSendPacket = isSendPacket,
                IsHelpRequired = isHelpRequired,
            };

            CheckUserParsedArguments(userInputData, optionsSet);
            return userInputData;
        }

        /// <summary>
        /// Asks user, if he wants to proceed
        /// </summary>
        /// <returns>True if user wants to proceed, false otherwise</returns>
        public bool IsUserWantsToContinue()
        {
            var answer = _consoleWrapper.GetConsolePressedKey();
            if (answer == ConsoleKey.Y)
            {
                return true;
            }

            if (answer == ConsoleKey.A)
            {
                _consoleWrapper.ExitConsole();
            }

            return false;
        }

        /// <summary>
        /// Asks user for input, until input contains patterns (user asks for usage, or for possible options)
        /// </summary>
        /// <param name="property">Property to print info for, if user asks for help</param>
        /// <returns>String with valid data, that contains no patterns</returns>
        public string AskUserInputWhileInputContainsPatterns(PropertyInfo property)
        {
            string userInput;
            do
            {
                userInput = GetUserInput();
            }
            while (IsUserInputContainingSpecificPattern(userInput, property));

            return userInput;
        }

        /// <summary>
        /// Asks for user input one time
        /// </summary>
        /// <returns>String, that contains user input data</returns>
        public string GetUserInput()
        {
            return _consoleWrapper.GetUserInputFromConsole();
        }

        /// <summary>
        /// Checks if user typed some pattern while modifying property/module
        /// </summary>
        /// <param name="userInput">User input data</param>
        /// <param name="property">Property, that user asks to help with</param>
        /// <returns>True if input contains pattern, false otherwise</returns>
        public bool IsUserInputContainingSpecificPattern(string userInput, PropertyInfo property)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            switch (userInput)
            {
                case "-h":
                    {
                        (Type propType, string[] help) = CheckIfSpecificEnumAndReturnTypeWithParametersToPrint(property.PropertyType);
                        if (propType != null)
                        {
                            _textDisplayer.PrintEnumValues(propType);
                            _textDisplayer.DisplayParametersForModuleConstructor(help);
                        }
                        else
                        {
                            _textDisplayer.PrintEnumValues(property.PropertyType);
                            _textDisplayer.PrintText(help[0], true);
                        }

                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        /// <summary>
        /// Contains checks for specific enum, which are not normally mapping, and cannot be determined in other way.
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns>Type of enum and string with parameters</returns>
        private (Type, string[]) CheckIfSpecificEnumAndReturnTypeWithParametersToPrint(Type propertyType)
        {
            if (propertyType == typeof(IpV4Fragmentation))
            {
                return (typeof(IpV4FragmentationOptions), new string[2] { "IpV4Fragmentation options", "offset" });
            }

            return (null, new string[1] { "\nNo additional help for this module" });
        }

        /// <summary>
        /// Verify that user arguments are ok
        /// </summary>
        /// <param name="userInputData">User arguments to check</param>
        /// <param name="options">User options with descriptions</param>
        public void CheckUserParsedArguments(UserInputData userInputData, OptionSet options)
        {
            if (userInputData.IsHelpRequired)
            {
                _textDisplayer.ShowHelp(options);
                _consoleWrapper.ExitConsole();
            }

            if (string.IsNullOrEmpty(userInputData.PathToFile))
            {
                _textDisplayer.PrintTextAndExit("-p is required option");
            }

            if (userInputData.PacketCountToSend == 0)
            {
                _textDisplayer.PrintTextAndExit("Wrong count set for packet to be sent");
            }
        }
    }
}