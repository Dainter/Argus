using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class LeadBy : Edge
    {
        public LeadBy(string newValue = "1") : base(GraphCommonStrings.LeadBy, newValue)
        {
        }

        public LeadBy(XmlElement xNode) : base(xNode)
        {
        }
    }
}