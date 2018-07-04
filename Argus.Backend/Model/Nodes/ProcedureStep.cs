using System;
using System.Data;
using System.Xml;
using Argus.Backend.Utility;
using GraphDB.Contract.Serial;
using GraphDB.Core;

namespace Argus.Backend.Model.Nodes
{
    public class ProcedureStep : Node
    {
        public string Type => GetType().Name;

        [XmlSerializable]
        public string Description { get; set; }


        public ProcedureStep(string name, string description) : base(name)
        {
            Description = description;
        }

        public ProcedureStep(Node oriNode) : base(oriNode)
        {
            if (oriNode.GetType() != GetType())
            {
                throw new ArgumentException("Node init failed, invalid node:" + oriNode.Name
                                                                              + " type:" + oriNode.GetType().Name + " try to convert as " + Type + ".");
            }

            ProcedureStep newNode = oriNode as ProcedureStep;
            if (newNode == null)
            {
                throw new ArgumentNullException("Node init failed, invalid node:" + oriNode.Name);
            }
            Description = newNode.Description;
        }

        public ProcedureStep(XmlElement xNode) : base(xNode)
        {
            try
            {
                Description = xNode.GetText("Description");
            }
            catch (Exception)
            {
                throw new DataException(GetType().Name + ":" + Name + "'s data is invalid, please check the DB.");
            }
        }
    }
}