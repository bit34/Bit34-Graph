namespace Com.Bit34Games.Graphs
{
    public interface IGraphAllocator<TNode, TEdge>
        where TNode : Node<TEdge>
        where TEdge : Edge
    {
        TNode CreateNode();
        TEdge CreateEdge();
        void  FreeNode(TNode node);
        void  FreeEdge(TEdge edge);
    }
}
