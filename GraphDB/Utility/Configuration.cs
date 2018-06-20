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
            //string path = Settings.Default.ReferenceAssemblyPath;
            string path = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            List<Assembly> assemblies = new List<Assembly>();
            foreach (FileSystemInfo fileInfo in directoryInfo.GetFileSystemInfos())
            {
                if (!(fileInfo is FileInfo))
                {
                    continue;
                }

                if (fileInfo.Extension == ".exe" || fileInfo.Extension == ".dll")
                {
                    Assembly newAssembly = Assembly.LoadFile(fileInfo.FullName);
                    assemblies.Add(newAssembly);
                }
            }

            return assemblies;
        }

        public static List<Assembly> GetAssemblies()
        {
            return myAssemblies;
        }

    }
}