using Bit34.Unity.Graph.Base;


namespace Bit34.Unity.Graph.Grid
{
    public abstract class GridGraph<TNode, TEdge> : Graph<TNode, TEdge>
        where TNode : GridGraphNode
        where TEdge : GraphEdge
    {
        //  MEMBERS
        private readonly GridGraphConfig _GridConfig;


        //  CONSTRUCTORS
        public GridGraph(GridGraphConfig config, IGraphAllocator<TNode, TEdge> allocator) :
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
