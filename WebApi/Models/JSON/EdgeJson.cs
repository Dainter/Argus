using GraphDB.Core;

namespace WebApi.Models.JSON
{
    /// <summary/>
    public class EdgeJson
    {
        /// <summary/>
        public string type { get; }
        /// <summary/>
        public string source { get; }
        /// <summary/>
        public string target { get; }
        /// <summary/>
        public EdgeJson(Edge edge)
        {
            source = edge.From.Name;
            target = edge.To.Name;
            type = edge.Attribute;
        }
    }
}