using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using Argus.Backend.Model.Edges;
using Argus.Backend.Utility;
using GraphDB.Contract.Enum;
using GraphDB.Contract.Serial;
using GraphDB.Core;

namespace Argus.Backend.Model.Nodes
{
    public class User : Node
    {

        public string Type => GetType().Name;

        [XmlSerializable]
        public string Department { get; private set; }

        public Role Role => GetRole();

        public IEnumerable<UserGroup> UserGroups =>GetUserGroups();

        [XmlSerializable]
        public string MailBox { get; private set; }

        [XmlSerializable]
        public string Password { get; set; }

        public IEnumerable<Task> SubmitTasks => GetSubmitTasks();

        public IEnumerable<Task> HandleTasks => GetHandleTasks();

        public User(string name, string department) : base(name)
        {
            Department = department;
            MailBox = name.ToLower() + "@siemens.com";
            Password = "123456";
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
            try
            {
                Department = xNode.GetText("Department");
                MailBox = xNode.GetText("MailBox");
                Password = xNode.GetText("Password");
            }
            catch (Exception)
            {
                throw new DataException(GetType().Name + ":" + Name + "'s data is invalid, please check the DB.");
            }
        }

        private Role GetRole()
        {
            var roles = GetEdgesByType(new List<Type> { typeof(As) }, EdgeDirection.Out);

            if( roles == null || !roles.Any() )
            {
                return new Role("Unknown", 10000, "Unknown");
            }
            return roles.First().To as Role;
        }

        private IEnumerable<UserGroup> GetUserGroups()
        {
            var groups = GetEdgesByType(new List<Type> { typeof(Lead) , typeof(BelongTo) }, EdgeDirection.Out);

            if (groups == null || !groups.Any())
            {
                return new List<UserGroup>();
            }
            return groups.Select( x => x.To as UserGroup);
        }

        private IEnumerable<Task> GetSubmitTasks()
        {
            var tasks = GetEdgesByType(new List<Type> { typeof(Create) }, EdgeDirection.Out);

            if (tasks == null || !tasks.Any())
            {
                return new List<Task>();
            }
            return tasks.Select(x => x.To as Task);
        }

        private IEnumerable<Task> GetHandleTasks()
        {
            var tasks = GetEdgesByType(new List<Type> { typeof(Assigned) }, EdgeDirection.Out);

            if (tasks == null || !tasks.Any())
            {
                return new List<Task>();
            }
            return tasks.Select(x => x.To as Task);
        }
    }
}