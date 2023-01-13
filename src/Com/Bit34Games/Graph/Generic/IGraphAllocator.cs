namespace Com.Bit34Games.Graph.Generic
{
    public interface IGraphAllocator<TNode, TEdge>
        where TNode : GraphNode
        where TEdge : GraphEdge
    {
        TNode CreateNode();
        TEdge CreateEdge();
        void  FreeNode(TNode node);
        void  FreeEdge(TEdge edge);
    }
}
