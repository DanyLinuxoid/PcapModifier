
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Logic.Factories;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Userdata.Packets;
using System;

namespace PcapPacketModifier.Logic.Layers
{
    /// <summary>
    /// Responsible for functions related to layer level
    /// </summary>
    public class LayerManager : ILayerManager
    {
        private readonly ILayerExtractor _layerExtractor;
        private readonly ILayerModifier _layerModifier;

        public LayerManager(ILayerExtractor layerExtractor, ILayerModifier layerModifier)
        {
            _layerExtractor = layerExtractor;
            _layerModifier = layerModifier;
        }

        /// <summary>
        /// After creating custom packet, extracts layers from old layer and placess them in modifies packet
        /// </summary>
        /// <param name="packet">Packet object from which layers must be extracted</param>
        /// <param name="protocol">Protocol of packet</param>
        /// <returns>Custom packet with extracted layers</returns>
        public CustomBasePacket ExtractLayersFromPacketAndReturnNewPacket(Packet packet, IpV4Protocol protocol)
        {
            return PacketFactory.GetPacket(protocol, _layerExtractor, _layerModifier).ExtractLayers(packet);
        }
    }
}