using System.Xml;
using GraphDB.Core;

namespace Argus.Backend.Model.Nodes
{
    public class UserGroup : Node
    {
        public UserGroup(string name) : base(name)
        {
        }

        public UserGroup(Node oriNode) : base(oriNode)
        {
        }

        public UserGroup(XmlElement xNode) : base(xNode)
        {
        }
    }
}