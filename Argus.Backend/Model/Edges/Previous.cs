using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class Previous : Edge
    {
        public Previous(string newValue = "1") : base(GraphCommonStrings.Previous, newValue)
        {
        }

        public Previous(XmlElement xNode) : base(xNode)
        {
        }
    }
}