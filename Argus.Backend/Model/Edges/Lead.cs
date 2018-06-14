using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class Lead : Edge
    {
        public Lead(string newValue = "1") : base(GraphCommonStrings.Lead, newValue)
        {
        }

        public Lead(XmlElement xNode) : base(xNode)
        {
        }
    }
}