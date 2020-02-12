using PcapPacketModifier.Logic.UserExperience.Interfaces;

namespace PcapPacketModifier.Logic.UserExperience.Interfaces
{
    /// <summary>
    /// Provides combined options for user experience
    /// </summary>
    public interface IUserExperience 
    {
        /// <summary>
        /// Text displayer for user
        /// </summary>
        ITextDisplayer UserTextDisplayer { get; }

        /// <summary>
        /// User input handler
        /// </summary>
        IUserInputHandler UserInputHandler { get; }
    }
}