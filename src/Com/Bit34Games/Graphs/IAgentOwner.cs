namespace Com.Bit34Games.Graphs
{
    public interface IAgentOwner<TNode>
        where TNode : GraphNode
    {
        int NodeRuntimeIndexCounter{ get; }
        //  METHODS
        TNode GetNode(int id);
    }
}
