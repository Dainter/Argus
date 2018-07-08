using Argus.Backend.Model.Nodes;

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
        public string Password { get; }

        /// <summary/>
        public UserViewModel( User user)
        {
            Name = user.Name;
            Department = user.Department;
            Role = user.Role.Name;
            Email = user.MailBox;
            Password = user.Password;
        }
    }
}