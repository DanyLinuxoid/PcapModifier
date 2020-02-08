using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace PcapPacketModifier.Logic.Layers.Interfaces
{
    /// <summary>
    /// Responsible for layer extraction functions
    /// </summary>
    public interface ILayerExtractor
    {
        /// <summary>
        /// Extracts Tcp layer from packet
        /// </summary>
        /// <param name="packet">Packet to extract layer from</param>
        /// <returns>New Tcp layer</returns>
        TcpLayer ExtractTcpLayerFromPacket(Packet packet);

        /// <summary>
        /// Extracts Udp layer from packet
        /// </summary>
        /// <param name="packet">Packet to extract layer from</param>
        /// <returns>New udp layer</returns>
        UdpLayer ExtractUdpLayerFromPacket(Packet packet);

        /// <summary>
        /// Extracts Ethernet layer from packet
        /// </summary>
        /// <param name="packet">Packet to extract layer from</param>
        /// <returns>New Ethernet layer</returns>
        EthernetLayer ExtractEthernetLayerFromPacket(Packet packet);

        /// <summary>
        /// Extracts IpV4 layer from packet
        /// </summary>
        /// <param name="packet">Packet to extract layer from</param>
        /// <returns>New IpV4 layer</returns>
        IpV4Layer ExtractIpv4LayerFromPacket(Packet packet);

        /// <summary>
        /// Extracts Payload layer from packet
        /// </summary>
        /// <param name="packet">Packet to extract layer from</param>
        /// <returns>New Payload layer</returns>
        PayloadLayer ExtractPayloadLayerFromPacket(Packet packet);

        /// <summary>
        /// Extracts Icmp layer from packet
        /// </summary>
        /// <param name="packet">Packet to extract layer from</param>
        /// <returns></returns>
        IcmpLayer ExtractIcmpLayerFromPacket(Packet packet);
    }
}
