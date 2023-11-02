namespace Com.Bit34Games.Graphs
{
    public interface IAgentOwner<TNode>
        where TNode : GraphNode
    {
        //  METHODS
        TNode GetNode(int id);
    }
}
