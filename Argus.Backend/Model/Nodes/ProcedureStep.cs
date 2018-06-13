using System;
using System.Xml;
using GraphDB.Contract.Serial;
using GraphDB.Core;
using GraphDB.Utility;

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
            Description = xNode.GetText("Description");
        }
    }
}