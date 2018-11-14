namespace Minic.Unity.Graph.Grid
{
    public abstract class GridGraph<TNodeData, TNode, TPath> : Graph<TNodeData, TNode, TPath>
        where TNodeData : new()
        where TNode : GridGraphNode<TNodeData>, new()
        where TPath : GridGraphPath<TNodeData, TNode>, new()
    {
        //  MEMBERS
        private readonly GridGraphConfig _GridConfig;


        //  CONSTRUCTORS
        public GridGraph(GridGraphConfig config, IGraphAllocator<TNodeData, TNode> allocator) :
            base(allocator)
        {
            _GridConfig = config;
        }


        //  METHODS
        abstract public TNode GetNodeByLocation(int column, int row);
        abstract public TNode TryGetNodeByLocation(int column, int row);
        public int GetOppositeEdge(int edge)
        {
            return (edge + (_GridConfig.StaticEdgeCount / 2)) % _GridConfig.StaticEdgeCount;
        }

    }
}
