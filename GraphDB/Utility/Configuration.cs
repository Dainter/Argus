using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using GraphDB.Contract.Serial;
using GraphDB.IO;
using GraphDB.Properties;

namespace GraphDB.Utility
{
    internal class Configuration
    {
        private static Configuration config = new Configuration();
        private static List<Assembly> myAssemblies;

        private Configuration()
        {
            myAssemblies = LoadAssemblies();
        }

        private static List<Assembly> LoadAssemblies()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            string asmName = Path.GetFileName(path);
            List<string> assemList = GetConfiguration("SerialAssemblyList");
            List<Assembly> assemblies = new List<Assembly>();
            foreach (string curItem in assemList)
            {
                assemblies.Add(Assembly.LoadFile(path.Replace(asmName, curItem)));
            }

            return assemblies;
        }

        public static List<Assembly> GetAssemblies()
        {
            return myAssemblies;
        }

        internal static List<string> GetConfiguration(string nodeName)
        {
            IIoStrategy xmlReader = new XMLStrategy(Settings.Default.GraphDBConfigPath);
            XmlElement root;
            try
            {
                root = xmlReader.ReadFile();
            }
            catch (SerializationException)
            {
                return new List<string>();
            }

            List<string> assemList = new List<string>();
            XmlElement setting = root.GetNode(nodeName);
            foreach (XmlNode curItem in setting.ChildNodes)
            {
                assemList.Add(curItem.InnerText);
            }
            return assemList;
        }
    }
}