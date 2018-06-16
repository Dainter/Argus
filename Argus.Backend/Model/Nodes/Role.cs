using System;
using System.Data;
using System.Xml;
using GraphDB.Contract.Serial;
using GraphDB.Core;
using GraphDB.Utility;

namespace Argus.Backend.Model.Nodes
{
    public class Role : Node
    {
        public string Type => GetType().Name;

        [XmlSerializable]
        public int Level { get; set; }

        [XmlSerializable]
        public string Description { get; set; }

        public Role(string name, int level, string description) : base(name)
        {
            Level = level;
            Description = description;
        }

        public Role(Node oriNode) : base(oriNode)
        {
            if (oriNode.GetType() != GetType())
            {
                throw new ArgumentException("Node init failed, invalid node:" + oriNode.Name
                                                                              + " type:" + oriNode.GetType().Name + " try to convert as " + Type + ".");
            }

            Role newNode = oriNode as Role;
            if (newNode == null)
            {
                throw new ArgumentNullException("Node init failed, invalid node:" + oriNode.Name);
            }

            Level = newNode.Level;
            Description = newNode.Description;
        }

        public Role(XmlElement xNode) : base(xNode)
        {
            try
            {
                Level = Convert.ToInt32(xNode.GetText("Level"));
                Description = xNode.GetText("Description");
            }
            catch (Exception)
            {
                throw new DataException(GetType().Name + ":" + Name + "'s data is invalid, please check the DB.");
            }
        }
    }
}