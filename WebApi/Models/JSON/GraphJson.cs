using System.Collections.Generic;
using System.Linq;
using GraphDB.Core;

namespace WebApi.Models.JSON
{
    /// <summary/>
    public class GraphJson
    {
        /// <summary/>
        public IEnumerable<NodeJson> nodes;
        /// <summary/>
        public IEnumerable<EdgeJson> edges;

        /// <summary/>
        public GraphJson( Graph curGraph )
        {
            nodes = curGraph.Nodes.Select( x => new NodeJson(x.Value) );
            edges = curGraph.Edges.Select( x => new WebApi.Models.JSON.EdgeJson( x ) );
        }
    }
}