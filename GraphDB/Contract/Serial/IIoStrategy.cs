using System.Xml;

namespace GraphDB.Contract.Serial
{
    public interface IIoStrategy//�ļ���д�㷨�ӿ�
    {
        string Path { get; set; }
        XmlElement ReadFile( );
        void SaveFile(XmlDocument doc);

    }
}
