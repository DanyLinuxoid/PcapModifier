using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Userdata.Packets;
using PcapPacketModifier.Userdata.Packets.Interfaces;
using System;

namespace PcapPacketModifier.Logic.Packets.Models
{
    public class CustomUdpPacket : CustomBasePacket
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
        /// Udp packet contains udp layer
        /// </summary>
        public UdpLayer UdpLayer { get; set; }

        public CustomUdpPacket(ILayerModifier layerModifier, ILayerExtractor layerExtractor)
        {
            _layerModifier = layerModifier;
            _layerExtractor = layerExtractor;
        }

        /// <summary>
        /// Builds packet and seals it
        /// </summary>
        /// <returns>Builded packet</returns>
        public override Packet BuildPacket(uint sequenceNumber)
        {
            EthernetLayer.EtherType = EthernetType.None;
            UdpLayer.Checksum = null;
            return new PacketBuilder(this.EthernetLayer, this.IpV4Layer, this.UdpLayer, this.PayloadLayer).Build(DateTime.Now);
        }

        /// <summary>
        /// Extracts layers from provided packet and swaps current layers with extracted 
        /// </summary>
        /// <param name="packet">Packet to extract layers from</param>
        /// <returns>Custom packet with freshly extracted layers</returns>
        public override INewPacket ExtractLayers(Packet packet)
        {
            this.EthernetLayer = _layerExtractor.ExtractEthernetLayerFromPacket(packet);
            this.UdpLayer = _layerExtractor.ExtractUdpLayerFromPacket(packet);
            this.IpV4Layer = _layerExtractor.ExtractIpv4LayerFromPacket(packet);
            this.PayloadLayer = _layerExtractor.ExtractPayloadLayerFromPacket(packet);

            return this;
        }

        /// <summary>
        /// Modifies every layer, one by one
        /// </summary>
        /// <returns>Same object with modified values</returns>
        public override INewPacket ModifyLayers()
        {
            this.UdpLayer= _layerModifier.ModifyLayer(this.UdpLayer);
            this.IpV4Layer = _layerModifier.ModifyLayer(this.IpV4Layer);
            this.PayloadLayer = _layerModifier.ModifyLayer(this.PayloadLayer);
            this.EthernetLayer = _layerModifier.ModifyLayer(this.EthernetLayer);

            return this;
        }
    }
}
