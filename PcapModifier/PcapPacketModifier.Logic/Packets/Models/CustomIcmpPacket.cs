using System;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Userdata.Packets;
using PcapPacketModifier.Userdata.Packets.Interfaces;

namespace PcapPacketModifier.Logic.Packets.Models
{
    /// <summary>
    /// Custom Icmp packet model 
    /// </summary>
    public class CustomIcmpPacket : CustomBasePacket
    {
        private readonly ILayerModifier _layerModifier;
        private readonly ILayerExchanger _layerExchanger;

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
        public IcmpEchoLayer IcmpLayer{ get; set; }

        public CustomIcmpPacket(ILayerModifier layerModifier, 
                                              ILayerExchanger layerExchanger)
        {
            _layerModifier = layerModifier;
            _layerExchanger = layerExchanger;

        }

        /// <summary>
        /// Builds packet and seals it
        /// </summary>
        /// <returns>Builded packet</returns>
        public override Packet BuildPacket(bool isIncrementSeqNumber, uint sequenceNumber = 1)
        {
            EthernetLayer.EtherType = EthernetType.None;
            IcmpLayer.Checksum = null;
            IcmpLayer.SequenceNumber = (ushort)sequenceNumber;
            return new PacketBuilder(this.EthernetLayer, this.IpV4Layer, this.IcmpLayer).Build(DateTime.Now);
        }

        /// <summary>
        /// Extracts layers from provided packet and swaps current layers with extracted 
        /// </summary>
        /// <param name="packet">Packet to extract layers from</param>
        /// <returns>Cusom packet with freshly extracted layers</returns>
        public override INewPacket ExtractLayers(Packet packet)
        {
            this.EthernetLayer = packet.Ethernet.ExtractLayer() as EthernetLayer;
            this.IpV4Layer = packet.Ethernet.IpV4.ExtractLayer() as IpV4Layer;
            this.IcmpLayer = packet.Ethernet.IpV4.Icmp.ExtractLayer() as IcmpEchoLayer;
            return this;
        }

        /// <summary>
        /// Modifies every layer, one by one
        /// </summary>
        /// <returns>Same object with modified values</returns>
        public override INewPacket ModifyLayers()
        {
            this.IpV4Layer = _layerModifier.ModifyLayer(this.IpV4Layer);
            this.EthernetLayer = _layerModifier.ModifyLayer(this.EthernetLayer);
            this.IcmpLayer = _layerModifier.ModifyLayer(this.IcmpLayer);

            return this;
        }

        /// <summary>
        /// Copies Modules from specified packet to current packet (if values are not default)
        /// </summary>
        /// <param name="toCopyFrom"></param>
        /// <returns>New packet with copied values</returns>
        public override INewPacket CopyModulesFrom(INewPacket source)
        {
            CustomIcmpPacket toCopyFrom = source as CustomIcmpPacket;
            this.EthernetLayer = _layerExchanger.AssignUserValuesFromFilledLayerToOtherLayer(toCopyFrom.EthernetLayer, this.EthernetLayer);
            this.IpV4Layer = _layerExchanger.AssignUserValuesFromFilledLayerToOtherLayer(toCopyFrom.IpV4Layer, this.IpV4Layer);
            this.IcmpLayer = _layerExchanger.AssignUserValuesFromFilledLayerToOtherLayer(toCopyFrom.IcmpLayer, this.IcmpLayer);

            return this;
        }
    }
}