namespace Minic.Unity.Graph.Grid
{
    public class GridGraphDefaultAllocator<TNodeData, TNode> : IGraphAllocator<TNodeData, TNode>
        where TNodeData : new()
        where TNode : GraphNode<TNodeData>, new()
    {
        //  METHODS
        public TNode CreateNode() { return new TNode(); }
        public GraphEdge CreateEdge() { return new GraphEdge(); }
        public void FreeNode(TNode node) { }
        public void FreeEdge(GraphEdge edge) { }
    }
}
