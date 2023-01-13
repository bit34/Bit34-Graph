namespace Com.Bit34Games.Graph.Generic
{
    public class GraphAllocator<TNode, TEdge> : IGraphAllocator<TNode, TEdge>
        where TNode : GraphNode, new()
        where TEdge : GraphEdge, new()
    {
        //  CONSTRUCTORS
        public GraphAllocator() { }

        //  METHODS
        public TNode CreateNode() { return new TNode(); }
        public TEdge CreateEdge() { return new TEdge(); }
        public void  FreeNode(TNode node) { }
        public void  FreeEdge(TEdge edge) { }
    }
}
