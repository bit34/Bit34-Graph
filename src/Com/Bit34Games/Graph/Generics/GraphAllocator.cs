namespace Com.Bit34Games.Graph.Generic
{
    public class GraphAllocator<TNode, TEdge, TNodeData, TEdgeData> : IGraphAllocator<TNode, TEdge>
        where TNode : GraphNode, new()
        where TEdge : GraphEdge, new()
        where TNodeData : new()
        where TEdgeData : new()
    {
        //  CONSTRUCTORS
        public GraphAllocator() { }

        //  METHODS
        public TNode CreateNode() { TNode node = new TNode(); node.InitData(new TNodeData()); return node; }
        public TEdge CreateEdge() { TEdge edge = new TEdge(); edge.InitData(new TEdgeData()); return edge; }
        public void  FreeNode(TNode node) { }
        public void  FreeEdge(TEdge edge) { }
    }
}
