using NDesk.Options;
using PcapDotNet.Packets.IpV4;

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
        public bool IsSendOnePacket { get; set; }

        /// <summary>
        /// If user wants to intercept traffic, modify it and only then forward/resend
        /// </summary>
        public bool IsInterceptAndForward { get; set; }

        /// <summary>
        /// User entered pattern for helping message
        /// </summary>
        public bool IsHelpRequired { get; set; }

        /// <summary>
        /// If user wants to see more info in output
        /// </summary>
        public bool IsVerbose { get; set; }

        /// <summary>
        /// Pause before packets in milliseconds
        /// </summary>
        public int TimeToWaitUntilNextPacketWillBeSended { get; set; }

        /// <summary>
        /// Holds value, by which protocol packets will be filtered during interception/modification
        /// </summary>
        public IpV4Protocol PacketFilterProtocol { get; set; }
    }
}