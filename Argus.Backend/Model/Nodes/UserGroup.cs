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
    public class UserGroup : Node
    {
        public string Type => GetType().Name;

        [XmlSerializable]
        public string GroupType { get; private set; }

        [XmlSerializable]
        public string Description { get; set; }

        public IEnumerable<User> Users => GetUsers();

        public User Leader => GetLeader();

        public IEnumerable<User> Members => GetMembers();

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
            try
            {
                GroupType = xNode.GetText("GroupType");
                Description = xNode.GetText("Description");
            }
            catch (Exception)
            {
                throw new DataException( GetType().Name +":" + Name + "'s data is invalid, please check the DB.");
            }
        }

        private IEnumerable<User> GetUsers()
        {
            var users = GetEdgesByType(new List<Type> { typeof(LeadBy), typeof(Include) }, EdgeDirection.Out);

            if (users == null || !users.Any())
            {
                return new List<User>();
            }
            List<User> userList = new List<User>();
            var leader = Leader;
            if (leader != null)
            {
                userList.Add(leader);
            }
            userList.AddRange(Members);
            return userList;
        }

        private User GetLeader()
        {
            var users = GetEdgesByType(new List<Type> { typeof(LeadBy) }, EdgeDirection.Out);
            if (users == null || !users.Any())
            {
                return new User("","");
            }
            return users.First().To as User;
        }

        private IEnumerable<User> GetMembers()
        {
            var users = GetEdgesByType(new List<Type> { typeof(Include) }, EdgeDirection.Out);

            if (users == null || !users.Any())
            {
                return new List<User>();
            }

            List<User> userList = new List<User>();
            foreach (var curItem in users)
            {
                if (curItem.To.GetType() == typeof(User))
                {
                    userList.Add(curItem.To as User);
                }
                else if (curItem.To.GetType() == typeof(UserGroup))
                {
                    userList.AddRange(((UserGroup) curItem.To).Users);
                }
            }
            return userList;
        }
    }
}