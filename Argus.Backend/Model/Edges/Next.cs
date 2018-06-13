using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class Next : Edge
    {
        public Next(string newValue = "1") : base(GraphCommonStrings.Next, newValue)
        {

        }

        public Next(XmlElement xNode) : base(xNode)
        {
        }
    }
}