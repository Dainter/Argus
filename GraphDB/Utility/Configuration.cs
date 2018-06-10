using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace GraphDB.Utility
{
    internal class Configuration
    {
        private readonly Dictionary<string, string> myGraphs;

        public List<Assembly> Assemblies { get; }

        public Configuration()
        {
            Assemblies = GetAssemblyList();
            myGraphs = LoadGraphs();
        }

        private Dictionary<string, string> LoadGraphs()
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
                graphFiles.Add(fileInfo.Name.Replace(fileInfo.Extension,""), fileInfo.FullName);
            }

            return graphFiles;
        }

        private List<Assembly> GetAssemblyList()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            string asmName = Path.GetFileName(path);
            List<string> assemList = SerializableHelper.GetConfiguration("SerialAssemblyList");
            List<Assembly> assemblies = new List<Assembly>();
            foreach (string curItem in assemList)
            {
                assemblies.Add(Assembly.LoadFile(path.Replace(asmName, curItem)));
            }

            return assemblies;
        }

        public Dictionary<string, string> GetGraphs()
        {
            return myGraphs ?? new Dictionary<string, string>();
        }

    }
}