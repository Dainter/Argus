using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using GraphDB.Contract.Serial;

namespace GraphDB.IO
{
    public class JsonStrategy : IIoStrategy
    {
        public string Path { get; set; }

        public JsonStrategy(string sPath)
        {
            Path = sPath;
        }

        public XmlElement ReadFile()
        {
            throw new System.NotImplementedException();
        }

        public void SaveFile(XmlDocument doc)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            try
            {
                var stream = new FileStream(Path, FileMode.Create);
                StreamWriter sw = new StreamWriter( stream );
                sw.Write(json);
                stream.Close();
            }
            catch (Exception)
            {
                throw new SerializationException("Failed to save file.");
            }
        }
    }
}