using System;
using System.Collections.Generic;
using SimpleInjector;
using System.Linq;
using PcapPacketModifier.Logic.Packets;

namespace PcapPacketModifier
{
    /// <summary>
    /// Responsible for dependency configuration
    /// </summary>
    public static class DependencyConfigurator
    {
        /// <summary>
        /// Calls private methods to configure dependencies
        /// </summary>
        /// <param name="container">Dependency container</param>
        public static void ConfigureDependencies(Container container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            RegisterClases(GetClases(), container);
        }

        /// <summary>
        /// Get all clases from logic if class has dependency on interface
        /// </summary>
        /// <returns>IEnumerable of types, clases</returns>
        private static IEnumerable<Type> GetClases()
        {
            return typeof(PacketManager).Assembly.GetExportedTypes().
            Where(type => type.Namespace != null && type.Namespace.StartsWith("PcapPacketModifier.Logic", StringComparison.Ordinal)).
            Where(type => type.GetInterfaces().Length > 0).
            Where(type => !type.IsGenericType).
            Where(type => !type.IsEnum).
            Where(type => !type.IsAbstract);
        }

        /// <summary>
        /// Registers all clases from GetClases() method
        /// </summary>
        /// <param name="clases">Clases of any type</param>
        /// <param name="injectionContainer">Dependency container</param>
        private static void RegisterClases(IEnumerable<Type> clases, Container injectionContainer)
        {
            var injectedClases = new Dictionary<Type, Type>();

            foreach (var type in clases) // type is class
            {
                foreach (var interfce in type.GetInterfaces())
                {
                    if (!injectedClases.ContainsKey(interfce))
                    {
                        injectedClases.Add(interfce, type);
                    }
                }
            }

            foreach (var classWithInterfacePair in injectedClases)
            {
                injectionContainer.Register(classWithInterfacePair.Key, classWithInterfacePair.Value);
            }
        }
    }
}