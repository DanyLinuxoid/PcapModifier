using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Userdata.Packets.Interfaces;

namespace PcapPacketModifier.Logic.Factories.Interfaces
{
    public interface IPacketFactory
    {
        /// <summary>
        /// Creates packet by protocol and provided input to constructor
        /// </summary>
        /// <param name="protocol">Packet protocol</param>
        /// <returns>New Custom packet of provided protocol with provided values in it</returns>
        INewPacket GetPacketByProtocol(IpV4Protocol protocol);
    }
}
