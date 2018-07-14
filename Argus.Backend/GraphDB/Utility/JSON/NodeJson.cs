using GraphDB.Core;

namespace GraphDB.Utility.JSON
{
    public class NodeJson
    {
        public string name { get; }

        public string type { get; }

        public NodeJson(Node node)
        {
            name = node.Name;
            type = node.GetType().Name;
        }
    }
}