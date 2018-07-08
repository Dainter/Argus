using Argus.Backend.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary/>
    public class TaskInfoViewModel
    {
        /// <summary/>
        public string ID { get; }
        /// <summary/>
        public string Title { get; }
        /// <summary/>
        public int Priority { get; }
        /// <summary/>
        public string Version { get; }
        /// <summary/>
        public string Submitter { get; }
        /// <summary/>
        public string Handler { get; }

        /// <summary/>
        public TaskInfoViewModel(Task curTask)
        {
            ID = curTask.ID;
            Title = curTask.Title;
            Priority = curTask.Priority;
            Version = curTask.Version;
            Submitter = curTask.Submitter.Name;
            Handler = curTask.Handler.Name;
        }
    }
}