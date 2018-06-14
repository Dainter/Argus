using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class Incharge : Edge
    {
        public Incharge(string newValue = "1") : base(GraphCommonStrings.Incharge, newValue)
        {
        }

        public Incharge(XmlElement xNode) : base(xNode)
        {
        }
    }
}