namespace Com.Bit34Games.Graph.Generic
{
    public abstract class GridGraph<TConfig, TNode, TEdge> : Graph<TConfig, TNode, TEdge>
        where TConfig : GridGraphConfig
        where TNode : GridGraphNode
        where TEdge : GridGraphEdge
    {
        //  CONSTRUCTORS
        public GridGraph(TConfig config, IGraphAllocator<TNode, TEdge> allocator) :
            base(config, allocator)
        {}


        //  METHODS
        public int GetOppositeEdge(int edge)
        {
            return (edge + (Config.staticEdgeCount / 2)) % Config.staticEdgeCount;
        }

    }
}
