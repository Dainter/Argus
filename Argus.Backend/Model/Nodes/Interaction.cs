using System.Xml;
using GraphDB.Core;

namespace Argus.Backend.Model.Nodes
{
    public class Interaction : Node
    {
        public Interaction(string name) : base(name)
        {
        }

        public Interaction(Node oriNode) : base(oriNode)
        {
        }

        public Interaction(XmlElement xNode) : base(xNode)
        {
        }
    }
}