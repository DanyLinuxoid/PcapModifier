using PcapDotNet.Packets;
using PcapPacketModifier.Userdata.Packets.Interfaces;

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
        INewPacket ExtractLayersFromPacketAndReturnNewPacket(Packet packet);
    }
}