using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class Include : Edge
    {
        public Include(string newValue = "1") : base(GraphCommonStrings.Include, newValue)
        {
        }

        public Include(XmlElement xNode) : base(xNode)
        {
        }
    }
}