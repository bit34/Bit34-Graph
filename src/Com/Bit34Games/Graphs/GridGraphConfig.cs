namespace Com.Bit34Games.Graphs
{
    public abstract class GridGraphConfig<TNode,TConnection> : GraphConfig<TNode, TConnection>
        where TNode : GridNode<TConnection>
        where TConnection : GridConnection
    {
        //  CONSTRUCTOR
        public GridGraphConfig(int staticConnectionCount) : 
            base(staticConnectionCount)
        {}
    }
}
