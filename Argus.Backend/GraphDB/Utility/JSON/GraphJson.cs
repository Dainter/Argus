using System.Collections.Generic;
using System.Linq;
using GraphDB.Core;

namespace GraphDB.Utility.JSON
{
    public class GraphJson
    {
        public IEnumerable<NodeJson> nodes;
        public IEnumerable<EdgeJson> edges;

        public GraphJson( Graph curGraph )
        {
            nodes = curGraph.Nodes.Select( x => new NodeJson(x.Value) );
            edges = curGraph.Edges.Select( x => new EdgeJson( x ) );
        }
    }
}