using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using GraphDB.Contract.Enum;
using GraphDB.Contract.Serial;
using GraphDB.Utility;

namespace GraphDB.Core
{
    public class Node//图数据库节点类：负责存储单一网络节点的信息，并向上层类提供功能接口函数
    {
        //成员变量
        private readonly string myGuid;                           //节点编号
        private readonly string myName;
        private readonly List<Edge> myOutLink;       //连边 使用字典结构存放（目标节点号，连边对象）
        private readonly List<Edge> myInLink;
        //属性///////////////////////////////
        [XmlSerializable]
        public string Guid => myGuid;

        [XmlSerializable]
        public string Name => myName;

        public int InDegree => myInLink.Count;

        public int OutDegree => myOutLink.Count;

        public List<Edge> OutBound => myOutLink;

        public List<Edge> InBound => myInLink;

        //方法///////////////////////////////
        //节点类Node构造函数
        public Node( string name )    
        {
            myGuid = System.Guid.NewGuid().ToString();
            myName = name ?? "";
            myOutLink = new List<Edge>();
            myInLink = new List<Edge>();
        }

        public Node(Node oriNode )
        {
            myGuid = oriNode.Guid;
            myName = oriNode.Name;
            myOutLink = new List<Edge>();
            myInLink = new List<Edge>();
        }
        //xml构造函数
        public Node(XmlElement xNode)
        {
            //取出制定标签的Inner Text
            myGuid = xNode.GetText("Guid");
            myName = xNode.GetText("Name");
            myOutLink = new List<Edge>();
            myInLink = new List<Edge>();
        }

        //增加连边
        public bool AddEdge(Edge newEdge)
        {
            if (newEdge == null)
            {
                return false;
            }
            //检测条件：当前边的起始节点是本节点，且终止节点不是本节点
            if (newEdge.From.Guid != Guid || newEdge.To.Guid == Guid)
            {
                return false;
            }
            //如果OutbOund已经包含该边
            if (OutBoundContainsEdge(newEdge))
            {
                return false;
            }
            //向Links中加入新项目  
            myOutLink.Add(newEdge);   
            return true;
        }

        //Inbound边注册
        public bool RegisterInbound(Edge newEdge)
        {
            if (newEdge == null)
            {
                return false;
            }
            //检测条件：当前边的起始节点不是本节点，且终止节点是本节点
            if (newEdge.To.Guid != Guid || newEdge.From.Guid == Guid)
            {
                return false;
            }
            //如果Inbound包含该边则不注册
            if (InBoundContainsEdge(newEdge))
            {
                return false;
            }
            //加入新边
            myInLink.Add(newEdge);
            return true;
        }

        //去除连边
        public bool RemoveEdge(Edge curEdge)
        {
            if (curEdge == null)
            {
                return false;
            }
            //检测条件：当前边的起始节点是本节点，且终止节点不是本节点
            if (curEdge.From.Guid != Guid || curEdge.To.Guid == Guid)
            {
                return false;
            }
            //如果OutbOund不包含该边则退出
            if (OutBoundContainsEdge(curEdge) == false)
            {
                return false;
            }
            myOutLink.Remove(curEdge);
            return true;
        }

        //清除所有连边,返回被清除的边列表
        public List<Edge> ClearEdge()
        {
            List<Edge> edgeList = new List<Edge>();
            //首先将OutBound中所有连边的终止节点中注销该边
            foreach (Edge edge in OutBound)
            {
                edge.To.UnRegisterInbound(edge);
                edge.From = null;
                edge.To = null;
                //当前边加入返回结果列表
                edgeList.Add(edge);
            }
            //从OutBound中清除所有边
            OutBound.Clear();
            //首先将InBound中所有连边的起始节点中去除该边
            foreach (Edge edge in InBound)
            {
                edge.From.RemoveEdge(edge);
                edge.From = null;
                edge.To = null;
                //当前边加入返回结果列表
                edgeList.Add(edge);
            }
            //从InBound中清除所有边
            InBound.Clear();
            //返回本节点涉及的连边列表
            return edgeList;
        }

        //Inbound注销
        public bool UnRegisterInbound(Edge curEdge)
        {
            if (curEdge == null)
            {
                return false;
            }
            //如果Inbound不包含当前边则不注销
            if (InBoundContainsEdge(curEdge) == false)
            {
                return false;
            }
            myInLink.Remove(curEdge);
            return true;
        }

        //返回OutBound是否包含和目标节点间的连边
        private bool OutBoundContainsEdge(Edge newEdge)
        {
            if (myOutLink.Contains(newEdge))
            {
                return true;
            }
            return myOutLink.Any( x => (x.To.Guid == newEdge.To.Guid) && (x.Attribute == newEdge.Attribute) );
        }

        //返回InBound是否包含和目标节点间的连边
        private bool InBoundContainsEdge(Edge newEdge)
        {
            if (myInLink.Contains(newEdge))
            {
                return true;
            }
            return myInLink.Any( x => (x.From.Guid == newEdge.From.Guid) && (x.Attribute == newEdge.Attribute) );
        }

        public override string ToString()
        {
            return GetType().Name + ": " + Name;
        }

        public string DataOutput()
        {
            string strResult = "";

            strResult +="Name: " + Name;
            strResult += " Type: " + GetType().Name;

            return strResult;
        }

        //查找目标为指定GUID的连边
        public IEnumerable<Edge> GetEdgesByGuid(string nodeGuid, EdgeDirection direction)
        {
            if( nodeGuid == null )
            {
                return new List<Edge>();
            }
            if (direction == EdgeDirection.In)
            {
                return InBound.Where(x => x.From.Guid == nodeGuid);
            }
            if (direction == EdgeDirection.Out)
            {
                return OutBound.Where(x => x.To.Guid == nodeGuid);
            }
            return new List<Edge>();
        }

        //查找目标为指定Name的连边
        public IEnumerable<Edge> GetEdgesByName(string nodeName, EdgeDirection direction)
        {
            if (nodeName == null)
            {
                return new List<Edge>();
            }
            if (direction == EdgeDirection.In)
            {
                return InBound.Where(x => x.From.Name == nodeName);
            }
            if (direction == EdgeDirection.Out)
            {
                return OutBound.Where(x => x.To.Name == nodeName);
            }
            return new List<Edge>();
        }

        //查找类型为指定Type的连边: 字面量
        public IEnumerable<Edge> GetEdgesByType(string attribute, EdgeDirection direction)
        {
            if (attribute == null)
            {
                return new List<Edge>();
            }
            if (direction == EdgeDirection.In)
            {
                return InBound.Where(x => x.Attribute == attribute);
            }
            if (direction == EdgeDirection.Out)
            {
                return OutBound.Where(x => x.Attribute == attribute);
            }
            return new List<Edge>();
        }

        //查找类型为指定Type的连边
        public IEnumerable<Edge> GetEdgesByType(IEnumerable<Type> types, EdgeDirection direction)
        {
            if ( types == null )
            {
                return new List<Edge>();
            }
            if (direction == EdgeDirection.In)
            {
                return InBound.Where(x =>types.Contains(x.GetType()));
            }
            if (direction == EdgeDirection.Out)
            {
                return OutBound.Where(x => types.Contains(x.GetType()));
            }
            return new List<Edge>();
        }
    }
}
