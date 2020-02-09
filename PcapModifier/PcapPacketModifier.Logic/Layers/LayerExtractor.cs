using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using PcapPacketModifier.Logic.Layers.Interfaces;

namespace PcapPacketModifier.Logic.Layers
{
    /// <summary>
    /// Responsible for extracting data from layer
    /// </summary>
    public class LayerExtractor : ILayerExtractor
    {
        /// <summary>
        /// Extracts modules from Tcp layer
        /// </summary>
        /// <param name="packet">Packet to extract data from</param>
        /// <returns>Tcp layer with extracted data</returns>
        public TcpLayer ExtractTcpLayerFromPacket(Packet packet)
        {
            if (packet is null)
            {
                throw new System.ArgumentNullException(nameof(packet));
            }

            if (packet.Ethernet.IpV4 == null)
            {
                throw new System.ArgumentNullException(nameof(packet));
            }

            return packet.Ethernet.IpV4.Tcp.ExtractLayer() as TcpLayer;
        }

        /// <summary>
        /// Extracts modules from Ethernet layer
        /// </summary>
        /// <param name="packet">Packet to extract data from</param>
        /// <returns>Ethernet layer with extracted data</returns>
        public EthernetLayer ExtractEthernetLayerFromPacket(Packet packet)
        {
            if (packet is null)
            {
                throw new System.ArgumentNullException(nameof(packet));
            }

            if (packet.Ethernet == null)
            {
                throw new System.ArgumentNullException(nameof(packet));
            }

            return packet.Ethernet.ExtractLayer() as EthernetLayer;
        }

        /// <summary>
        /// Extracts modules from IpV4 layer
        /// </summary>
        /// <param name="packet">Packet to extract data from</param>
        /// <returns>IpV4 layer with extracted data</returns>
        public IpV4Layer ExtractIpv4LayerFromPacket(Packet packet)
        {
            if (packet is null)
            {
                throw new System.ArgumentNullException(nameof(packet));
            }

            if (packet.Ethernet.IpV4 == null)
            {
                throw new System.ArgumentNullException(nameof(packet));
            }

            return packet.Ethernet.IpV4.ExtractLayer() as IpV4Layer;
        }

        /// <summary>
        /// Extracts modules from Payload layer
        /// </summary>
        /// <param name="packet">Packet to extract data from</param>
        /// <returns>Payload layer with extracted data</returns>
        public PayloadLayer ExtractPayloadLayerFromPacket(Packet packet)
        {
            if (packet is null)
            {
                throw new System.ArgumentNullException(nameof(packet));
            }

            if (packet.Ethernet.Payload == null)
            {
                throw new System.ArgumentNullException(nameof(packet));
            }

            return packet.Ethernet.Payload.ExtractLayer() as PayloadLayer;
        }

        /// <summary>
        /// Extracts Udp layer from packet
        /// </summary>
        /// <param name="packet">Packet to extract layer from</param>
        /// <returns>Udp layer that can be modified</returns>

        public UdpLayer ExtractUdpLayerFromPacket(Packet packet)
        {
            if (packet is null)
            {
                throw new System.ArgumentNullException(nameof(packet));
            }

            if (packet.Ethernet.IpV4.Udp == null)
            {
                throw new System.ArgumentNullException(nameof(packet));
            }

            return packet.Ethernet.IpV4.Udp.ExtractLayer() as UdpLayer;
        }

        /// <summary>
        /// Extracts Icmp layer from packet
        /// </summary>
        /// <param name="packet">Packet to extract layer from</param>
        /// <returns>Icmp layer that can be modified</returns>
        public IcmpLayer ExtractIcmpLayerFromPacket(Packet packet)
        {
            if (packet is null)
            {
                throw new System.ArgumentNullException(nameof(packet));
            }

            if (packet.Ethernet.IpV4.Icmp == null)
            {
                throw new System.ArgumentNullException(nameof(packet));
            }

            return packet.Ethernet.IpV4.Icmp.ExtractLayer() as IcmpLayer;
        }
    }
}