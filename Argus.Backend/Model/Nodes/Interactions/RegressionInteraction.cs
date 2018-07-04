using System;
using System.Data;
using System.Xml;
using Argus.Backend.Business;
using Argus.Backend.Utility;
using GraphDB.Contract.Serial;

namespace Argus.Backend.Model.Nodes.Interactions
{
    public class RegressionInteraction : AbstractInteraction
    {
        [XmlSerializable]
        public string TestReport { get; set; }

        public RegressionInteraction(string handler) : base(ProcedureStepEunm.Regression.ToString(), handler)
        {
            TestReport = "";
        }

        public RegressionInteraction(XmlElement xNode) : base(xNode)
        {
            try
            {
                TestReport = xNode.GetText("TestReport");
            }
            catch (Exception)
            {
                throw new DataException(GetType().Name + " on " + CreateTime + "'s data is invalid, please check the DB.");
            }
        }
    }
}