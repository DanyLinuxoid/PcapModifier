using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Logic.Extensions;
using PcapPacketModifier.Logic.Factories.Interfaces;
using PcapPacketModifier.Logic.Modules.StructHandlers.Interfaces;
using System;

namespace PcapPacketModifier.Logic.Modules.StructHandlers
{
    /// <summary>
    /// Responsible for handling modules of value type
    /// </summary>
    public class ModuleStructHandler : IModuleStructHandler
    {
        private readonly IGenericInstanceCreator _genericInstanceCreator;
        private readonly IModuleStructArgumentsHandler _moduleStructArguments;

        public ModuleStructHandler(IGenericInstanceCreator genericInstanceCreator,
                                                IModuleStructArgumentsHandler moduleStructArguments)
        {
            _genericInstanceCreator = genericInstanceCreator;
            _moduleStructArguments = moduleStructArguments;
        }

        /// <summary>
        /// Checks if type is comon struct, that does not requires any parameters, returns
        /// parsed value if type is common
        /// </summary>
        /// <param name="type">Type to parse</param>
        /// <param name="input">String to parse</param>
        /// <returns>Parsed value</returns>
        public object CheckIfTypeIsCommonStructAndReturnObject(Type type, string input)
        {
            if (type == typeof(ushort) ||
                type == typeof(ushort?)) return input.ToType<ushort>();
            if (type == typeof(byte) ||
                type == typeof(byte?)) return input.ToType<byte>();
            if (type == typeof(bool) ||
                type == typeof(bool?)) return input.ToType<bool>();
            if (type == typeof(uint) ||
                type == typeof(uint?)) return input.ToType<uint>();
            if (type == typeof(int) ||
                type == typeof(int?)) return input.ToType<int>();

            return null;
        }

        /// <summary>
        /// Checks if struct must be with arguments, returns parsed object wwith arguments
        /// if type is foundand if conversion was successfull
        /// </summary>
        /// <param name="type">Type to check and parse</param>
        /// <param name="input">String to parse</param>
        /// <returns></returns>
        public object CheckIfStructMustHaveArgumentsAndReturnObject(Type type, string input)
        {
            if (type == typeof(MacAddress))
            {
                var result = _genericInstanceCreator.TryCreateNewInstance<MacAddress>(input);
                if (result != default(MacAddress))
                    return result;
            }
            if (type == typeof(IpV4Address))
            {
                var result = _genericInstanceCreator.TryCreateNewInstance<IpV4Address>(input);
                if (result != default(IpV4Address))
                    return result;
            }

            if (type == typeof(IpV4Fragmentation))
            {
                var parameters = _moduleStructArguments.InputToParametersForIpV4Fragmentation(input);
                if (parameters != null)
                    return _genericInstanceCreator.TryCreateNewInstance<IpV4Fragmentation>(parameters);
            }

            return null;
        }
    }
}
