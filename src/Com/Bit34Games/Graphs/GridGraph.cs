namespace Com.Bit34Games.Graphs
{
    public abstract class GridGraph<TConfig, TNode, TConnection> : Graph<TConfig, TNode, TConnection>
        where TConfig : GridGraphConfig<TNode, TConnection>
        where TNode : GridGraphNode<TConnection>
        where TConnection : GridGraphConnection
    {
        //  CONSTRUCTORS
        public GridGraph(TConfig config, IGraphAllocator<TNode, TConnection> allocator) :
            base(config, allocator)
        {}


        //  METHODS
        public int GetOppositeConnection(int connection)
        {
            return (connection + (Config.staticConnectionCount / 2)) % Config.staticConnectionCount;
        }

    }
}
