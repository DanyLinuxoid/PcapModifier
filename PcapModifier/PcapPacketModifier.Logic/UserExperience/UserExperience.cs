using PcapPacketModifier.Logic.UserExperience.Interfaces;

namespace PcapPacketModifier.Logic.UserExperience
{
    /// <summary>
    /// Responsible for providing options for user related stuff, text displaying, input handling, etc
    /// </summary>
    public class UserExperience : IUserExperience
    {
        /// <summary>
        /// Provides functions for text displaying
        /// </summary>
        public ITextDisplayer UserTextDisplayer { get; }

        /// <summary>
        /// Provides functions for input handling
        /// </summary>
        public IUserInputHandler UserInputHandler { get; }

        public UserExperience(ITextDisplayer textDisplayer,
                                        IUserInputHandler userInputHandler)
        {
            UserTextDisplayer = textDisplayer;
            UserInputHandler = userInputHandler;
        }
    }
}
