using System;
using System.Data;
using System.Xml;
using Argus.Backend.Business;
using Argus.Backend.Utility;
using GraphDB.Contract.Serial;

namespace Argus.Backend.Model.Nodes.Interactions
{
    public class EvaluateInteraction : AbstractInteraction
    {
        [XmlSerializable]
        public string SolutionComments { get; set; }

        public EvaluateInteraction( string handler ) : base(ProcedureStepEunm.Evaluate.ToString(), handler)
        {
            SolutionComments = "";
        }

        public EvaluateInteraction(XmlElement xNode) : base(xNode)
        {
            try
            {
                SolutionComments = xNode.GetText("SolutionComments");
            }
            catch (Exception)
            {
                throw new DataException(GetType().Name + " on " + CreateTime + "'s data is invalid, please check the DB.");
            }
        }
    }
}