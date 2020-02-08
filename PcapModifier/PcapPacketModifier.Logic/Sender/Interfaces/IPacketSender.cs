using PcapPacketModifier.Userdata.Packets;

namespace PcapPacketModifier.Logic.Sender.Interfaces
{
    public interface IPacketSender
    {
        /// <summary>
        /// Sends packet by calling kernel core functions
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="countToSend">Count to send</param>
        /// <param name="timeToWaitBeforeNextPacketToSend ">Time to wait before sending packet again</param>
        void SendPacket(CustomBasePacket packet, int countToSend, int timeToWaitBeforeNextPacketToSend = 0);
    }
}
