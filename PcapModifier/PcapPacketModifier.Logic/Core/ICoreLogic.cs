
using PcapPacketModifier.Userdata.User;

namespace PcapPacketModifier.Logic.Core
{ 
    public interface ICoreLogic
    {
        /// <summary>
        /// Starts core logic of program
        /// </summary>
        /// <param name="inputData">User input data to work with during process</param>
        void ProcessLogic(UserInputData inputData);
    }
}