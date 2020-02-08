using System;

namespace PcapPacketModifier.Logic.Tools
{
    public abstract class PathProviderBase
    {
        /// <summary>
        /// Returns path to solution
        /// </summary>
        /// <returns>Path to solution as string</returns>
        protected virtual string GetPathToSolution()
        {
            string PathToSolution = AppDomain.CurrentDomain.BaseDirectory;
            return PathToSolution.Substring(0, PathToSolution.IndexOf("bin\\"));
        }
    }
}
