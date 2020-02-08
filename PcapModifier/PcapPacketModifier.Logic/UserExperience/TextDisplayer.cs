using NDesk.Options;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapPacketModifier.Logic.Helpers;
using PcapPacketModifier.Logic.Logger.Interfaces;
using PcapPacketModifier.Logic.UserExperience.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PcapPacketModifier.Logic.UserExperience
{
    /// <summary>
    /// Responsible for text printing to user
    /// </summary>
    public class TextDisplayer : ITextDisplayer
    {
        private readonly ISimpleLogger _simpleLogger;
        private readonly IConsoleWrapper _consoleWrapper;

        public TextDisplayer(ISimpleLogger simpleLogger, IConsoleWrapper consoleWrapper)
        {
            _simpleLogger = simpleLogger;
            _consoleWrapper = consoleWrapper;
        }

        /// <summary>
        /// Shows help message to user with possible options
        /// </summary>
        public void ShowHelp(OptionSet options)
        {
            ShowUsage();
            ShowDescriptionForProgram();
            PrintText("Options:");
            options.WriteOptionDescriptions(Console.Out);
        }

        /// <summary>
        /// Shows usage for user, such as available options 
        /// </summary>
        public void ShowUsage()
        {
            _consoleWrapper.WriteToConsole("Usage: pcapmodifier -p [path to packet] [function to do] [optional args]");
            _consoleWrapper.WriteToConsole("Example: pcapmodifier -p=C:\\packet.pcap -m -S -c=10\n");
        }

        /// <summary>
        /// Shows info about program, description
        /// </summary>
        private void ShowDescriptionForProgram()
        {
            PrintText("Program is capable of modifying '.pcap' format packets, from local OR online source (by intercepting) \nand sending these packets to Web.");
            PrintText("Can accept and modify packets of protocols: TCP, ICMP, UDP\n");
            PrintText("--- NOTE --- ");
            PrintText("This program is not working very well if you are behind Wi-Fi, in that case it can send corrupted or doubled packets, \nbest to use it with Ethernet\n");
        }

        /// <summary>
        /// Prints packet data, layer by layer
        /// </summary>
        /// <param name="packet">Packet with data to print</param> //////////////// IMPLEMENT!
        public void ShowNewPacketData(Packet packet)
        {
        }

        /// <summary>
        /// Template for text printing 
        /// </summary>
        /// <param name="text">Text to print</param>
        /// <param name="newLine">If new line is needed</param>
        public void PrintText(string text, bool newLine = true)
        {
            _consoleWrapper.WriteToConsole(text, newLine);
        }

        /// <summary>
        /// Prints message and exit from program
        /// </summary>
        /// <param name="text">Text to print</param>
        public void PrintTextAndExit(string text)
        {
            _consoleWrapper.WriteToConsole(text);
            _consoleWrapper.ExitConsole();
        }

        /// <summary>
        /// Prints all enum options from provided object
        /// </summary>
        /// <param name="enums">Object to print enums from</param>
        public void PrintEnumValues(Type enums)
        {
            Array enumValues;
            try
            {
                enumValues = Enum.GetValues(enums);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case InvalidOperationException ioe:
                    case ArgumentException ae:
                        _simpleLogger.WriteLog(ErrorConstructor.ConstructErrorMessageFromException(ex));
                        _consoleWrapper.WriteToConsole(ex.Message);
                        return;
                }

                _simpleLogger.WriteLog(ErrorConstructor.ConstructErrorMessageFromException(ex));
                throw;
            }

            _consoleWrapper.WriteToConsole("\nAvailable Options:");
            foreach (var value in enumValues)
            {
                _consoleWrapper.WriteToConsole(value.ToString());
            }
        }

        /// <summary>
        /// Prints message if module was succesfully modified
        /// </summary>
        public void SuccessfullyModifiedModule()
        {
            _consoleWrapper.ChangeConsoleTextColor(ConsoleColor.Green);
            _consoleWrapper.WriteToConsole("Successfully modified module");
            _consoleWrapper.ChangeConsoleTextColor();
        }

        /// <summary>
        /// Prints message if module modification failed
        /// </summary>
        public void FailedModifyingModule()
        {
            _consoleWrapper.WriteToConsole("Failed modifying module, default value is set");
        }

        /// <summary>
        /// Prints hint for user, is used while manually modifying modules 
        /// </summary>
        public void Hint()
        {
            _consoleWrapper.WriteToConsole("Empty field = Default");
            _consoleWrapper.WriteToConsole("type -h for help ");
        }

        /// <summary>
        /// Prints module information
        /// </summary>
        /// <param name="type">Type of module</param>
        /// <param name="fieldName">Filed name of module</param>
        /// <param name="value">Value of module</param>
        public void PrintModuleInfo(string type, string fieldName, string value)
        {
            _consoleWrapper.WriteToConsole("\nType: " + type + " | Field: " + fieldName + " | Value: " + value);
        }

        /// <summary>
        /// Clears console
        /// </summary>
        public void ClearConsole()
        {
            _consoleWrapper.ClearConsole();
        }

        /// <summary>
        /// Prints help message before each layer modifications, such as options available
        /// keys to press etc.
        /// </summary>
        public void PrintHelpingMessageBeforeModifyingLayer(Layer layer)
        {
            if (layer is null)
            {
                throw new ArgumentNullException(nameof(layer));
            }

            _consoleWrapper.ClearConsole();
            _consoleWrapper.WriteToConsole(layer.GetType().ToString().Split('.').Last() + "\n", true);
            Hint();
        }

        /// <summary>
        /// Displays parameters for module constructor
        /// </summary>
        /// <param name="parameters">Parameters to display</param>
        public void DisplayParametersForModuleConstructor(string[] parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                _consoleWrapper.WriteToConsole($"{i + 1} parameter: {parameters[i]}");
            }
        }

        /// <summary>
        /// Displays all network interfaces on local machine to user
        /// </summary>
        /// <param name="devices">Registered devices on local machine</param>
        public void DisplayAllLocalMachineNetworkInterfaces(IList<LivePacketDevice> devices)
        {
            ClearConsole();
            PrintText("Choose network device to send packet");
            for(int i = 0; i < devices.Count; i++)
            {
                PrintText($"{i + 1}." +  devices[i].Description + "\n");
            }
        }

        /// <summary>
        /// Displays payload data as normal string
        /// </summary>
        /// <param name="layer">String of decoded bytes that can be displayed</param>
        public void DisplayPayloadData(PayloadLayer layer)
        {
            PrintText("Payload: " + Encoding.ASCII.GetString(layer.Data.ToArray()));
        }
    }
}