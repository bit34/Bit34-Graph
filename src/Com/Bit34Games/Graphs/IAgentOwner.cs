namespace Com.Bit34Games.Graphs
{
    public interface IAgentOwner<TNode, TConnection>
        where TNode : Node<TConnection>
        where TConnection : Connection
    {
        int NodeRuntimeIndexCounter{ get; }
        //  METHODS
        TNode GetNode(int id);
    }
}
