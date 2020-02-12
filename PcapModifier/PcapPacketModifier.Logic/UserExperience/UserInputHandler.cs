using System;
using System.Collections.Generic;
using System.Reflection;
using NDesk.Options;
using PcapDotNet.Core;
using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Logic.Packets;
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
            PreprocessUserArguments(args);

            string pathToFile = null;
            int packetCountToSend = 1;
            int timeToWaitUntilNextPacketToSend = 0;
            bool isModifyPacket = false;
            bool isUserWantsToSavePacketAfterModifying = false;
            bool isSendPacket = false;
            bool isHelpRequired = false;
            bool isInterceptAndForward = false;
            bool isVerbosity = false; // NOT IMPLEMENTED
            IpV4Protocol protocolToFilterBy = default;
            OptionSet optionsSet = new OptionSet()
            {
                { "p|path=", "--{PATH} to packet", p => pathToFile = p },
                { "c|count=", "--Packet {COUNT} to send", c => int.TryParse(c, out packetCountToSend) },
                { "m|modify", "--Modify packet", m => { isModifyPacket = (m != null); } },
                { "s|save", "--Save packet to .pcap file", s=> { isUserWantsToSavePacketAfterModifying = (s != null); } },
                { "S|send", "--Send packet to web", S=> { isSendPacket = (S != null); } },
                { "h|help", "--Show help", h => { isHelpRequired = (h != null); } },
                { "I|interforward", "--Intercept and forward packets through local machines internet interface", I => { isInterceptAndForward = (I != null); } },
                { "v|verbose", "--Display additional information", v => { isVerbosity = (v != null); } }, // NOT IMPLEMENTED
                { "t|time=", "--Time to wait until next packet will be sended (in milliseconds)", t => int.TryParse(t, out timeToWaitUntilNextPacketToSend) },
                { "f|filter=", "--Is used with -I options, to filter packets by protocol", f => Enum.TryParse(f, out protocolToFilterBy) },
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
                IsSendOnePacket = isSendPacket,
                IsHelpRequired = isHelpRequired,
                IsVerbose = isVerbosity,
                TimeToWaitUntilNextPacketWillBeSended = timeToWaitUntilNextPacketToSend,
                IsInterceptAndForward = isInterceptAndForward,
                PacketFilterProtocol = protocolToFilterBy,
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
        /// Gets user chosen internet device
        /// </summary>
        /// <param allDevices="Local machine devices"></param>
        /// <returns>User chosen device</returns>
        public int GetUserChoosenLocalMachineInternetDevice(IList<LivePacketDevice> allDevices)
        {
            int userChosenDevice = 0;
            int.TryParse(GetUserInput(), out userChosenDevice);
            int correctedDeviceNumber = userChosenDevice - 1; // -1 because of array
            if (!(correctedDeviceNumber <= 0) && !(correctedDeviceNumber > allDevices.Count))
            {
                return userChosenDevice;
            }

            return 0;
        }

        /// <summary>
        /// Verify that user arguments are ok
        /// </summary>
        /// <param name="userInputData">User arguments to check</param>
        /// <param name="options">User options with descriptions</param>
        private void CheckUserParsedArguments(UserInputData userInputData, OptionSet options)
        {
            if (string.IsNullOrEmpty(userInputData.PathToFile) &&
                userInputData.IsSendOnePacket)
            {
                _textDisplayer.PrintTextAndExit("Path must be provided");
            }

            if (userInputData.IsHelpRequired)
            {
                _textDisplayer.ShowHelp(options);
                _consoleWrapper.ExitConsole();
            }

            if (userInputData.PacketCountToSend == 0)
            {
                _textDisplayer.PrintTextAndExit("Wrong count set for packet to be sent");
            }

            if (userInputData.IsSendOnePacket &&
                userInputData.IsInterceptAndForward)
            {
                _textDisplayer.PrintTextAndExit("Can't send one packet and intercept multiple, choose one option");
            }

            if (userInputData.PacketFilterProtocol != default &&
                !userInputData.IsInterceptAndForward)
            {
                _textDisplayer.PrintText("Packet filtering can be used only with interception mode");
            }

            if (!Enum.IsDefined(typeof(SupportedProtocols), userInputData.PacketFilterProtocol.ToString()))
            {
                _textDisplayer.PrintText("Currently application can work with these protocols:");
                _textDisplayer.PrintEnumValues(typeof(SupportedProtocols));
                _consoleWrapper.ExitConsole();
            }
        }

        /// <summary>
        /// Does small preprocessing, in order to know, if user using arguments in wrong order, in incorrect positions, etc
        /// </summary>
        /// <param name="args">User arguments</param>
        private void PreprocessUserArguments(string[] args)
        {
            if (args is null ||
                args.Length < 2)
            {
                _textDisplayer.ShowUsage();
                _consoleWrapper.ExitConsole();
            }

            if (args[1] != "-S" &&
                args[1] != "-I")
            {
                _textDisplayer.PrintText("Second option must be mode to send packets");
            }
        }
    }
}