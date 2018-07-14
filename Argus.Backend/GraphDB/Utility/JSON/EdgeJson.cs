using GraphDB.Core;

namespace GraphDB.Utility.JSON
{
    public class EdgeJson
    {
        public string type { get; }

        public string source { get; }

        public string target { get; }

        public EdgeJson(Edge edge)
        {
            source = edge.From.Name;
            target = edge.To.Name;
            type = edge.Attribute;
        }
    }
}