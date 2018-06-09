using System.Xml;
using GraphDB.Contract.Serial;
using GraphDB.Utility;

namespace GraphDB.Core
{
    public class Edge//ͼ���ݿ������ࣺ����洢����������Ϣ
    {
        //��Ա����
        private Node myFromNode;//�������
        private Node myToNode;//�����յ�
        private string myFromGuid;//�������GUID
        private string myToGuid;//�����յ�GUID
        private readonly string myAttribute;//��������
        private string myValue;//����ȡֵ

        //����//////////////////////////
        public Node From
        {
            get => myFromNode;
            set
            {
                myFromNode = value;
                if( value != null )
                {
                    myFromGuid = value.Guid;
                }
                else
                {
                    myFromGuid = "";
                }
            }
        }
        public Node To
        {
            get => myToNode;
            set
            {
                myToNode = value;
                if (value != null)
                {
                    myToGuid = value.Guid;
                }
                else
                {
                    myToGuid = "";
                }
                
            }
        }
        [XmlSerializable]
        public string FromGuid
        {
            get => myFromGuid;
            set => myFromGuid = value;
        }
        [XmlSerializable]
        public string ToGuid
        {
            get => myToGuid;
            set => myToGuid = value;
        }
        [XmlSerializable]
        public string Attribute => myAttribute;
        [XmlSerializable]
        public string Value
        {
            get => myValue;
            set => myValue = value;
        }

        //����/////////////////////////
        //������Edge���캯��
        public Edge(string newAttribute, string newValue = "1")//���캯�� �������������и�ֵ
        {
            myFromGuid = "";
            myToGuid = "";
            myAttribute = newAttribute;
            myValue = newValue;
        }
        //������Edge���캯��
        public Edge( XmlElement xNode)//���캯�� �������������и�ֵ
        {
            //ȡ���ƶ���ǩ��Inner Text
            myFromGuid = xNode.GetText("FromGuid");
            myToGuid = xNode.GetText("ToGuid");
            myAttribute = xNode.GetText("Attribute");
            myValue = xNode.GetText("Value");
        }
    }
}
