using NDesk.Options;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System;
using System.Collections.Generic;

namespace PcapPacketModifier.Logic.UserExperience.Interfaces
{
    /// <summary>
    /// Frontend part for console, provides text output
    /// </summary>
    public interface ITextDisplayer
    {
        /// <summary>
        /// Prints help for user
        /// </summary>
        void ShowHelp(OptionSet options);

        /// <summary>
        /// Prints usage for user
        /// </summary>
        void ShowUsage();

        /// <summary>
        /// Prints data of new packet, layer by layer
        /// </summary>
        /// <param name="packet">Packet to print info for</param>
        void ShowPacketBaseInfo(Packet packet);

        /// <summary>
        /// Template for text printing
        /// </summary>
        /// <param name="text">Text to print</param>
        /// <param name="newLine">If new line is needed after text</param>
        void PrintText(string text, bool newLine = true);

        /// <summary>
        /// Prints message and exit from program
        /// </summary>
        /// <param name="text">Text to print</param>
        void PrintTextAndExit(string text);

        /// <summary>
        /// Prints available options for specified enum object
        /// </summary>
        /// <param name="enums">Object with options to print</param>
        void PrintEnumValues(Type enums);

        /// <summary>
        /// Prints message if module modification was successfull
        /// </summary>
        void SuccessfullyModifiedModule();

        /// <summary>
        /// Prints hint for user
        /// </summary>
        void Hint();

        /// <summary>
        /// Prints module info for user
        /// </summary>
        /// <param name="type">Type of module</param>
        /// <param name="fieldName">Name of moule/property</param>
        /// <param name="value">Value of module</param>
        void PrintModuleInfo(string type, string fieldName, string value);

        /// <summary>
        /// Prints message if module modification failed
        /// </summary>
        void FailedModifyingModule();

        /// <summary>
        /// Clears Console
        /// </summary>
        void ClearConsole();

        /// <summary>
        /// Prints helping message
        /// </summary>
        void PrintHelpingMessageBeforeModifyingLayer(Layer layer);

        /// <summary>
        /// Displays parameters for constructor, that are provided by some helper method
        /// </summary>
        /// <param name="parameters"></param>
        void DisplayParametersForModuleConstructor(string[] parameters);

        /// <summary>
        /// Displays all network interfaces on local machine to user
        /// </summary>
        /// <param name="devices"></param>
        void DisplayAllLocalMachineNetworkInterfaces(IList<LivePacketDevice> devices);

        /// <summary>
        /// Displays payload data as normal string
        /// </summary>
        /// <param name="layer">String of decoded bytes that can be displayed</param>
        void DisplayPayloadData(PayloadLayer layer);

        /// <summary>
        /// Displays small hint message while interception is in progress
        /// </summary>
        void DisplayHintWhileInterceptionIsInProgress();

        /// <summary>
        /// Prints items in list
        /// </summary>
        /// <typeparam name="T">Type of items in list</typeparam>
        /// <param name="listItems">List with items</param>
        void PrintItemsInList<T>(List<T> listItems);

        /// <summary>
        /// Write text according to count
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <param name="count">Count to write</param>
        void PrintOneLineManyTimes(string text, int count);
    }
}
