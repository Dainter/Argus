using System;
using System.Data;
using System.Globalization;
using System.Xml;
using GraphDB.Contract.Serial;
using GraphDB.Core;
using GraphDB.Utility;

namespace Argus.Backend.Model.Nodes
{
    public class Task : Node
    {
        static CultureInfo CurCultureInfo = new CultureInfo("en-us");

        //CreateTime
        DateTime myCreateTime;
        //AssignTime
        DateTime myAssignedTime;

        public string Type => GetType().Name;

        [XmlSerializable]
        public string ID { get; }

        [XmlSerializable]
        public string Title { get; }

        [XmlSerializable]
        public string CreateTime
        {
            get => myCreateTime.ToString(CurCultureInfo);
            private set => myCreateTime = Convert.ToDateTime(value, CurCultureInfo);
        }

        [XmlSerializable]
        public string AssignedTime
        {
            get => myAssignedTime.ToString(CurCultureInfo);
            private set => myAssignedTime = Convert.ToDateTime(value, CurCultureInfo);
        }

        [XmlSerializable]
        public int Priority { get; set; }


        public Task(string id, string title, int priority = 3) : base(id)
        {
            ID = id;
            Title = title;
            myCreateTime = DateTime.Now;
            myAssignedTime = DateTime.Now;
            Priority = priority;
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

            ID = newNode.ID;
            Title = newNode.Title;
            CreateTime = newNode.CreateTime;
            AssignedTime = newNode.AssignedTime;
            Priority = newNode.Priority;
        }

        public Task(XmlElement xNode) : base(xNode)
        {
            try
            {
                ID = xNode.GetText("ID");
                Title = xNode.GetText("Title");
                CreateTime = xNode.GetText("CreateTime");
                AssignedTime = xNode.GetText("AssignedTime");
                Priority = Convert.ToInt32(xNode.GetText("Priority"));
            }
            catch (Exception)
            {
                throw new DataException(GetType().Name + ":" + Name + "'s data is invalid, please check the DB.");
            }
        }
    }
}