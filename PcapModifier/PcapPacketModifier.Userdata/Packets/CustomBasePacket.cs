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
        public abstract Packet BuildPacket();

        /// <summary>
        /// Extract layers from packet and place them in custom packet
        /// </summary>
        /// <param name="packet"></param>
        /// <returns>Returns packet with extracted layers</returns>
        public abstract CustomBasePacket ExtractLayers(Packet packet);

        /// <summary>
        /// Custom packet layers can be modified
        /// </summary>
        /// <returns>Packet with modified layers</returns>
        public abstract CustomBasePacket ModifyLayers();

        /// <summary>
        /// Sets some fields in packet to default values, because in building process
        /// they will be filled automatically, ignores user values
        /// </summary>
        protected abstract void PreProcessLayersBeforeBuildingPacket();
    }
}
