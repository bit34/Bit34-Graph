namespace Com.Bit34Games.Graphs
{
    public interface IAgentOwner<TNode, TEdge>
        where TNode : Node<TEdge>
        where TEdge : Edge
    {
        int NodeRuntimeIndexCounter{ get; }
        //  METHODS
        TNode GetNode(int id);
    }
}
