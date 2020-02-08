using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Logic.Extensions;
using PcapPacketModifier.Logic.Helpers.Interfaces;
using PcapPacketModifier.Logic.Modules.StructHandlers.Interfaces;

namespace PcapPacketModifier.Logic.Modules.StructHandlers
{
    /// <summary>
    /// Provides implementation for modules to handle struct arguments, in case if struct requires some
    /// </summary>
    public class ModuleStructArgumentsHandler : IModuleStructArgumentsHandler
    {
        private readonly IStringHelper _stringHelper;

        public ModuleStructArgumentsHandler(IStringHelper stringHelper)
        {
            _stringHelper = stringHelper;
        }

        /// <summary>
        /// Returns arguments for IpV4Fragmentation struct
        /// </summary>
        /// <param name="userInput">String to parse on struct parameters</param>
        /// <returns>Parameters for IpV4Fragmentation</returns>
        public object[] InputToParametersForIpV4Fragmentation(string userInput)
        {
            var parameters = _stringHelper.StringWithSignSeparatorsToArrayOfValues(userInput);
            if (parameters.Length == 2)
            {
                var firstParam = parameters[0].ToEnum(typeof(IpV4FragmentationOptions));
                var secondParam = parameters[1].ToType<ushort>();
                if (firstParam != null &&
                    secondParam != null)
                {
                    return new object[] { firstParam, secondParam };
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if input is valid ipv4 address
        /// </summary>
        /// <param name="input">Input to check</param>
        /// <returns>Created struct with ipV4 address</returns>
        public string CheckIfInputIsValidIpV4AddressAndReturnInput(string input)
        {
            return _stringHelper.IsValidIpAddress(input) ? input : null;
        }

        /// <summary>
        /// Checks if input is valid ipv4 address
        /// </summary>
        /// <param name="input">Input to check</param>
        /// <returns>Created struct with ipV4 address</returns>
        public string CheckIfInputIsValidMacAddressAndReturnInput(string input)
        {
            return _stringHelper.IsValidMacAddress(input) ? input : null;
        }
    }
}