namespace Com.Bit34Games.Graphs
{
    public class GraphAllocator<TNode, TEdge> : IGraphAllocator<TNode, TEdge>
        where TNode : Node<TEdge>, new()
        where TEdge : Edge, new()
    {
        //  CONSTRUCTORS
        public GraphAllocator() { }

        //  METHODS
        public TNode CreateNode() { return new TNode(); }
        public TEdge CreateEdge() { return new TEdge(); }
        public void FreeNode(TNode node) { }
        public void FreeEdge(TEdge edge) { }
    }
}
