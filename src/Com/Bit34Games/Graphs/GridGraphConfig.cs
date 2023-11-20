namespace Com.Bit34Games.Graphs
{
    public abstract class GridGraphConfig<TNode,TEdge> : GraphConfig<TNode, TEdge>
        where TNode : GridNode<TEdge>
        where TEdge : GridEdge
    {
        //  CONSTRUCTOR
        public GridGraphConfig(int staticEdgeCount) : 
            base(staticEdgeCount)
        {}
    }
}
