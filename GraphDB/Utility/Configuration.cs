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
        private List<Assembly> myAssemblies;
        private DirectoryInfo myDirectoryInfo;

        public Configuration(string path)
        {
            myDirectoryInfo = new DirectoryInfo(path);
            myAssemblies = LoadAssemblies();
        }

        private List<Assembly> LoadAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();
            foreach (FileSystemInfo fileInfo in myDirectoryInfo.GetFileSystemInfos())
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

        public List<Assembly> GetAssemblies()
        {
            return myAssemblies;
        }

    }
}