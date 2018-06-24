using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using GraphDB.Contract.Serial;
using GraphDB.Core;


namespace GraphDB.Utility
{
    internal static class SerializableHelper
    {
        public static XmlElement Serialize(XmlDocument doc, object obj)
        {
            PropertyInfo[] pInfos = obj.GetType().GetProperties();

            XmlElement xmlNode = doc.CreateElement(GetBaseType(obj));
            xmlNode.SetAttribute( XmlNames.Class, obj.GetType().FullName);
            foreach (var curItem in pInfos)
            {
                if (!IsSerialableProperty(curItem))
                {
                    continue;
                }
                XmlElement tag = doc.CreateElement(curItem.Name);
                string txt = curItem.GetValue(obj).ToString();
                XmlText value = doc.CreateTextNode(txt);
                tag.AppendChild(value);
                xmlNode.AppendChild(tag);
            }
            return xmlNode;
        }

        public static object Deserialize(XmlElement xmlNode, Configuration config)
        {
            object[] parameters = new object[1];
            parameters[0] = xmlNode;
            string className = xmlNode.GetAttribute(XmlNames.Class);
            Assembly asm = Assembly.GetExecutingAssembly();
            return asm?.CreateInstance(className, true, BindingFlags.Default, null, parameters, null, null);
        }

        private static bool IsSerialableProperty(PropertyInfo pInfo)
        {
            if (pInfo.CustomAttributes.Any(x => x.AttributeType.Name == typeof(XmlSerializableAttribute).Name))
            {
                return true;
            }
            return false;
        }

        private static string GetBaseType(object obj)
        {
            if( obj is Node )
            {
                return XmlNames.Node;
            }
            if (obj is Edge)
            {
                return XmlNames.Edge;
            }
            return "";
        }

        private static Assembly GetAssembly( string typeName, Configuration config )
        {
            var assemList = config.GetAssemblies();
            foreach ( var curItem in assemList)
            {
                if(curItem.ExportedTypes.Any( x => x.FullName == typeName ) )
                {
                    return curItem;
                }
            }
            throw new FileLoadException("No valid assembly has been found.");
        }

    }
}