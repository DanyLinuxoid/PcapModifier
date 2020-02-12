using PcapDotNet.Packets;

namespace PcapPacketModifier.Userdata.Packets.Interfaces
{
    public interface INewPacket
    {
        /// <summary>
        /// New packet layers can be modified 
        /// </summary>
        /// <returns></returns>
        INewPacket ModifyLayers();

        /// <summary>
        /// Extracts layers to new packet from Packet
        /// </summary>
        /// <param name="packet">Packet to extract layers from</param>
        /// <returns>New cusom packet with extracted layers</returns>
        INewPacket ExtractLayers(Packet packet);

        /// <summary>
        /// Builds new packet
        /// </summary>
        /// <returns>New builded packet</returns>
        Packet BuildPacket(uint sequenceNumber = 1);
    }
}
