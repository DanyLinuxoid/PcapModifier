using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Userdata.Packets;

namespace PcapPacketModifier.Logic.Layers.Interfaces
{
    /// <summary>
    /// Provides top level functions for layer based operations
    /// </summary>
    public interface ILayerManager
    {
        /// <summary>
        /// Extracts layers from packet and returns custom packet with these layers
        /// </summary>
        /// <param name="packet">Packet to extract layers from</param>
        /// <param name="protocol">Protocol of packet</param>
        /// <returns>Custom packet with extracted layers</returns>
        CustomBasePacket ExtractLayersFromPacketAndReturnNewPacket(Packet packet, IpV4Protocol protocol);
    }
}