using System;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Userdata.Packets;
using PcapPacketModifier.Userdata.Packets.Interfaces;

namespace PcapPacketModifier.Logic.Packets.Models
{
    /// <summary>
    /// Custom Tcp packet model 
    /// </summary>
    public class CustomTcpPacket : CustomBasePacket
    {
        private readonly ILayerModifier _layerModifier;
        private readonly ILayerExchanger _layerExchanger;

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

        public CustomTcpPacket(ILayerModifier layerModifier, 
                                           ILayerExchanger layerExchanger)
        {
            _layerModifier = layerModifier;
            _layerExchanger = layerExchanger;
        }

        /// <summary>
        /// Builds packet and seals it
        /// </summary>
        /// <returns>Builded packet</returns>
        public override Packet BuildPacket(bool isIncrementSeqNumber, uint sequenceNumber = 0)
        {
            if (isIncrementSeqNumber)
            {
                TcpLayer.SequenceNumber = sequenceNumber;
                TcpLayer.AcknowledgmentNumber = sequenceNumber;
            }

            EthernetLayer.EtherType = EthernetType.None;
            TcpLayer.Checksum = null;
            return new PacketBuilder(this.EthernetLayer, this.IpV4Layer, this.TcpLayer, this.PayloadLayer).Build(DateTime.Now);
        }

        /// <summary>
        /// Extracts layers from provided packet and swaps current layers with extracted 
        /// </summary>
        /// <param name="packet">Packet to extract layers from</param>
        /// <returns>Cusom packet with freshly extracted layers</returns>
        public override INewPacket ExtractLayers(Packet packet)
        {
            this.EthernetLayer = packet.Ethernet.ExtractLayer() as EthernetLayer;
            this.TcpLayer = packet.Ethernet.IpV4.Tcp.ExtractLayer() as TcpLayer;
            this.IpV4Layer = packet.Ethernet.IpV4.ExtractLayer() as IpV4Layer;
            this.PayloadLayer = packet.Ethernet.Payload.ExtractLayer() as PayloadLayer;

            return this;
        }

        /// <summary>
        /// Modifies every layer, one by one
        /// </summary>
        /// <returns>Same object with modified values</returns>
        public override INewPacket ModifyLayers()
        {
            this.TcpLayer = _layerModifier.ModifyLayer(this.TcpLayer);
            this.IpV4Layer = _layerModifier.ModifyLayer(this.IpV4Layer);
            this.PayloadLayer = _layerModifier.ModifyLayer(this.PayloadLayer);
            this.EthernetLayer = _layerModifier.ModifyLayer(this.EthernetLayer);

            return this;
        }

        /// <summary>
        /// Copies Modules from specified packet to current packet (if values are not default)
        /// </summary>
        /// <param name="toCopyFrom"></param>
        /// <returns>New packet with copied values</returns>
        public override INewPacket CopyModulesFrom(INewPacket source)
        {
            CustomTcpPacket toCopyFrom = source as CustomTcpPacket;
            this.TcpLayer = _layerExchanger.AssignUserValuesFromFilledLayerToOtherLayer(toCopyFrom.TcpLayer, this.TcpLayer);
            this.EthernetLayer = _layerExchanger.AssignUserValuesFromFilledLayerToOtherLayer(toCopyFrom.EthernetLayer, this.EthernetLayer);
            this.PayloadLayer = _layerExchanger.AssignUserValuesFromFilledLayerToOtherLayer(toCopyFrom.PayloadLayer, this.PayloadLayer);
            this.IpV4Layer = _layerExchanger.AssignUserValuesFromFilledLayerToOtherLayer(toCopyFrom.IpV4Layer, this.IpV4Layer);

            return this;
        }
    }
}