using Argus.Backend.Model.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Models
{
    /// <summary/>
    public class UserViewModel
    {
        /// <summary/>
        public string Name { get; }
        /// <summary/>
        public string Department { get; }
        /// <summary/>
        public string Role { get; }
        /// <summary/>
        public string Email { get; }
        /// <summary/>
        public IEnumerable<string> UserGroups { get; }

        /// <summary/>
        public UserViewModel( User user)
        {
            Name = user.Name;
            Department = user.Department;
            Role = user.Role.Name;
            Email = user.MailBox;
            UserGroups = user.UserGroups.Select( x => x.Name);
        }
    }
}