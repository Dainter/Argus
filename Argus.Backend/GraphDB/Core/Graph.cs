using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using GraphDB.Contract.Enum;
using GraphDB.Contract.Serial;
using GraphDB.IO;
using GraphDB.Utility;

namespace GraphDB.Core
{
    public class Graph
    {
        private readonly Dictionary<string, Node> myNodeList;
        private readonly List<Edge> myEdgeList;
        private readonly IIoStrategy myIohandler;
        private readonly Configuration myConfiguration;

        public string Name { get; private set; }

        public string FilePath { get; private set; }

        public string AssemblyPath { get; private set; }

        public Dictionary<string, Node> Nodes => myNodeList;

        public List<Edge> Edges => myEdgeList;

        public Graph(string name)
        {
            Name = name;
            myNodeList = new Dictionary<string, Node>();
            myEdgeList = new List<Edge>();
        }

        public Graph(string name, string dbPath, string assemblyPath  )
        {
            Name = name;
            FilePath = dbPath;
            AssemblyPath = assemblyPath;
            myNodeList = new Dictionary<string, Node>();
            myEdgeList = new List<Edge>();
            myIohandler = new XMLStrategy(FilePath);
            myConfiguration = new Configuration(AssemblyPath);

            if ( !File.Exists(FilePath) )
            {
                SaveDataBase();
                return;
            }
            GraphInit();
        }

        private void GraphInit()
        {
            XmlElement graph = myIohandler.ReadFile();
            var nodes = graph.GetNode(XmlNames.Nodes);
            var edges = graph.GetNode(XmlNames.Edges);

            //Nodes
            foreach (XmlElement curItem in nodes)
            {
                Node newNode = (Node)SerializableHelper.Deserialize(curItem, myConfiguration);
                if (newNode == null)
                {
                    throw new SerializationException($"Error found during Deserialize. XML:{curItem}");
                }
                myNodeList.Add(newNode.Guid, newNode);
            }
            //Edges
            foreach (XmlElement curItem in edges)
            {
                Edge newEdge = (Edge)SerializableHelper.Deserialize(curItem, myConfiguration);
                if (newEdge == null)
                {
                    throw new SerializationException($"Error found during Deserialize. XML:{curItem}");
                }
                //Add Link
                AddEdgeByGuid(newEdge.FromGuid, newEdge.ToGuid, newEdge);
            }
        }

        public void SaveDataBase()
        {
            XmlDocument doc = ToXML();
            myIohandler.SaveFile(doc);
        }

        public void SaveAsDataBase(string newPath)
        {
            IIoStrategy newIohandler = new XMLStrategy(newPath);
            XmlDocument doc = ToXML();
            newIohandler.SaveFile(doc);
        }

        //将数据保存为XML文件（接口）
        public XmlDocument ToXML()
        {
            //所有网络数据都保存为xml格式
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement(XmlNames.Graph);

            //Nodes
            var nodes = doc.CreateElement(XmlNames.Nodes);
            nodes.SetAttribute(XmlNames.NodeNumber, myNodeList.Count.ToString());
            foreach (KeyValuePair<string, Node> curItem in myNodeList)
            {
                XmlElement newNode = SerializableHelper.Serialize(doc, curItem.Value);
                nodes.AppendChild(newNode);     
            }
            root.AppendChild(nodes);
            //Edges
            var edges = doc.CreateElement(XmlNames.Edges);
            edges.SetAttribute(XmlNames.EdgeNumber, myEdgeList.Count.ToString());
            foreach (Edge curItem in myEdgeList)
            {
                edges.AppendChild(SerializableHelper.Serialize(doc, curItem)); 
            }
            root.AppendChild(edges);

            doc.AppendChild(root);
            return doc;
        }


        #region Query
        //查询函数，返回节点列表中指定名称的节点
        public Node GetNodeByGuid(string nodeGuid)
        {
            if (nodeGuid == null)
            {
                return null;
            }
            //遍历节点列表
            var query = Nodes.Where(x => x.Value.Guid == nodeGuid).Select(x => x.Value);
            if (!query.Any())
            {
                return null;
            }
            return query.First();
        }

        //查询函数，返回节点列表中指定名称的节点
        public Node GetNodeByName(string nodeName)
        {
            if (nodeName == null)
            {
                return null;
            }
            //遍历节点列表
            var query = Nodes.Where(x => x.Value.Name == nodeName).Select(x => x.Value);
            if (!query.Any())
            {
                return null;
            }
            return query.First();
        }

        //查询函数，返回节点列表中指定类型的节点
        public IEnumerable<Node> GetNodesByType(Type type)
        {
            if (type == null)
            {
                return null;
            }
            //遍历节点列表
            return Nodes.Where(x => x.Value.GetType() == type).Select(x => x.Value);
        }

        //查询函数，返回节点列表中指定名称和类型的节点
        public IEnumerable<Node> GetNodesByNameAndType(string nodeName, Type type)
        {
            if (nodeName == null || type == null)
            {
                return null;
            }
            //遍历节点列表
            return Nodes.Where(x => x.Value.Name == nodeName && x.Value.GetType() == type).Select(x => x.Value);
        }

        //查询函数，返回节点中某个GUID的索引位置
        public int IndexOf(string guid)
        {
            int index = 0;
            foreach (var curItem in Nodes)
            {
                if (curItem.Key == guid)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        //查询函数，返回节点中某个Node的索引位置
        public int IndexOf(Node node)
        {
            int index = 0;
            foreach (var curItem in Nodes)
            {
                if (curItem.Key == node.Guid)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        //查询函数，检查节点是否已存在
        public bool ContainsNode(string nodeName)
        {
            if (nodeName == null)
            {
                return false;
            }
            return Nodes.Any(x => x.Value.Name == nodeName);
        }

        //查询函数，返回指定GUID的节点间的连边
        public IEnumerable<Edge> GetEdgesByGuid(string startGuid, string endGuid)
        {
            if (startGuid == null || endGuid == null)
            {
                return null;
            }
            return Edges.Where(x => x.FromGuid == startGuid && x.ToGuid == endGuid);
        }

        //查询函数，返回指定名称的节点间的连边
        public IEnumerable<Edge> GetEdgesByName(string startName, string endName)
        {
            if (startName == null || endName == null)
            {
                return null;
            }
            return Edges.Where(x => x.From.Name == startName && x.To.Name == endName);
        }

        //查询函数，返回Start开始的类型为Type的连边
        public IEnumerable<Edge> GetEdgesByType(string startName, string attribute)
        {
            if (startName == null || attribute == null)
            {
                return null;
            }
            return Edges.Where(x => x.From.Name == startName && x.Attribute == attribute);
        }

        //查询函数，返回两点之间指定Type的连边
        public Edge GetEdgeByType(string startName, string endName, string attribute)
        {
            var query = GetEdgesByName(startName, endName);
            if (!query.Any())
            {
                return null;
            }
            return query.First(x => x.Attribute == attribute);
        }
        #endregion

        #region Interface Node Operations
        //加入节点（接口）
        public void AddNode(Node newNode)
        {
            if (newNode == null)
            {
                throw new ArgumentNullException();
            }
            //检查节点是否已经存在“名称一致”
            if (ContainsNode(newNode))
            {
                throw new ArgumentException("Node named:" + newNode.Name + "is already exists.");
            }

            AddNodeIntoGraph(newNode);
        }

        //删除节点（接口）
        public void RemoveNode(string nodeName)
        {
            if (nodeName == null)
            {
                throw new ArgumentNullException();
            }
            var tarNode = GetNodeByName(nodeName);
            //检查节点是否已经存在“名称一致”
            if (tarNode == null)
            {
                throw new ArgumentException("Node:" + nodeName + " is not exists.");
            }
            RemoveNodeFromGraph(tarNode);
        }

        #endregion

        #region Interface Edge Operation
        //加入连边（接口）
        public void AddEdge(string startName, string endName, Edge newEdge)
        {
            if (startName == null || endName == null || newEdge == null)
            {
                throw new ArgumentNullException();
            }
            //获取起始节点，不存在报错
            var startNode = GetNodeByName(startName);
            if (startNode == null)
            {
                throw new ArgumentException("Node:" + startName + " is not exists.");
            }
            //获取终止节点，不存在报错
            var endNode = GetNodeByName(endName);
            if (endNode == null)
            {
                throw new ArgumentException("Node:" + endName + " is not exists.");
            }
            //查找两点间是否存在相同类型关系，存在报错
            if (ContainsEdge(startNode, endNode, newEdge))
            {
                throw new ArgumentException("Edge between Node:" + startName + " and Node: " + endName + " is not exists.");
            }
            //在两点间加入新边
            AddEdgeIntoGraph(startNode, endNode, newEdge);
        }

        //加入连边（接口）
        public void AddEdge(Node startNode, Node endNode, Edge newEdge)
        {
            if (startNode == null || endNode == null || newEdge == null)
            {
                throw new ArgumentNullException();
            }
            //起始节点不存在报错
            if (!ContainsNode(startNode))
            {
                throw new ArgumentException("Node:" + startNode.Name + " is not exists.");
            }
            //终止节点不存在报错
            if (!ContainsNode(endNode))
            {
                throw new ArgumentException("Node:" + endNode.Name + " is not exists.");
            }
            AddEdgeIntoGraph(startNode, endNode, newEdge);
        }

        //加入连边（接口）
        public void AddEdgeByGuid(string startGuid, string endGuid, Edge newEdge)
        {
            if (startGuid == null || endGuid == null || newEdge == null)
            {
                throw new ArgumentNullException();
            }
            var curNode = myNodeList[startGuid];
            if (curNode == null)
            {
                throw new ArgumentException("Node with GUID:" + startGuid + " is not exists.");
            }
            var tarNode = myNodeList[endGuid];
            if (tarNode == null)
            {
                throw new ArgumentException("Node with GUID:" + endGuid + " is not exists.");
            }
            AddEdgeIntoGraph(curNode, tarNode, newEdge);
        }

        //删除连边（接口）
        public void RemoveEdge(string startName, string endName, string attribute)
        {
            if (startName == null || endName == null || attribute == null)
            {
                throw new ArgumentNullException();
            }
            //获取起始节点，不存在报错
            var startNode = GetNodeByName(startName);
            if (startNode == null)
            {
                throw new ArgumentException("Node:" + startName + " is not exists.");
            }
            //获取终止节点，不存在报错
            var endNode = GetNodeByName(endName);
            if (endNode == null)
            {
                throw new ArgumentException("Node:" + endName + " is not exists.");
            }
            RemoveEdgeFromGraph(startNode, endNode, attribute);
        }

        //移除连边 by Guid（接口）
        public void RemoveEdgeByGuid(string startNodeGuid, string endNodeGuid, string attribute)
        {
            if (startNodeGuid == null || endNodeGuid == null || attribute == null)
            {
                throw new ArgumentNullException();
            }
            var startNode = myNodeList[startNodeGuid];
            if (startNode == null)
            {
                throw new ArgumentException("Node with GUID:" + startNodeGuid + " is not exists.");
            }
            var endNode = myNodeList[endNodeGuid];
            if (endNode == null)
            {
                throw new ArgumentException("Node with GUID:" + endNodeGuid + " is not exists.");
            }
            RemoveEdgeFromGraph(startNode, endNode, attribute);
        }
        #endregion


        #region Private Node Operation
        //检查节点是否已存在
        private bool ContainsNode(Node curNode)
        {
            if (curNode == null)
            {
                return false;
            }
            return Nodes.Any(x => x.Value.Name == curNode.Name);
        }

        //加入节点
        private void AddNodeIntoGraph(Node newNode)
        {
            if (newNode == null)
            {
                return;
            }
            //节点加入节点列表
            myNodeList.Add(newNode.Guid, newNode);
        }

        //删除节点 by Guid
        private void RemoveNodeFromGraph(string guid)
        {
            if (guid == null)
            {
                return;
            }
            Node curNode = myNodeList[guid];
            if (curNode == null)
            {
                throw new ArgumentException("Node with GUID:" + guid + " is not exists.");
            }
            RemoveNodeFromGraph(curNode);
        }

        //删除节点 by Node
        private void RemoveNodeFromGraph(Node curNode)
        {
            if (curNode == null)
            {
                return;
            }
            //清除节点所有连边
            ClearUnusedEdge(curNode.ClearEdge());
            //从节点列表中移除节点
            myNodeList.Remove(curNode.Guid);
        }

        #endregion

        #region Private Edge Operation
        //检查连边是否已存在
        private bool ContainsEdge(Node startNode, Node endNode, Edge curEdge)
        {
            if (startNode == null || endNode == null || curEdge == null)
            {
                return false;
            }
            var query = startNode.GetEdgesByGuid(endNode.Guid, EdgeDirection.Out);
            return query.Any(x => x.Attribute == curEdge.Attribute);
        }

        //加入连边 by Node
        private void AddEdgeIntoGraph(Node curNode, Node tarNode, Edge newEdge)
        {
            if (curNode == null || tarNode == null || newEdge == null)
            {
                return;
            }
            //连边的头指针指向起节点
            newEdge.From = curNode;
            //连边的尾指针指向目标节点
            newEdge.To = tarNode;
            //将新连边加入起始节点的outbound
            if (curNode.AddEdge(newEdge) == false)
            {
                return;
            }
            //将新连边加入目标节点的Inbound
            if (tarNode.RegisterInbound(newEdge) == false)
            {
                return;
            }
            //全部完成后将连边加入网络连边列表
            myEdgeList.Add(newEdge);
        }

        //移除连边 by Node
        private void RemoveEdgeFromGraph(Node curNode, Node tarNode, string attribute)
        {
            if (curNode == null || tarNode == null || attribute == null)
            {
                return;
            }
            //从起始节点的出边中遍历,查找终止节点编号和目标节点编号和类型一致的连边
            Edge curEdge = curNode.OutBound.First(x => x.To.Guid == tarNode.Guid && x.Attribute == attribute);
            if (curEdge == null)
            {//没找到直接返回
                return;
            }
            //起始节点Outbound中移除连边
            curNode.RemoveEdge(curEdge);
            //从终止节点InBound中注销连边
            tarNode.UnRegisterInbound(curEdge);
            //全部完成后，从总连边列表中移除该边
            myEdgeList.Remove(curEdge);
        }

        //删除所有被解除绑定的连边
        private void ClearUnusedEdge(List<Edge> unusedList)
        {
            if (unusedList == null)
            {
                return;
            }
            //将入参列表中所有连边从总连边列表中删除
            foreach (Edge edge in unusedList)
            {
                myEdgeList.Remove(edge);
            }
            //清空入参列表本身内容
            unusedList.Clear();
        }
        #endregion

        //删除所有节点和连边
        public void ClearAll()
        {
            myEdgeList.Clear();
            myNodeList.Clear();
        }

        //删除所有连边
        public void ClearAllEdge()
        {
            myEdgeList.Clear();
        }

    }
}