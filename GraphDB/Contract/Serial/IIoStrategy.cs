using System.Xml;

namespace GraphDB.Contract.Serial
{
    public interface IIoStrategy//文件读写算法接口
    {
        string Path { get; set; }
        XmlElement ReadFile( );
        void SaveFile(XmlDocument doc);

    }
}
