using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using Argus.Backend;
using Argus.Backend.Model.Nodes;

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
        public IEnumerable<User> GetUsers()
        {
            return DataStorage.GetStorage().GetUsers(Properties.Settings.Default.WorkflowDBName);
        }

        /// <summary/>
        public IEnumerable<UserGroup> GetUserGroups()
        {
            return DataStorage.GetStorage().GetUserGroups(Properties.Settings.Default.WorkflowDBName);
        }

        /// <summary/>
        public IEnumerable<ProcedureStep> GetProcedureSteps()
        {
            
            return DataStorage.GetStorage().GetAll<ProcedureStep>(Properties.Settings.Default.WorkflowDBName) as IEnumerable<ProcedureStep>;
        }


    }
}