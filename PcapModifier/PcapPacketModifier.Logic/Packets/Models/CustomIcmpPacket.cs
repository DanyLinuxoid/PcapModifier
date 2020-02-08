using System;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Userdata.Packets;

namespace PcapPacketModifier.Logic.Packets.Models
{
    /// <summary>
    /// Custom Icmp packet model 
    /// </summary>
    public class CustomIcmpPacket : CustomBasePacket
    {
        private readonly ILayerModifier _layerModifier;
        private readonly ILayerExtractor _layerExtractor;

        /// <summary>
        /// Icmp packet contains IpV4Layer
        /// </summary>
        public IpV4Layer IpV4Layer { get; set; }

        /// <summary>
        /// Icmp packet contains ethernet layer 
        /// </summary>
        public EthernetLayer EthernetLayer { get; set; }

        /// <summary>
        /// Icmp layer for icmp packet
        /// </summary>
        public IcmpLayer IcmpLayer { get; set; }

        public CustomIcmpPacket(ILayerModifier layerModifier, ILayerExtractor layerExtractor)
        {
            _layerModifier = layerModifier;
            _layerExtractor = layerExtractor;
        }

        /// <summary>
        /// Builds packet and seals it
        /// </summary>
        /// <returns>Builded packet</returns>
        public override Packet BuildPacket()
        {
            PreProcessLayersBeforeBuildingPacket();
            return new PacketBuilder(this.EthernetLayer, this.IpV4Layer, this.IcmpLayer).Build(DateTime.Now);
        }

        /// <summary>
        /// Sets some fields in packet to default values, because in building process
        /// they will be filled automatically, ignores user values
        /// </summary>
        protected override void PreProcessLayersBeforeBuildingPacket()
        {
            EthernetLayer.EtherType = EthernetType.None;
            IcmpLayer.Checksum = null;
        }

        /// <summary>
        /// Extracts layers from provided packet and swaps current layers with extracted 
        /// </summary>
        /// <param name="packet">Packet to extract layers from</param>
        /// <returns>Cusom packet with freshly extracted layers</returns>
        public override CustomBasePacket ExtractLayers(Packet packet)
        {
            this.EthernetLayer = _layerExtractor.ExtractEthernetLayerFromPacket(packet);
            this.IpV4Layer = _layerExtractor.ExtractIpv4LayerFromPacket(packet);
            this.IcmpLayer = _layerExtractor.ExtractIcmpLayerFromPacket(packet);

            return this;
        }

        /// <summary>
        /// Modifies every layer, one by one
        /// </summary>
        /// <returns>Same object with modified values</returns>
        public override CustomBasePacket ModifyLayers()
        {
            this.IpV4Layer = _layerModifier.ModifyLayer(this.IpV4Layer);
            this.EthernetLayer = _layerModifier.ModifyLayer(this.EthernetLayer);
            this.IcmpLayer = _layerModifier.ModifyLayer(this.IcmpLayer);

            return this;
        }
    }
}