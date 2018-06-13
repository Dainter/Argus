using System.Xml;
using GraphDB.Core;

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
        }
    }
}