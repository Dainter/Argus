using GraphDB.Core;

namespace WebApi.Models.JSON
{
    /// <summary/>
    public class NodeJson
    {
        /// <summary/>
        public string name { get; }
        /// <summary/>
        public string type { get; }
        /// <summary/>
        public NodeJson(Node node)
        {
            name = node.Name;
            type = node.GetType().Name;
        }
    }
}