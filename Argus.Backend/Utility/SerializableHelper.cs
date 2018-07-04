using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using GraphDB.Contract.Serial;

namespace Argus.Backend.Utility
{
    internal static class SerializableHelper
    {
        public static XmlElement Serialize(XmlDocument doc, object obj, string name = "Item")
        {
            PropertyInfo[] pInfos = obj.GetType().GetProperties();

            XmlElement xmlNode = doc.CreateElement(name);
            xmlNode.SetAttribute(XmlNames.Class, obj.GetType().FullName);
            foreach (var curItem in pInfos)
            {
                if (!IsSerialableProperty(curItem))
                {
                    continue;
                }

                if (IsEnumerableProperty(curItem))
                {
                    XmlNode newNode = curItem.GetValue(obj) as XmlNode;
                    if (newNode == null)
                    {
                        continue;
                    }
                    xmlNode.AppendChild(doc.ImportNode(newNode, true));
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

        public static object Deserialize(XmlElement xmlNode)
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

        private static bool IsEnumerableProperty(PropertyInfo pInfo)
        {
            if (pInfo.CustomAttributes.Any(x => x.AttributeType.Name == typeof(XmlEnumerableAttribute).Name))
            {
                return true;
            }
            return false;
        }
    }
}
