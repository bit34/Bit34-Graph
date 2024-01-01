namespace Com.Bit34Games.Graphs
{
    public class Agent<TNode, TEdge> : IPathOwner
        where TNode : Node<TEdge>
        where TEdge : Edge
    {
        //  MEMBERS
        internal IAgentOwner<TNode, TEdge> owner;

        //  METHODS
        internal void AddedToGraph(IAgentOwner<TNode, TEdge> owner)
        {
            this.owner = owner;
        }

        internal void RemovedFromGraph()
        {
            owner = null;
        }

    }

}
