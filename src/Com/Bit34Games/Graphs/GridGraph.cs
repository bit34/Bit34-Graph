namespace Com.Bit34Games.Graphs
{
    public abstract class GridGraph<TNode, TEdge, TAgent> : Graph<TNode, TEdge, TAgent>
        where TNode : GridNode<TEdge>
        where TEdge : GridEdge
        where TAgent : Agent<TNode, TEdge>
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
