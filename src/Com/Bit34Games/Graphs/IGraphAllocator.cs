namespace Com.Bit34Games.Graphs
{
    public interface IGraphAllocator<TNode, TConnection>
        where TNode : GraphNode
        where TConnection : GraphConnection
    {
        TNode CreateNode();
        TConnection CreateConnection();
        void  FreeNode(TNode node);
        void  FreeConnection(TConnection connection);
    }
}
