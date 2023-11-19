namespace Com.Bit34Games.Graphs
{
    public class Agent<TNode, TConnection> : IAgentPathOwner
        where TNode : GraphNode<TConnection>
        where TConnection : GraphConnection
    {
        //  MEMBERS
        internal IAgentOwner<TNode, TConnection> ownerGraph;

        //  METHODS
        internal void AddedToGraph(IAgentOwner<TNode, TConnection> ownerGraph)
        {
            this.ownerGraph = ownerGraph;
        }

        internal void RemovedFromGraph()
        {
            ownerGraph = null;
        }

        public Path FindPath(int                            startNodeId, 
                             int                            targetNodeId, 
                             PathConfig<TNode, TConnection> pathConfig)
        {
            return FindPath(ownerGraph.GetNode(startNodeId), ownerGraph.GetNode(targetNodeId), pathConfig);
        }

        public Path FindPath(TNode                          startNode, 
                             TNode                          endNode, 
                             PathConfig<TNode, TConnection> pathConfig)
        {
            PathFindingProcess<TNode, TConnection> process = new PathFindingProcess<TNode, TConnection>(startNode, 
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
                return new Path(process.startNode.Id, process.endNode.Id, process.BacktrackConnections());
            }

            //  No valid path
            return null;
        }

    }

}
