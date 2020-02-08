using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using System;

namespace PcapPacketModifier.Logic.Extensions
{
    /// <summary>
    /// Extensions for .NET Type class
    /// </summary>
    public static class TypeExtentions
    {
        /// <summary>
        /// Checks if type is any that can be skipped (skipped because it cannot be displayed, implemented, or it
        /// is not required to be changed(because in building process it is set to default value))
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if type must be skipped, false otherwise</returns>
        public static bool IsTypeToSkip(this Type type)
        {
            if (type == typeof(TcpOptions) ||
                type == typeof(IpV4Options) ||
                type == typeof(IpV4Protocol?) ||
                type == typeof(EthernetType))
            {
                return true;
            }

            return false;
        }
    }
}
