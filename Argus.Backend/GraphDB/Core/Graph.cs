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

        //�����ݱ���ΪXML�ļ����ӿڣ�
        public XmlDocument ToXML()
        {
            //�����������ݶ�����Ϊxml��ʽ
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
        //��ѯ���������ؽڵ��б���ָ�����ƵĽڵ�
        public Node GetNodeByGuid(string nodeGuid)
        {
            if (nodeGuid == null)
            {
                return null;
            }
            //�����ڵ��б�
            var query = Nodes.Where(x => x.Value.Guid == nodeGuid).Select(x => x.Value);
            if (!query.Any())
            {
                return null;
            }
            return query.First();
        }

        //��ѯ���������ؽڵ��б���ָ�����ƵĽڵ�
        public Node GetNodeByName(string nodeName)
        {
            if (nodeName == null)
            {
                return null;
            }
            //�����ڵ��б�
            var query = Nodes.Where(x => x.Value.Name == nodeName).Select(x => x.Value);
            if (!query.Any())
            {
                return null;
            }
            return query.First();
        }

        //��ѯ���������ؽڵ��б���ָ�����͵Ľڵ�
        public IEnumerable<Node> GetNodesByType(Type type)
        {
            if (type == null)
            {
                return null;
            }
            //�����ڵ��б�
            return Nodes.Where(x => x.Value.GetType() == type).Select(x => x.Value);
        }

        //��ѯ���������ؽڵ��б���ָ�����ƺ����͵Ľڵ�
        public IEnumerable<Node> GetNodesByNameAndType(string nodeName, Type type)
        {
            if (nodeName == null || type == null)
            {
                return null;
            }
            //�����ڵ��б�
            return Nodes.Where(x => x.Value.Name == nodeName && x.Value.GetType() == type).Select(x => x.Value);
        }

        //��ѯ���������ؽڵ���ĳ��GUID������λ��
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

        //��ѯ���������ؽڵ���ĳ��Node������λ��
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

        //��ѯ���������ڵ��Ƿ��Ѵ���
        public bool ContainsNode(string nodeName)
        {
            if (nodeName == null)
            {
                return false;
            }
            return Nodes.Any(x => x.Value.Name == nodeName);
        }

        //��ѯ����������ָ��GUID�Ľڵ�������
        public IEnumerable<Edge> GetEdgesByGuid(string startGuid, string endGuid)
        {
            if (startGuid == null || endGuid == null)
            {
                return null;
            }
            return Edges.Where(x => x.FromGuid == startGuid && x.ToGuid == endGuid);
        }

        //��ѯ����������ָ�����ƵĽڵ�������
        public IEnumerable<Edge> GetEdgesByName(string startName, string endName)
        {
            if (startName == null || endName == null)
            {
                return null;
            }
            return Edges.Where(x => x.From.Name == startName && x.To.Name == endName);
        }

        //��ѯ����������Start��ʼ������ΪType������
        public IEnumerable<Edge> GetEdgesByType(string startName, string attribute)
        {
            if (startName == null || attribute == null)
            {
                return null;
            }
            return Edges.Where(x => x.From.Name == startName && x.Attribute == attribute);
        }

        //��ѯ��������������֮��ָ��Type������
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
        //����ڵ㣨�ӿڣ�
        public void AddNode(Node newNode)
        {
            if (newNode == null)
            {
                throw new ArgumentNullException();
            }
            //���ڵ��Ƿ��Ѿ����ڡ�����һ�¡�
            if (ContainsNode(newNode))
            {
                throw new ArgumentException("Node named:" + newNode.Name + "is already exists.");
            }

            AddNodeIntoGraph(newNode);
        }

        //ɾ���ڵ㣨�ӿڣ�
        public void RemoveNode(string nodeName)
        {
            if (nodeName == null)
            {
                throw new ArgumentNullException();
            }
            var tarNode = GetNodeByName(nodeName);
            //���ڵ��Ƿ��Ѿ����ڡ�����һ�¡�
            if (tarNode == null)
            {
                throw new ArgumentException("Node:" + nodeName + " is not exists.");
            }
            RemoveNodeFromGraph(tarNode);
        }

        #endregion

        #region Interface Edge Operation
        //�������ߣ��ӿڣ�
        public void AddEdge(string startName, string endName, Edge newEdge)
        {
            if (startName == null || endName == null || newEdge == null)
            {
                throw new ArgumentNullException();
            }
            //��ȡ��ʼ�ڵ㣬�����ڱ���
            var startNode = GetNodeByName(startName);
            if (startNode == null)
            {
                throw new ArgumentException("Node:" + startName + " is not exists.");
            }
            //��ȡ��ֹ�ڵ㣬�����ڱ���
            var endNode = GetNodeByName(endName);
            if (endNode == null)
            {
                throw new ArgumentException("Node:" + endName + " is not exists.");
            }
            //����������Ƿ������ͬ���͹�ϵ�����ڱ���
            if (ContainsEdge(startNode, endNode, newEdge))
            {
                throw new ArgumentException("Edge between Node:" + startName + " and Node: " + endName + " is not exists.");
            }
            //�����������±�
            AddEdgeIntoGraph(startNode, endNode, newEdge);
        }

        //�������ߣ��ӿڣ�
        public void AddEdge(Node startNode, Node endNode, Edge newEdge)
        {
            if (startNode == null || endNode == null || newEdge == null)
            {
                throw new ArgumentNullException();
            }
            //��ʼ�ڵ㲻���ڱ���
            if (!ContainsNode(startNode))
            {
                throw new ArgumentException("Node:" + startNode.Name + " is not exists.");
            }
            //��ֹ�ڵ㲻���ڱ���
            if (!ContainsNode(endNode))
            {
                throw new ArgumentException("Node:" + endNode.Name + " is not exists.");
            }
            AddEdgeIntoGraph(startNode, endNode, newEdge);
        }

        //�������ߣ��ӿڣ�
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

        //ɾ�����ߣ��ӿڣ�
        public void RemoveEdge(string startName, string endName, string attribute)
        {
            if (startName == null || endName == null || attribute == null)
            {
                throw new ArgumentNullException();
            }
            //��ȡ��ʼ�ڵ㣬�����ڱ���
            var startNode = GetNodeByName(startName);
            if (startNode == null)
            {
                throw new ArgumentException("Node:" + startName + " is not exists.");
            }
            //��ȡ��ֹ�ڵ㣬�����ڱ���
            var endNode = GetNodeByName(endName);
            if (endNode == null)
            {
                throw new ArgumentException("Node:" + endName + " is not exists.");
            }
            RemoveEdgeFromGraph(startNode, endNode, attribute);
        }

        //�Ƴ����� by Guid���ӿڣ�
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
        //���ڵ��Ƿ��Ѵ���
        private bool ContainsNode(Node curNode)
        {
            if (curNode == null)
            {
                return false;
            }
            return Nodes.Any(x => x.Value.Name == curNode.Name);
        }

        //����ڵ�
        private void AddNodeIntoGraph(Node newNode)
        {
            if (newNode == null)
            {
                return;
            }
            //�ڵ����ڵ��б�
            myNodeList.Add(newNode.Guid, newNode);
        }

        //ɾ���ڵ� by Guid
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

        //ɾ���ڵ� by Node
        private void RemoveNodeFromGraph(Node curNode)
        {
            if (curNode == null)
            {
                return;
            }
            //����ڵ���������
            ClearUnusedEdge(curNode.ClearEdge());
            //�ӽڵ��б����Ƴ��ڵ�
            myNodeList.Remove(curNode.Guid);
        }

        #endregion

        #region Private Edge Operation
        //��������Ƿ��Ѵ���
        private bool ContainsEdge(Node startNode, Node endNode, Edge curEdge)
        {
            if (startNode == null || endNode == null || curEdge == null)
            {
                return false;
            }
            var query = startNode.GetEdgesByGuid(endNode.Guid, EdgeDirection.Out);
            return query.Any(x => x.Attribute == curEdge.Attribute);
        }

        //�������� by Node
        private void AddEdgeIntoGraph(Node curNode, Node tarNode, Edge newEdge)
        {
            if (curNode == null || tarNode == null || newEdge == null)
            {
                return;
            }
            //���ߵ�ͷָ��ָ����ڵ�
            newEdge.From = curNode;
            //���ߵ�βָ��ָ��Ŀ��ڵ�
            newEdge.To = tarNode;
            //�������߼�����ʼ�ڵ��outbound
            if (curNode.AddEdge(newEdge) == false)
            {
                return;
            }
            //�������߼���Ŀ��ڵ��Inbound
            if (tarNode.RegisterInbound(newEdge) == false)
            {
                return;
            }
            //ȫ����ɺ����߼������������б�
            myEdgeList.Add(newEdge);
        }

        //�Ƴ����� by Node
        private void RemoveEdgeFromGraph(Node curNode, Node tarNode, string attribute)
        {
            if (curNode == null || tarNode == null || attribute == null)
            {
                return;
            }
            //����ʼ�ڵ�ĳ����б���,������ֹ�ڵ��ź�Ŀ��ڵ��ź�����һ�µ�����
            Edge curEdge = curNode.OutBound.First(x => x.To.Guid == tarNode.Guid && x.Attribute == attribute);
            if (curEdge == null)
            {//û�ҵ�ֱ�ӷ���
                return;
            }
            //��ʼ�ڵ�Outbound���Ƴ�����
            curNode.RemoveEdge(curEdge);
            //����ֹ�ڵ�InBound��ע������
            tarNode.UnRegisterInbound(curEdge);
            //ȫ����ɺ󣬴��������б����Ƴ��ñ�
            myEdgeList.Remove(curEdge);
        }

        //ɾ�����б�����󶨵�����
        private void ClearUnusedEdge(List<Edge> unusedList)
        {
            if (unusedList == null)
            {
                return;
            }
            //������б����������ߴ��������б���ɾ��
            foreach (Edge edge in unusedList)
            {
                myEdgeList.Remove(edge);
            }
            //�������б�������
            unusedList.Clear();
        }
        #endregion

        //ɾ�����нڵ������
        public void ClearAll()
        {
            myEdgeList.Clear();
            myNodeList.Clear();
        }

        //ɾ����������
        public void ClearAllEdge()
        {
            myEdgeList.Clear();
        }

    }
}