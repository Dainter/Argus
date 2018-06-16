using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class As : Edge
    {
        public As(string newValue = "1") : base(GraphCommonStrings.As, newValue)
        {
        }

        public As(XmlElement xNode) : base(xNode)
        {
        }
    }
}