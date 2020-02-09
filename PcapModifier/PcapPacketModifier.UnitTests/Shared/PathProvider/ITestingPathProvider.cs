using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Shared.PathProvider
{
    /// <summary>
    /// Provides functions for getting various paths to test objects
    /// </summary>
    public interface ITestingPathProvider
    {
        /// <summary>
        /// Calls method to get path to solution and then adds path to file in solution
        /// </summary>
        /// <returns>Path to test packet</returns>
        string GetPathToTestPacket();

        /// <summary>
        /// Calls method to get path to solution and then adds path to file in solution
        /// </summary>
        /// <returns>Path to test log</returns>
        string GetPathToTestLog();

        /// <summary>
        /// Gets default path to solution
        /// </summary>
        /// <returns>Path to solution as string</returns>
        string GetDefaultPathToSolution();
    }
}
