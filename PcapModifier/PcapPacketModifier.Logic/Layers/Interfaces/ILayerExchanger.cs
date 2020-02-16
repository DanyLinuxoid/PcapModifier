using PcapDotNet.Packets;

namespace PcapPacketModifier.Logic.Layers.Interfaces
{
    /// <summary>
    /// Mapper, changes values from one layer to another if this value is not default (some user value)
    /// Is used to automatically inject user values in intercepted packets
    /// </summary>
    public interface ILayerExchanger
    {
        /// <summary>
        /// Assigns user values from any supported layer to other layer, maps values
        /// </summary>
        /// <param name="layerToCopyFrom">Layer to copy from</param>
        /// <param name="layerToCopyTo">Layer to copy to</param>
        /// <returns>Modified layer with user values</returns>
        T AssignUserValuesFromFilledLayerToOtherLayer<T>(T layerToCopyFrom, T layerToCopyTo) where T : Layer;
    }
}
