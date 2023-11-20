namespace Com.Bit34Games.Graphs
{
    public interface IAgentOwner<TNode, TEdge>
        where TNode : Node<TEdge>
        where TEdge : Edge
    {
        int NodeRidCounter{ get; }
        //  METHODS
        TNode GetNode(int id);
    }
}
