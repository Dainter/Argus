using System;
using System.Collections.Generic;
using System.IO;

namespace GraphDB.Utility
{
    internal class Configuration
    {
        public Dictionary<string, string> Graphs { get; }

        public Configuration()
        {
            Graphs = GetGraphs();
        }

        private Dictionary<string, string> GetGraphs()
        {
            List<string> graphFileList = SerializableHelper.GetConfiguration("GraphList");
            Dictionary<string, string> graphFiles = new Dictionary<string, string>();
            if (graphFileList.Count == 0)
            {
                return graphFiles;
            }
            
            string dbDirectory = Environment.CurrentDirectory;
            foreach (var curItem in graphFileList)
            {
                FileInfo fileInfo = new FileInfo(dbDirectory + curItem);
                if (!fileInfo.Exists)
                {
                    continue;
                }
                graphFiles.Add(fileInfo.Name, fileInfo.FullName);
            }

            return graphFiles;
        }
        
    }
}