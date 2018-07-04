using System;
using System.Globalization;
using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend.Model.Edges
{
    public class CreateBy : Edge
    {
        static readonly CultureInfo CurCultureInfo = new CultureInfo("en-us");

        public CreateBy() : base(GraphCommonStrings.CreateBy, DateTime.Now.ToString(CurCultureInfo))
        {
        }

        public CreateBy(XmlElement xNode) : base(xNode)
        {
        }
    }
}