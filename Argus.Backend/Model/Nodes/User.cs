using System;
using System.Xml;
using GraphDB.Contract.Serial;
using GraphDB.Core;
using GraphDB.Utility;

namespace Argus.Backend.Model.Nodes
{
    public class User : Node
    {

        public string Type => GetType().Name;

        [XmlSerializable]
        public string Department { get; private set; }

        [XmlSerializable]
        public string MailBox { get; private set; }

        [XmlSerializable]
        public string Password { get; set; }

        public User(string name, string department) : base(name)
        {
            Department = department;
            MailBox = "";
            Password = "";
        }

        public User(Node oriNode) : base(oriNode)
        {
            if (oriNode.GetType() != GetType())
            {
                throw new ArgumentException("Node init failed, invalid node:" + oriNode.Name 
                                                                              +" type:" + oriNode.GetType().Name+" try to convert as "+ Type +".");
            }

            User newNode = oriNode as User;
            if (newNode == null)
            {
                throw new ArgumentNullException("Node init failed, invalid node:" + oriNode.Name);
            }
            Department = newNode.Department;
            MailBox = newNode.MailBox;
            Password = newNode.Password;
        }

        public User(XmlElement xNode) : base(xNode)
        {
            Department = xNode.GetText("Department");
            MailBox = xNode.GetText("MailBox");
            Password = xNode.GetText("Password");
        }
    }
}