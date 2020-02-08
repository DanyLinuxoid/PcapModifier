using PcapDotNet.Packets;

namespace PcapPacketModifier.Logic.Modules.Interfaces
{
    /// <summary>
    /// Provides functions to modify modules in layer
    /// </summary>
    public interface IModuleModifier
    {
        /// <summary>
        /// Modifies layer modules
        /// </summary>
        /// <param name="layer">Layer to modify</param>
        /// <returns>Modified layer</returns>
        T ChangeLayerModulesBasedOnUserInput<T>(T layer) where T : Layer;
    }
}