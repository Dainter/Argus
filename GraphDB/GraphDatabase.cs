using System;
using System.Collections.Generic;
using System.IO;
using GraphDB.Core;
using GraphDB.Utility;

namespace GraphDB
{
    public class GraphDatabase
    {
        private Configuration myConfiguration;
        private List<Graph> myGraphs;

        public string Name { get; private set; }

        public string Path { get; private set; }

        public GraphDatabase(string name)
        {
            Name = name;
            Path = VerifyOrCreate();
            myConfiguration = new Configuration();
        }

        private string VerifyOrCreate()
        {
            string dbDirectory = Environment.CurrentDirectory;
            string path = dbDirectory + @"\" + Name;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (dirInfo.Exists)
            {
                return path;
            }
            dirInfo.Create();
            return dirInfo.FullName;
        }

    }
}