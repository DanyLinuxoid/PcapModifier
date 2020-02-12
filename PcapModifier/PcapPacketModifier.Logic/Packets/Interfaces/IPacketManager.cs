using PcapDotNet.Packets;
using PcapPacketModifier.Userdata.Packets;
using PcapPacketModifier.Userdata.Packets.Interfaces;

namespace PcapPacketModifier.Logic.Packets.Interfaces
{
    /// <summary>
    ///  Provides function for top level application
    /// </summary>
    public interface IPacketManager
    {
        /// <summary>
        /// Extracts layers from provided packet
        /// </summary>
        /// <param name="packet">Packet to extract layers from</param>
        /// <returns>Retusn packet with new layers</returns>
        INewPacket ExtractLayersFromPacket(Packet packet);

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
    }
}
