using System;
using System.Data;
using System.Xml;
using GraphDB.Core;
using GraphDB.Utility;

namespace Argus.Backend.Model.Nodes
{
    public class Task : Node
    {
        public Task(string name) : base(name)
        {
        }

        public Task(Node oriNode) : base(oriNode)
        {
        }

        public Task(XmlElement xNode) : base(xNode)
        {
            try
            {

            }
            catch (Exception)
            {
                throw new DataException(GetType().Name + ":" + Name + "'s data is invalid, please check the DB.");
            }
        }
    }
}