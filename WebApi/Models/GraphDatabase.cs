using System.Collections.Generic;
using System.Web;
using Argus.Backend;

namespace WebApi.Models
{
    /// <summary/>
    public class GraphDatabase
    {

        private static GraphDatabase _myGraphDatabase;

        /// <summary/>
        public static GraphDatabase GetDatabase()
        {
            return _myGraphDatabase ?? (_myGraphDatabase = new GraphDatabase());
        }

        /// <summary/>
        private GraphDatabase()
        {
            string dbPath = HttpContext.Current.Server.MapPath(Properties.Settings.Default.WorkflowDBPath);
            string assemblyPath = HttpContext.Current.Server.MapPath(Properties.Settings.Default.AssemblyPath);
            DataStorage.GetStorage().OpenOrCreate( Properties.Settings.Default.WorkflowDBName, dbPath, assemblyPath);
        }

        /// <summary/>
        public IEnumerable<T> GetAll<T>()
        {
            return DataStorage.GetStorage().GetAll<T>(Properties.Settings.Default.WorkflowDBName);
        }
    }
}