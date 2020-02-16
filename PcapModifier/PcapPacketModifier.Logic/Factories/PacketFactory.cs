using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using PcapPacketModifier.Logic.Factories.Interfaces;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Logic.Packets.Models;
using PcapPacketModifier.Userdata.Packets.Interfaces;

namespace PcapPacketModifier.Logic.Factories
{
    /// <summary>
    /// Simple static factory for packet creation
    /// </summary>
    public class PacketFactory : IPacketFactory
    {
        private readonly ILayerExchanger _layerExchanger;
        private readonly ILayerModifier _layerModifier;

        public PacketFactory(ILayerExchanger layerExchanger,
                                      ILayerModifier layerModifier)
        {
            _layerExchanger = layerExchanger;
            _layerModifier = layerModifier;
        }

        /// <summary>
        /// Creates packet by protocol and provided input to constructor
        /// </summary>
        /// <param name="protocol">Packet protocol</param>
        /// <returns>New Custom packet of provided protocol with provided values in it</returns>
        public INewPacket GetPacketByProtocol(IpV4Protocol protocol)
        {
            switch (protocol)
            {
                case IpV4Protocol.Tcp:
                    return new CustomTcpPacket(_layerModifier, _layerExchanger)
                    {
                        EthernetLayer = new EthernetLayer(),
                        IpV4Layer = new IpV4Layer(),
                        PayloadLayer = new PayloadLayer(),
                        TcpLayer = new TcpLayer(),
                    };
                case IpV4Protocol.Udp:
                    return new CustomUdpPacket(_layerModifier, _layerExchanger)
                    {
                        EthernetLayer = new EthernetLayer(),
                        IpV4Layer = new IpV4Layer(),
                        PayloadLayer = new PayloadLayer(),
                        UdpLayer = new UdpLayer(),
                    };
                case IpV4Protocol.InternetControlMessageProtocol:
                    return new CustomIcmpPacket(_layerModifier, _layerExchanger)
                    {
                        EthernetLayer = new EthernetLayer(),
                        IcmpLayer = new IcmpEchoLayer(),
                        IpV4Layer = new IpV4Layer(),
                    };

                default:
                    return null;
            }
        }
    }
}