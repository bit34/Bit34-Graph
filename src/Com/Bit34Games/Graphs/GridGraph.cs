namespace Com.Bit34Games.Graphs
{
    public abstract class GridGraph<TConfig, TNode, TEdge> : Graph<TConfig, TNode, TEdge>
        where TConfig : GridGraphConfig<TNode, TEdge>
        where TNode : GridNode<TEdge>
        where TEdge : GridEdge
    {
        //  CONSTRUCTORS
        public GridGraph(TConfig config, IGraphAllocator<TNode, TEdge> allocator) :
            base(config, allocator)
        {}


        //  METHODS
        public int GetOppositeEdge(int edge)
        {
            return (edge + (Config.statiEdgeCount / 2)) % Config.statiEdgeCount;
        }

    }
}
