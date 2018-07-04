using System;
using System.Data;
using System.Xml;
using Argus.Backend.Business;
using Argus.Backend.Utility;
using GraphDB.Contract.Serial;

namespace Argus.Backend.Model.Nodes.Interactions
{
    public class SolveInteraction : AbstractInteraction
    {
        [XmlSerializable]
        public string RootCause { get; set; }

        [XmlSerializable]
        public string Implementation { get; set; }

        [XmlSerializable]
        public string TestSuggestion { get; set; }

        public SolveInteraction( string handler ) : base(ProcedureStepEunm.Solve.ToString(), handler)
        {
            RootCause = "";
            Implementation = "";
            TestSuggestion = "";
        }

        public SolveInteraction(XmlElement xNode) : base(xNode)
        {
            try
            {
                RootCause = xNode.GetText("RootCause");
                Implementation = xNode.GetText("Implementation");
                TestSuggestion = xNode.GetText("TestSuggestion");
            }
            catch (Exception)
            {
                throw new DataException(GetType().Name + " on " + CreateTime + "'s data is invalid, please check the DB.");
            }
        }
    }
}