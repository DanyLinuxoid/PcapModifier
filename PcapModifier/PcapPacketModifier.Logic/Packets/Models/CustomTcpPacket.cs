using System;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Userdata.Packets;

namespace PcapPacketModifier.Logic.Packets.Models
{
    /// <summary>
    /// Custom Tcp packet model 
    /// </summary>
    public class CustomTcpPacket : CustomBasePacket
    {
        private readonly ILayerModifier _layerModifier;
        private readonly ILayerExtractor _layerExtractor;

        /// <summary>
        /// Tcp packet contains IpV4Layer
        /// </summary>
        public IpV4Layer IpV4Layer { get; set; }

        /// <summary>
        /// Tcp packet contains Payload layer with data
        /// </summary>
        public PayloadLayer PayloadLayer { get; set; }

        /// <summary>
        /// Tcp packet contains ethernet layer 
        /// </summary>
        public EthernetLayer EthernetLayer { get; set; }

        /// <summary>
        /// Tcp packet contains transport layer
        /// </summary>
        public TcpLayer TcpLayer { get; set; }

        public CustomTcpPacket(ILayerModifier layerModifier, ILayerExtractor layerExtractor)
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
            return new PacketBuilder(this.EthernetLayer, this.IpV4Layer, this.TcpLayer, this.PayloadLayer).Build(DateTime.Now);
        }

        /// <summary>
        /// Sets some fields in packet to default values, because in building process
        /// they will be filled automatically, ignores user values
        /// </summary>
        protected override void PreProcessLayersBeforeBuildingPacket()
        {
            EthernetLayer.EtherType = EthernetType.None;
            TcpLayer.Checksum = null;
        }

        /// <summary>
        /// Extracts layers from provided packet and swaps current layers with extracted 
        /// </summary>
        /// <param name="packet">Packet to extract layers from</param>
        /// <returns>Cusom packet with freshly extracted layers</returns>
        public override CustomBasePacket ExtractLayers(Packet packet)
        {
            this.EthernetLayer = _layerExtractor.ExtractEthernetLayerFromPacket(packet);
            this.TcpLayer = _layerExtractor.ExtractTcpLayerFromPacket(packet);
            this.IpV4Layer = _layerExtractor.ExtractIpv4LayerFromPacket(packet);
            this.PayloadLayer = _layerExtractor.ExtractPayloadLayerFromPacket(packet);

            return this;
        }

        /// <summary>
        /// Modifies every layer, one by one
        /// </summary>
        /// <returns>Same object with modified values</returns>
        public override CustomBasePacket ModifyLayers()
        {
            this.TcpLayer = _layerModifier.ModifyLayer(this.TcpLayer);
            this.IpV4Layer = _layerModifier.ModifyLayer(this.IpV4Layer);
            this.PayloadLayer = _layerModifier.ModifyLayer(this.PayloadLayer);
            this.EthernetLayer = _layerModifier.ModifyLayer(this.EthernetLayer);

            return this;
        }
    }
}