using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Argus.Backend.Model.Nodes;
using GraphDB.Core;

namespace Argus.Backend
{
    public class DataStorage
    {
        private DataStorage()
        {
            myGraphs= new Dictionary<string, Graph>();
        }    

        private static DataStorage _myStorage;

        private static Dictionary<string, Graph> myGraphs;

        public static DataStorage GetStorage()
        {
            return _myStorage ?? (_myStorage = new DataStorage());
        }

        public void OpenOrCreate(string name, string dbPath, string assemblyPath)
        {
            if (myGraphs.ContainsKey(name))
            {
                throw new ArgumentException("The database with same name already exists.");
            }
            var newGraph = new Graph(name, dbPath, assemblyPath);
            myGraphs.Add(name, newGraph);
        }

        public string GetString()
        {
            return "Fuck you.";
        }

        public IEnumerable<string> GetUsers(string dbName)
        {
            Graph curGraph = GetGraph(dbName);
            var users = curGraph.GetNodesByType(typeof(User)).Select(x => x.Name);
            return null;
        }

        private Graph GetGraph(string dbName)
        {
            if (dbName == null || !myGraphs.ContainsKey(dbName))
            {
                throw new ArgumentException("The name of database is invalid.");
            }
            Graph curGraph = myGraphs[dbName];
            if (curGraph == null)
            {
                throw new DataException("The database' is invalid, please check the initialization.");
            }

            return curGraph;
        }

        public IEnumerable GetAll<T>(string name)
        {
            if (name == null || !myGraphs.ContainsKey(name))
            {
                throw new ArgumentException("The name of database is invalid.");
            }
            Graph curGraph = myGraphs[name];
            if (curGraph == null)
            {
                throw new DataException("The database' is invalid, please check the initialization.");
            }

            return curGraph.GetNodesByType(typeof(T)).Cast<T>();
        }
    }
}