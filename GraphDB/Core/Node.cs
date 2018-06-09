using System.Collections.Generic;
using System.Linq;
using System.Xml;
using GraphDB.Contract.Enum;
using GraphDB.Contract.Serial;
using GraphDB.Utility;

namespace GraphDB.Core
{
    public class Node//ͼ���ݿ�ڵ��ࣺ����洢��һ����ڵ����Ϣ�������ϲ����ṩ���ܽӿں���
    {
        //��Ա����
        private readonly string myGuid;                           //�ڵ���
        private readonly string myName;
        private readonly List<Edge> myOutLink;       //���� ʹ���ֵ�ṹ��ţ�Ŀ��ڵ�ţ����߶���
        private readonly List<Edge> myInLink;
        //����///////////////////////////////
        [XmlSerializable]
        public string Guid => myGuid;

        [XmlSerializable]
        public string Name => myName;

        public int InDegree => myInLink.Count;

        public int OutDegree => myOutLink.Count;

        public List<Edge> OutBound => myOutLink;

        public List<Edge> InBound => myInLink;

        //����///////////////////////////////
        //�ڵ���Node���캯��
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
        //xml���캯��
        public Node(XmlElement xNode)
        {
            //ȡ���ƶ���ǩ��Inner Text
            myGuid = xNode.GetText("Guid");
            myName = xNode.GetText("Name");
            myOutLink = new List<Edge>();
            myInLink = new List<Edge>();
        }

        //��������
        public bool AddEdge(Edge newEdge)
        {
            if (newEdge == null)
            {
                return false;
            }
            //�����������ǰ�ߵ���ʼ�ڵ��Ǳ��ڵ㣬����ֹ�ڵ㲻�Ǳ��ڵ�
            if (newEdge.From.Guid != Guid || newEdge.To.Guid == Guid)
            {
                return false;
            }
            //���OutbOund�Ѿ������ñ�
            if (OutBoundContainsEdge(newEdge))
            {
                return false;
            }
            //��Links�м�������Ŀ  
            myOutLink.Add(newEdge);   
            return true;
        }

        //Inbound��ע��
        public bool RegisterInbound(Edge newEdge)
        {
            if (newEdge == null)
            {
                return false;
            }
            //�����������ǰ�ߵ���ʼ�ڵ㲻�Ǳ��ڵ㣬����ֹ�ڵ��Ǳ��ڵ�
            if (newEdge.To.Guid != Guid || newEdge.From.Guid == Guid)
            {
                return false;
            }
            //���Inbound�����ñ���ע��
            if (InBoundContainsEdge(newEdge))
            {
                return false;
            }
            //�����±�
            myInLink.Add(newEdge);
            return true;
        }

        //ȥ������
        public bool RemoveEdge(Edge curEdge)
        {
            if (curEdge == null)
            {
                return false;
            }
            //�����������ǰ�ߵ���ʼ�ڵ��Ǳ��ڵ㣬����ֹ�ڵ㲻�Ǳ��ڵ�
            if (curEdge.From.Guid != Guid || curEdge.To.Guid == Guid)
            {
                return false;
            }
            //���OutbOund�������ñ����˳�
            if (OutBoundContainsEdge(curEdge) == false)
            {
                return false;
            }
            myOutLink.Remove(curEdge);
            return true;
        }

        //�����������,���ر�����ı��б�
        public List<Edge> ClearEdge()
        {
            List<Edge> edgeList = new List<Edge>();
            //���Ƚ�OutBound���������ߵ���ֹ�ڵ���ע���ñ�
            foreach (Edge edge in OutBound)
            {
                edge.To.UnRegisterInbound(edge);
                edge.From = null;
                edge.To = null;
                //��ǰ�߼��뷵�ؽ���б�
                edgeList.Add(edge);
            }
            //��OutBound��������б�
            OutBound.Clear();
            //���Ƚ�InBound���������ߵ���ʼ�ڵ���ȥ���ñ�
            foreach (Edge edge in InBound)
            {
                edge.From.RemoveEdge(edge);
                edge.From = null;
                edge.To = null;
                //��ǰ�߼��뷵�ؽ���б�
                edgeList.Add(edge);
            }
            //��InBound��������б�
            InBound.Clear();
            //���ر��ڵ��漰�������б�
            return edgeList;
        }

        //Inboundע��
        public bool UnRegisterInbound(Edge curEdge)
        {
            if (curEdge == null)
            {
                return false;
            }
            //���Inbound��������ǰ����ע��
            if (InBoundContainsEdge(curEdge) == false)
            {
                return false;
            }
            myInLink.Remove(curEdge);
            return true;
        }

        //����OutBound�Ƿ������Ŀ��ڵ�������
        private bool OutBoundContainsEdge(Edge newEdge)
        {
            if (myOutLink.Contains(newEdge))
            {
                return true;
            }
            return myOutLink.Any( x => (x.To.Guid == newEdge.To.Guid) && (x.Attribute == newEdge.Attribute) );
        }

        //����InBound�Ƿ������Ŀ��ڵ�������
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
            return Guid;
        }

        public string DataOutput()
        {
            string strResult = "";

            strResult +="Name: " + Name;
            strResult += "\nType: " + GetType().Name;

            return strResult;
        }

        //����Ŀ��Ϊָ��GUID������
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

        //����Ŀ��Ϊָ��Name������
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

        //��������Ϊָ��Type������
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
    }
}
