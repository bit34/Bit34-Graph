namespace Com.Bit34Games.Graphs
{
    public abstract class GridGraph<TConfig, TNode, TConnection> : Graph<TConfig, TNode, TConnection>
        where TConfig : GridGraphConfig<TNode>
        where TNode : GridGraphNode
        where TConnection : GridGraphConnection
    {
        //  CONSTRUCTORS
        public GridGraph(TConfig config, IGraphAllocator<TNode, TConnection> allocator) :
            base(config, allocator)
        {}


        //  METHODS
        public int GetOppositeConnection(int edge)
        {
            return (edge + (Config.staticConnectionCount / 2)) % Config.staticConnectionCount;
        }

    }
}
