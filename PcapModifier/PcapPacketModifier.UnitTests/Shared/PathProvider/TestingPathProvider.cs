using PcapPacketModifier.Logic.Tools;

namespace UnitTests.Shared.PathProvider
{
    /// <summary>
    /// Provides functions for getting various paths to test objects
    /// </summary>
    public class TestingPathProvider : PathProviderBase, ITestingPathProvider
    {
        /// <summary>
        /// Calls method to get path to solution and then adds path to file in solution
        /// </summary>
        /// <returns>Path to test packet</returns>
        public string GetPathToTestPacket()
        {
            return GetPathToSolution() + "Shared\\DummyObjects\\PacketFiles\\somepacket.pcap";
        }

        /// <summary>
        /// Calls method to get path to solution and then adds path to file in solution
        /// </summary>
        /// <returns>Path to test log</returns>
        public string GetPathToTestLog()
        {
            return GetPathToSolution() + "Shared\\DummyObjects\\TextFiles\\testlog.txt";
        }

        /// <summary>
        /// Gets default path to solution
        /// </summary>
        /// <returns>Path to solution as string</returns>
        public string GetDefaultPathToSolution()
        {
            return base.GetPathToSolution();
        }
    }
}