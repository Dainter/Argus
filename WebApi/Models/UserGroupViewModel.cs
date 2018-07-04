using System.Collections.Generic;
using System.Linq;
using Argus.Backend.Model.Nodes;

namespace WebApi.Models
{
    /// <summary/>
    public class UserGroupViewModel
    {
        /// <summary/>
        public string Name { get; }
        /// <summary/>
        public string GroupType { get; }
        /// <summary/>
        public string Description { get; }
        /// <summary/>
        public string Leader { get; }
        /// <summary/>
        public IEnumerable<string> Members { get; }

        /// <summary/>
        public UserGroupViewModel(UserGroup userGroup)
        {
            Name = userGroup.Name;
            GroupType = userGroup.GroupType;
            Description = userGroup.Description;
            Leader = userGroup.Leader?.Name;
            Members = userGroup.Members.Select( x => x.Name);
        }
    }
}