using System.Xml;

using GraphDB.Core;

namespace GraphDB.Constructor.Semantic.Model
{
    public class Hanzi : Node
    {
        public Hanzi( string name ) : base( name ) {}
        public Hanzi( Node oriNode ) : base( oriNode ) {}
        public Hanzi( XmlElement xNode ) : base( xNode ) {}
    }
}