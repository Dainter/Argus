using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class HandleBy : Edge
    {
        public HandleBy(string newValue = "1") : base(GraphCommonStrings.HandleBy, newValue)
        {
        }

        public HandleBy(XmlElement xNode) : base(xNode)
        {
        }
    }
}