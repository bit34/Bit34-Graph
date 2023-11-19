namespace Com.Bit34Games.Graphs
{
    public interface IAgentOwner<TNode, TConnection>
        where TNode : GraphNode<TConnection>
        where TConnection : GraphConnection
    {
        int NodeRuntimeIndexCounter{ get; }
        //  METHODS
        TNode GetNode(int id);
    }
}
