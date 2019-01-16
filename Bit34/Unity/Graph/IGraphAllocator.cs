namespace Bit34.Unity.Graph
{
    public interface IGraphAllocator<TNodeData, TNode>
        where TNodeData : new()
        where TNode : GraphNode<TNodeData>, new()
    {
        TNode CreateNode();
        GraphEdge CreateEdge();
        void FreeNode(TNode node);
        void FreeEdge(GraphEdge edge);
    }
}
