namespace Com.Bit34Games.Graphs
{
    public abstract class GridGraph<TNode, TEdge> : Graph<TNode, TEdge>
        where TNode : GridNode<TEdge>
        where TEdge : GridEdge
    {
        //  CONSTRUCTORS
        public GridGraph(IGraphAllocator<TNode, TEdge> allocator, int staticEdgeCount) :
            base(allocator, staticEdgeCount)
        {}


        //  METHODS
        public int GetOppositeEdge(int edge)
        {
            return (edge + (staticEdgeCount / 2)) % staticEdgeCount;
        }

    }
}
