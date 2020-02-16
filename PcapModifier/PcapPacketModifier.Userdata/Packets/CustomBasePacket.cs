using PcapDotNet.Packets;
using PcapPacketModifier.Userdata.Packets.Interfaces;

namespace PcapPacketModifier.Userdata.Packets
{
    /// <summary>
    /// Base class for all custom packets, all new custom packet must inherit from this super class
    /// </summary>
    public abstract class CustomBasePacket : INewPacket
    {
        /// <summary>
        /// Custom packet can be builded
        /// </summary>
        /// <returns>Builded packet, that can be processed</returns>
        public abstract Packet BuildPacket(bool isIsncrementSeqNumber, uint sequenceNumber = 1);

        /// <summary>
        /// Extract layers from packet and place them in custom packet
        /// </summary>
        /// <param name="packet"></param>
        /// <returns>Returns packet with extracted layers</returns>
        public abstract INewPacket ExtractLayers(Packet packet);

        /// <summary>
        /// Custom packet layers can be modified
        /// </summary>
        /// <returns>Packet with modified layers</returns>
        public abstract INewPacket ModifyLayers();

        /// <summary>
        /// Copies Modules from specified packet to current packet (if values are not default)
        /// </summary>
        /// <param name="toCopyFrom"></param>
        /// <returns>New packet with copied values</returns>
        public abstract INewPacket CopyModulesFrom(INewPacket toCopyFrom);
    }
}
