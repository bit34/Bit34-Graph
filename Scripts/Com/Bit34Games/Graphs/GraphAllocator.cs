namespace Com.Bit34Games.Graphs
{
    public class GraphAllocator<TNode, TConnection> : IGraphAllocator<TNode, TConnection>
        where TNode : GraphNode, new()
        where TConnection : GraphConnection, new()
    {
        //  CONSTRUCTORS
        public GraphAllocator() { }

        //  METHODS
        public TNode CreateNode() { return new TNode(); }
        public TConnection CreateConnection() { return new TConnection(); }
        public void FreeNode(TNode node) { }
        public void FreeConnection(TConnection connection) { }
    }
}
