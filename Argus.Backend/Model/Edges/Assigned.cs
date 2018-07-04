using System;
using System.Globalization;
using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class Assigned : Edge
    {
        static readonly CultureInfo CurCultureInfo = new CultureInfo("en-us");

        public Assigned() : base(GraphCommonStrings.Assigned, DateTime.Now.ToString(CurCultureInfo))
        {
        }

        public Assigned(XmlElement xNode) : base(xNode)
        {
        }
    }
}