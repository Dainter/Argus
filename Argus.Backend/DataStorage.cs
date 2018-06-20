using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public void OpenOrCreate(string name, string path)
        {
            if (myGraphs.ContainsKey(name))
            {
                throw new ArgumentException("The database with same name already exists.");
            }
            var newGraph = new Graph(name, path);
            myGraphs.Add(name, newGraph);
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