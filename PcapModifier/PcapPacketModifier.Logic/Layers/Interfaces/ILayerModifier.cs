
using PcapDotNet.Packets;

namespace PcapPacketModifier.Logic.Layers.Interfaces
{
    /// <summary>
    /// Provides top level functions for layer modifications
    /// </summary>
    public interface ILayerModifier
    {
        /// <summary>
        /// Modifies any layer
        /// </summary>
        /// <typeparam name="T">Type of layer</typeparam>
        /// <param name="layer">Layer to modify</param>
        /// <returns>Modified layer of provided type</returns>
        T ModifyLayer<T>(T layer) where T : Layer;
    }
}