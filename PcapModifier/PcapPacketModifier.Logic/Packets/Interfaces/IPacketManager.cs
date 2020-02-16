using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Userdata.Packets.Interfaces;

namespace PcapPacketModifier.Logic.Packets.Interfaces
{
    /// <summary>
    ///  Provides function for top level application
    /// </summary>
    public interface IPacketManager
    {
        /// <summary>
        /// Extracts data from provided packet
        /// </summary>
        /// <param name="protocol">Protocol to get packet by</param>
        /// <returns>Returns new Packet object with extracted layers</returns>
        INewPacket GetPacketByProtocol(IpV4Protocol protocol);
        
        /// <summary>
        /// Sends provided paket count to ip and mac address which is set in packet
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="countToSend">Count to send</param>
        void SendPacket(INewPacket packet, int countToSend, int timeToWaitUntilNextPacketSend);

        /// <summary>
        /// Is able to intercept and forward these packets, or forward after modifying
        /// </summary>
        /// <param name="packet">Packet to forward</param>
        void InterceptAndForwardPackets(Userdata.User.UserInputData userInput);

        /// <summary>
        /// Copy packet modules from one to other
        /// </summary>
        /// <param name="toCopyFrom">Source packet to copy from</param>
        /// <param name="toCopyTo">Packet to copy to</param>
        /// <returns>Packet with copied modules</returns>
        INewPacket CopyModifiedModulesFromModifiedPacketToNewPacket(INewPacket toCopyFrom, INewPacket toCopyTo);
    }
}
