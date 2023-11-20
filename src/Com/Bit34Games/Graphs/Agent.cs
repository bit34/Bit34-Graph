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

        public Path FindPath(int                      startNodeId, 
                             int                      targetNodeId, 
                             PathConfig<TNode, TEdge> pathConfig)
        {
            return FindPath(owner.GetNode(startNodeId), owner.GetNode(targetNodeId), pathConfig);
        }

        public Path FindPath(TNode                    startNode, 
                             TNode                    endNode, 
                             PathConfig<TNode, TEdge> pathConfig)
        {
            PathFindingProcess<TNode, TEdge> process = new PathFindingProcess<TNode, TEdge>(startNode, 
                                                                                            endNode, 
                                                                                            pathConfig, 
                                                                                            this);

            //  Iterate open nodes until end node is reached or no open nodes left in queue
            while (process.HasSteps())
            {
                process.PerformStep();
            }

            //  Is end node reached
            if (process.EndReached())
            {
                return new Path(process.startNode.Id, process.endNode.Id, process.BacktrackEdges());
            }

            //  No valid path
            return null;
        }

    }

}
