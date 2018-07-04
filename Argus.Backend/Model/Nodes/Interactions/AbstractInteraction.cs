using System;
using System.Data;
using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Contract.Serial;

namespace Argus.Backend.Model.Nodes.Interactions
{
    public abstract class AbstractInteraction
    {
        [XmlSerializable]
        public DateTime CreateTime { get; }

        [XmlSerializable]
        public string CurrentStep { get; }

        [XmlSerializable]
        public string Handler { get; }

        protected AbstractInteraction(string currentStep, string handler)
        {
            CreateTime = DateTime.Now;
            CurrentStep = currentStep;
            Handler = handler;
        }

        protected AbstractInteraction(XmlElement xNode)
        {
            try
            {
                CreateTime = Convert.ToDateTime(xNode.GetText("CreateTime"));
                CurrentStep = xNode.GetText("CurrentStep");
                Handler = xNode.GetText("Handler");
            }
            catch (Exception)
            {
                throw new DataException("Data is invalid, please check the DB.");
            }
        }

        public override string ToString()
        {
            return CurrentStep + " by " + Handler;
        }
    }
}