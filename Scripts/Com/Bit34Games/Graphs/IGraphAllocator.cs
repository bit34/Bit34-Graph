namespace Com.Bit34Games.Graphs
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
