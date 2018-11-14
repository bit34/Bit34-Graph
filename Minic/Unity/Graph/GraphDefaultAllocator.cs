namespace Minic.Unity.Graph
{
    public class GraphDefaultAllocator<TNodeData, TNode> : IGraphAllocator<TNodeData, TNode>
        where TNodeData : new()
        where TNode : GraphNode<TNodeData>, new()
    {
        //  CONSTRUCTORS
        public GraphDefaultAllocator() { }

        //  METHODS
        public TNode CreateNode() { return new TNode(); }
        public GraphEdge CreateEdge() { return new GraphEdge(); }
        public void FreeNode(TNode node) { }
        public void FreeEdge(GraphEdge edge) { }
    }
}
