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
        Packet BuildPacket(bool isIncrementSeqNumber, uint sequenceNumber = 1);

        /// <summary>
        /// Copies Modules from specified packet to current packet (if values are not default)
        /// </summary>
        /// <param name="toCopyFrom"></param>
        /// <returns>New packet with copied values</returns>
        INewPacket CopyModulesFrom(INewPacket toCopyFrom);
    }
}
