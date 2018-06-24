
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using GraphDB.Contract.Serial;

namespace GraphDB.IO
{
    public class XMLStrategy:IIoStrategy//XML�ļ���д�㷨
    {
        string myFilePath;

        public string Path
        {
            get => myFilePath;
            set => myFilePath = value;
        }

        public XMLStrategy(string sPath)
        {
            myFilePath = sPath;
        }

        //XMLStrategy�㷨��ȡ����
        public XmlElement ReadFile()
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                var stream = new FileStream(myFilePath, FileMode.Open);
                doc.Load(stream);               //�����ļ�����xml�ĵ�
                stream.Close();
            }
            catch (Exception)
            {
                throw new SerializationException("Failed to load file.");
            }
            //��������
            XmlElement graph =(XmlElement)doc.FirstChild;
            return graph;
        }

        //XMLStrategy�㷨���溯��
        public void SaveFile(XmlDocument doc)
        {
            try
            {
                var stream = new FileStream(myFilePath, FileMode.Create);
                doc.Save(stream);               //����xml�ĵ�����
                stream.Close();
            }
            catch (Exception)
            {
                throw new SerializationException("Failed to save file.");
            }
        }
    }
}
