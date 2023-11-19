namespace Com.Bit34Games.Graphs
{
    public interface IGraphAllocator<TNode, TConnection>
        where TNode : Node<TConnection>
        where TConnection : Connection
    {
        TNode CreateNode();
        TConnection CreateConnection();
        void  FreeNode(TNode node);
        void  FreeConnection(TConnection connection);
    }
}
