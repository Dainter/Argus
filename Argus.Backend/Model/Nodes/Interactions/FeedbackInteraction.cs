using System;
using System.Data;
using System.Xml;
using Argus.Backend.Business;
using Argus.Backend.Utility;
using GraphDB.Contract.Serial;

namespace Argus.Backend.Model.Nodes.Interactions
{
    public class FeedbackInteraction : AbstractInteraction
    {
        [XmlSerializable]
        public string FixPlan { get; set; }

        [XmlSerializable]
        public string Feedback { get; set; }

        public FeedbackInteraction(string handler) : base(ProcedureStepEunm.Feedback.ToString(), handler)
        {
            FixPlan = "";
            Feedback = "";
        }

        public FeedbackInteraction(XmlElement xNode) : base(xNode)
        {
            try
            {
                FixPlan = xNode.GetText("FixPlan");
                Feedback = xNode.GetText("Feedback");
            }
            catch (Exception)
            {
                throw new DataException(GetType().Name + " on " + CreateTime + "'s data is invalid, please check the DB.");
            }
        }
    }
}