﻿using PcapPacketModifier.Logic.Tools.Interfaces;
using System.IO;

namespace PcapPacketModifier.Logic.Tools
{
    public class PathProvider : PathProviderBase, IPathProvider
    {
        /// <summary>
        /// Gets default path for log file
        /// </summary>
        /// <returns>Path for log file</returns>
        public string GetDefaultPathForLog()
        {
            return Directory.GetCurrentDirectory() + "\\log.txt";
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
