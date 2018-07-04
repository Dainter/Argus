using System;
using System.Data;
using System.Xml;
using Argus.Backend.Business;
using Argus.Backend.Utility;
using GraphDB.Contract.Serial;

namespace Argus.Backend.Model.Nodes.Interactions
{
    public class TicketCheckInteraction : AbstractInteraction
    {
        [XmlSerializable]
        public string AnalysisResult { get; set; }

        [XmlSerializable]
        public string Suggestion { get; set; }

        public TicketCheckInteraction(string handler):base (ProcedureStepEunm.TicketCheck.ToString(), handler)
        {
            AnalysisResult = "";
            Suggestion = "";
        }

        public TicketCheckInteraction(XmlElement xNode):base(xNode)
        {
            try
            {
                AnalysisResult = xNode.GetText("AnalysisResult");
                Suggestion = xNode.GetText("Suggestion");
            }
            catch (Exception)
            {
                throw new DataException(GetType().Name + " on " + CreateTime + "'s data is invalid, please check the DB.");
            }
        }
    }
}