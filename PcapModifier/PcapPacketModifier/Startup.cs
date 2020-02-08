using SimpleInjector;

namespace PcapPacketModifier
{
    /// <summary>
    /// Startup to process before logic will start
    /// </summary>
    public sealed class Startup : System.IDisposable
    {
        /// <summary>
        /// Container with dependency injection
        /// </summary>
        private readonly Container _injectorContainer = new Container();

        /// <summary>
        /// Configures startup by injecting dependencies in container
        /// </summary>
        /// <returns>Container with ready dependencies and registered clases</returns>
        public Container ConfigureStartupAndReturndReadyContainer()
        {
            ContainerFiller(_injectorContainer);
            return _injectorContainer;
        }

        /// <summary>
        /// Fills container with all dependecies, that logic needs
        /// </summary>
        /// <param name="container">Container with dependencies and registered clases</param>
        private static void ContainerFiller(Container container)
        {
            DependencyConfigurator.ConfigureDependencies(container);
        }

        /// <summary>
        /// Disposes container
        /// </summary>
        public void Dispose()
        {
            _injectorContainer.Dispose();
        }
    }
}