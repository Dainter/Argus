using System;
using System.Globalization;
using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class AssignTo : Edge
    {
        static readonly CultureInfo CurCultureInfo = new CultureInfo("en-us");

        public AssignTo() : base(GraphCommonStrings.AssignTo, DateTime.Now.ToString(CurCultureInfo))
        {
        }

        public AssignTo(XmlElement xNode) : base(xNode)
        {
        }
    }
}