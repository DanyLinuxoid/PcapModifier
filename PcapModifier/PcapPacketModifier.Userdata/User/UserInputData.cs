using NDesk.Options;

namespace PcapPacketModifier.Userdata.User
{
    /// <summary>
    /// Input data to work with during program process
    /// </summary>
    public class UserInputData
    {
        /// <summary>
        /// Path to file
        /// </summary>
        public string PathToFile { get; set; }

        /// <summary>
        /// How many packets to send
        /// </summary>
        public int PacketCountToSend { get; set; }

        /// <summary>
        /// If user wants to modify packet
        /// </summary>
        public bool IsModifyPacket { get; set; }

        /// <summary>
        /// Field holds result of user input regarding file saving option
        /// </summary>
        public bool IsUserWantsToSavePacketAfterModifying { get; set; }

        /// <summary>
        /// If user wants to send one packet after building
        /// </summary>
        public bool IsSendPacket { get; set; }

        /// <summary>
        /// User entered pattern for helping message
        /// </summary>
        public bool IsHelpRequired { get; set; }
    }
}