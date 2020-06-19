using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
                if (!(fileInfo is FileInfo) || fileInfo.Name.Contains("System") || fileInfo.Name.Contains("Microsoft"))
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