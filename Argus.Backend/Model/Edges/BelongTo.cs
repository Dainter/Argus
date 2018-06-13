using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class BelongTo : Edge
    {
        public BelongTo(string newValue = "1") : base(GraphCommonStrings.BelongTo, newValue)
        {
        }

        public BelongTo(XmlElement xNode) : base(xNode)
        {
        }
    }
}