namespace Com.Bit34Games.Graphs
{
    public abstract class GridGraphConfig<TNode> : GraphConfig<TNode>
        where TNode : GridGraphNode
    {
        //  CONSTRUCTOR
        public GridGraphConfig(int staticConnectionCount) : 
            base(staticConnectionCount)
        {}
    }
}
