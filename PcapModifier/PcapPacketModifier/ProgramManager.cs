using PcapPacketModifier.Logic.Core;
using PcapPacketModifier.Logic.UserExperience.Interfaces;
using PcapPacketModifier.Userdata.User;

namespace PcapPacketModifier
{
    /// <summary>
    /// Top class of the program, provides only mechanism to start program
    /// </summary>
    public class ProgramManager : IProgramManager
    {
        private readonly IUserInputHandler _userInputHandler;
        private readonly ICoreLogic _coreLogic;

        public ProgramManager(IUserInputHandler inputHandler, 
                                           ICoreLogic coreLogic)
        {
            _userInputHandler = inputHandler;
            _coreLogic = coreLogic;
        }

        /// <summary>
        /// Starts program after processing user input
        /// </summary>
        /// <param name="args"></param>
        public void StartProgram(string[] args)
        {
            UserInputData inputData = ProcessUserInput(args); 
            _coreLogic.ProcessLogic(inputData);
        }

        /// <summary>
        /// Evaluates, that user input corresponds to requirements
        /// </summary>
        /// <param name="args">User input data</param>
        /// <returns>Class, that contains user input data with options, that user choosed</returns>
        private UserInputData ProcessUserInput(string[] args)
        {
            return _userInputHandler.ParseUserConsoleArguments(args);
        }
    }
}