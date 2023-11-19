namespace Com.Bit34Games.Graphs
{
    public abstract class GridGraphConfig<TNode,TConnection> : GraphConfig<TNode, TConnection>
        where TNode : GridGraphNode<TConnection>
        where TConnection : GridGraphConnection
    {
        //  CONSTRUCTOR
        public GridGraphConfig(int staticConnectionCount) : 
            base(staticConnectionCount)
        {}
    }
}
