using System;
using System.Globalization;
using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class Create : Edge
    {
        static readonly CultureInfo CurCultureInfo = new CultureInfo("en-us");

        public Create() : base(GraphCommonStrings.Create, DateTime.Now.ToString(CurCultureInfo))
        {
        }

        public Create(XmlElement xNode) : base(xNode)
        {
        }
    }
}