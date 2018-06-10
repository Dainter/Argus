using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GraphDB.Core;
using GraphDB.Utility;

namespace GraphDB.Contract
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
            InitGraphs();
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

        private void InitGraphs()
        {
            myGraphs = new List<Graph>();
            var myGraphList = myConfiguration.GetGraphs();
            foreach (var curItem in myGraphList)
            {
                myGraphs.Add(new Graph(curItem.Key, curItem.Value));
            }
        }

        #region Interfaces
        public IEnumerable<Graph> GetGraphs(string name = null)
        {
            if (name == null)
            {
                return myGraphs;
            }

            return myGraphs.Where(x => x.Name == name);
        }

        public void CreateNewGraph(string name)
        {
            VerifyName(name, false);
            Graph newGraph = new Graph(name);
            myGraphs.Add(newGraph);
        }

        public void RemoveGraph(string name)
        {
            VerifyName(name, true);
            throw new NotImplementedException();
        }

        public void RefreshGraphs()
        {
            myConfiguration = new Configuration();
            InitGraphs();
        }

        public void CommitGraphs()
        {
            foreach (var curItem in myGraphs)
            {
                curItem.SaveDataBase();
            }
        }

        #endregion

        private void VerifyName(string name, bool shouldBeExists)
        {
            if (name == null || name.Trim() == "")
            {
                throw new ArgumentException("Graph name should be null or empty string.");
            }

            if (shouldBeExists != myGraphs.Any(x => x.Name == name))
            {
                if (shouldBeExists)
                {
                    throw new ArgumentException("Can't find graph named: " + name + ".");
                }
                throw new ArgumentException("Graph named: " + name + " is already exists.");
            }
        }
    }
}