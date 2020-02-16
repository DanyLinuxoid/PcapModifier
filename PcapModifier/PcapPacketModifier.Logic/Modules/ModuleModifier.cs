using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PcapDotNet.Packets;
using PcapPacketModifier.Logic.Extensions;
using PcapPacketModifier.Logic.Modules.Interfaces;
using PcapPacketModifier.Logic.UserExperience.Interfaces;

namespace PcapPacketModifier.Logic.Modules
{
    /// <summary>
    /// Responsible for module modification
    /// </summary>
    public class ModuleModifier : IModuleModifier
    {
        private readonly IUserExperience _userExperience;
        private readonly ISpecificModulesModifier _specificModuleModifier;

        public ModuleModifier(ISpecificModulesModifier specificModules,
                                        IUserExperience userExperience)
        {
            _specificModuleModifier = specificModules;
            _userExperience = userExperience;
        }

        /// <summary>
        /// Logic to modify layer properties in library class, as we cannot normally create valid copy of object from library, in order
        /// to modify it normally (cannot assign interfaces, etc). 
        /// Object properties are modified by using reflection on public {get; set;} methods, all others are untouched.
        /// </summary>
        /// <typeparam name="T">Generic layer type</typeparam>
        /// <param name="layer">Layer object</param>
        /// <returns>Generic modified layer</returns>
        public T ChangeLayerModulesBasedOnUserInput<T>(T layer) where T : Layer
        {
            if (layer == null)
            {
                throw new ArgumentNullException(nameof(layer));
            }

            _userExperience.UserTextDisplayer.PrintHelpingMessageBeforeModifyingLayer(layer);
            if (layer.GetType() == typeof(PayloadLayer))
            {
                _userExperience.UserTextDisplayer.DisplayPayloadData(layer as PayloadLayer);
            }

            var sourceProps = GetLayerPublicAndWritableProperties(layer);
            foreach (PropertyInfo property in sourceProps)
            {
                var fieldType = property.PropertyType;
                if (fieldType.IsTypeToSkip())
                {
                    continue;
                }

                // Printing property type, field, current value
                _userExperience.UserTextDisplayer.PrintModuleInfo(fieldType.ToString().Split('.').Last(), property.Name, property.GetValue(layer)?.ToString());

                string userInput = _userExperience.UserInputHandler.AskUserInputWhileInputContainsPatterns(property);
                if (!string.IsNullOrWhiteSpace(userInput))
                {
                    object valueOfType = _specificModuleModifier.HandleSpecificModule(property.PropertyType, userInput);
                    if (valueOfType != null)
                    {
                        property.SetValue(layer, valueOfType);
                        _userExperience.UserTextDisplayer.SuccessfullyModifiedModule();
                        continue;
                    }

                    _userExperience.UserTextDisplayer.FailedModifyingModule();
                }
            }

            return layer;
        }

        /// <summary>
        /// Get's layer properties
        /// </summary>
        /// <param name="layer">Layer to extract properties</param>
        /// <returns>List with properties to modify</returns>
        private List<PropertyInfo> GetLayerPublicAndWritableProperties(Layer layer)
        {
            return layer.GetType().GetProperties().Where(x => x.PropertyType.IsPublic &&
                                                                                       x.CanWrite).ToList();
        }
    }
}