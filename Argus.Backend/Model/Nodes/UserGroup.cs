using System;
using System.Xml;
using GraphDB.Contract.Serial;
using GraphDB.Core;
using GraphDB.Utility;

namespace Argus.Backend.Model.Nodes
{
    public class UserGroup : Node
    {
        public string Type => GetType().Name;

        [XmlSerializable]
        public string GroupType { get; private set; }

        [XmlSerializable]
        public string Description { get; set; }

        public UserGroup(string name, string type, string description) : base(name)
        {
            GroupType = type;
            Description = description;
        }

        public UserGroup(Node oriNode) : base(oriNode)
        {
            if (oriNode.GetType() != GetType())
            {
                throw new ArgumentException("Node init failed, invalid node:" + oriNode.Name
                                                                              + " type:" + oriNode.GetType().Name + " try to convert as " + Type + ".");
            }

            UserGroup newNode = oriNode as UserGroup;
            if (newNode == null)
            {
                throw new ArgumentNullException("Node init failed, invalid node:" + oriNode.Name);
            }

            GroupType = newNode.GroupType;
            Description = newNode.Description;
        }

        public UserGroup(XmlElement xNode) : base(xNode)
        {
            GroupType = xNode.GetText("GroupType");
            Description = xNode.GetText("Description");
        }
    }
}