using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class CurrentStep : Edge
    {
        public CurrentStep(string newValue = "1") : base(GraphCommonStrings.CurrentStep, newValue)
        {
        }

        public CurrentStep(XmlElement xNode) : base(xNode)
        {
        }
    }
}