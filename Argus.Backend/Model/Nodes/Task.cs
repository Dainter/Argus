using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml;
using Argus.Backend.Model.Edges;
using Argus.Backend.Model.Nodes.Interactions;
using Argus.Backend.Utility;
using GraphDB.Contract.Enum;
using GraphDB.Contract.Serial;
using GraphDB.Core;

namespace Argus.Backend.Model.Nodes
{
    public class Task : Node
    {
        static readonly CultureInfo CurCultureInfo = new CultureInfo("en-us");

        //CreateTime
        DateTime myFaultStartTime;
        //AssignTime
        DateTime myFaultEndTime;
        //Interactions
        List<AbstractInteraction> myInteractions;

        public string Type => GetType().Name;

        public string ID => Name;

        [XmlSerializable]
        public string Title { get; }

        [XmlSerializable]
        public string Description { get; }

        [XmlSerializable]
        public int Priority { get; set; }

        [XmlSerializable]
        public string DeviceId { get; }

        [XmlSerializable]
        public string Version { get; }

        [XmlSerializable]
        public string StartTime
        {
            get => myFaultStartTime.ToString(CurCultureInfo);
            private set => myFaultStartTime = Convert.ToDateTime(value, CurCultureInfo);
        }

        [XmlSerializable]
        public string EndTime
        {
            get => myFaultEndTime.ToString(CurCultureInfo);
            private set => myFaultEndTime = Convert.ToDateTime(value, CurCultureInfo);
        }

        public User Submitter => GetSubmitter();

        public User Handler => GetHandler();

        [XmlSerializable]
        [XmlEnumerable]
        public XmlNode SerialInteractions => GetSerialInteractions();

        public IEnumerable<AbstractInteraction> Interactions => myInteractions;

        public Task(string id, string title, string description, FaultInfo faultInfo, int priority = 3) : base(id)
        {
            Title = title;
            Description = description;

            DeviceId = faultInfo.DeviceId;
            Version = faultInfo.Version;
            myFaultStartTime = faultInfo.StartTime;
            myFaultEndTime = faultInfo.EndTime;

            Priority = priority;
            myInteractions = new List<AbstractInteraction>();
        }

        public Task(Node oriNode) : base(oriNode)
        {
            if (oriNode.GetType() != GetType())
            {
                throw new ArgumentException("Node init failed, invalid node:" + oriNode.Name
                                                                              + " type:" + oriNode.GetType().Name + " try to convert as " + Type + ".");
            }

            Task newNode = oriNode as Task;
            if (newNode == null)
            {
                throw new ArgumentNullException("Node init failed, invalid node:" + oriNode.Name);
            }

            Title = newNode.Title;
            Description = newNode.Description;

            DeviceId = newNode.DeviceId;
            Version = newNode.Version;
            StartTime = newNode.StartTime;
            EndTime = newNode.EndTime;

            Priority = newNode.Priority;
            myInteractions = newNode.myInteractions;

        }

        public Task(XmlElement xNode) : base(xNode)
        {
            try
            {
                Title = xNode.GetText("Title");
                Description = xNode.GetText("Description");

                DeviceId = xNode.GetText("DeviceId");
                Version = xNode.GetText("Version");
                StartTime = xNode.GetText("StartTime");
                EndTime = xNode.GetText("EndTime");

                Priority = Convert.ToInt32(xNode.GetText("Priority"));
                myInteractions = new List<AbstractInteraction>();
                XmlNode interactions = xNode.GetNode("Interactions");
                foreach (XmlElement curItem in interactions.ChildNodes)
                {
                    AbstractInteraction newInteraction = SerializableHelper.Deserialize(curItem) as AbstractInteraction;
                    if (newInteraction == null)
                    {
                        continue;
                    }
                    myInteractions.Add( newInteraction );
                }
            }
            catch (Exception)
            {
                throw new DataException(GetType().Name + ":" + Name + "'s data is invalid, please check the DB.");
            }
        }

        public void AddInteraction(AbstractInteraction newInteraction)
        {
            if (newInteraction == null)
            {
                return;
            }
            myInteractions.Add(newInteraction);
        }

        public User GetSubmitter()
        {
            var users = GetEdgesByType(new List<Type> { typeof(CreateBy) }, EdgeDirection.Out);
            if (users == null || !users.Any())
            {
                return new User("", "");
            }
            return users.First().To as User;
        }

        public User GetHandler()
        {
            var users = GetEdgesByType(new List<Type> { typeof(AssignTo) }, EdgeDirection.Out);
            if (users == null || !users.Any())
            {
                return new User("", "");
            }
            return users.First().To as User;
        }

        public XmlNode GetSerialInteractions()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("Interactions");

            foreach (var curItem in myInteractions)
            {
                XmlNode newElement = SerializableHelper.Serialize(doc, curItem, curItem.CurrentStep);
                if (newElement == null)
                {
                    continue;
                }
                root.AppendChild(newElement);
            }

            return root;
        }
    }
}