namespace PcapPacketModifier.Logic.Factories.Interfaces
{
    /// <summary>
    /// Creates instances
    /// </summary>
    public interface IGenericInstanceCreator
    {
        /// <summary>
        /// Creates instance of any type, with parameters if object has such constructor
        /// </summary>
        /// <typeparam name="T">Type to create</typeparam>
        /// <param name="input">Input as parameters</param>
        /// <returns>Created instance with or without parameters</returns>
        T TryCreateNewInstance<T>(params object[] input);
    }
}
