
using PcapDotNet.Packets;
using PcapPacketModifier.Logic.Layers.Interfaces;
using PcapPacketModifier.Logic.Modules.Interfaces;
using System;

namespace PcapPacketModifier.Logic.Layers
{
    /// <summary>
    /// Responsible for modifying layer
    /// </summary>
    public class LayerModifier : ILayerModifier
    {
        private readonly IModuleModifier _moduleModifier;

        public LayerModifier(IModuleModifier moduleModifier)
        {
            _moduleModifier = moduleModifier;
        }

        /// <summary>
        /// Modifies layer of any type
        /// </summary>
        /// <typeparam name="T">Layer type</typeparam>
        /// <param name="layer">Layer object</param>
        /// <returns>Modified layer</returns>
        public T ModifyLayer<T>(T layer) where T : Layer
        {
            if (layer is null)
            {
                throw new ArgumentNullException(nameof(layer));
            }

            return (T)_moduleModifier.ChangeLayerModulesBasedOnUserInput(layer);
        }
    }
}