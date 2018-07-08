using Argus.Backend.Model.Nodes.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary/>
    public class TaskViewModel
    {
        /// <summary/>
        public string ID { get; }
        /// <summary/>
        public string Title { get; }
        /// <summary/>
        public string Description { get; }
        /// <summary/>
        public int Priority { get; }
        /// <summary/>
        public string DeviceId { get; }
        /// <summary/>
        public string Version { get; }
        /// <summary/>
        public string StartTime { get; }
        /// <summary/>
        public string EndTime { get; }
        /// <summary/>
        public IEnumerable<AbstractInteraction> Interactions { get; }

    }
}