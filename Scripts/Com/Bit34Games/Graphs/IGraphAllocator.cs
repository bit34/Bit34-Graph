namespace Com.Bit34Games.Graphs
{
    public interface IGraphAllocator<TNode, TEdge>
        where TNode : GraphNode
        where TEdge : GraphConnection
    {
        TNode CreateNode();
        TEdge CreateConnection();
        void  FreeNode(TNode node);
        void  FreeConnection(TEdge edge);
    }
}
