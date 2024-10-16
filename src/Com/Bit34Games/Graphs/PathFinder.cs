namespace Com.Bit34Games.Graphs
{
    public abstract class PathFinder<TAgent, TNode, TEdge>
        where TAgent : Agent<TNode, TEdge>
        where TNode : Node<TEdge>
        where TEdge : Edge
    {
        //  MEMBERS
        public readonly bool useStaticEdges;
        public readonly bool useDynamicEdges;
        public readonly bool allowPartialPaths;


        //  CONSTRUCTOR(S)
        public PathFinder(bool useStaticEdges=true, 
                          bool useDynamicEdges=true,
                          bool allowPartialPaths=false)
        {
            this.useStaticEdges    = useStaticEdges;
            this.useDynamicEdges   = useDynamicEdges;
            this.allowPartialPaths = allowPartialPaths;
        }


        //  METHODS
        public Path FindPath(TAgent agent,
                             int    startNodeId, 
                             int    targetNodeId)
        {
            return FindPath(agent, 
                            agent.owner.GetNode(startNodeId), 
                            agent.owner.GetNode(targetNodeId));
        }

        public Path FindPath(TAgent agent,
                             TNode  startNode, 
                             TNode  endNode)
        {
            PathFinderProcess<TAgent, TNode, TEdge> process = new PathFinderProcess<TAgent, TNode, TEdge>(this,
                                                                                                          agent, 
                                                                                                          startNode, 
                                                                                                          endNode);

            //  Iterate open nodes until end node is reached or no open nodes left in queue
            while (process.HasSteps())
            {
                process.PerformStep();
            }

            //  Is end node reached
            if (process.EndReached())
            {
                return new Path(PathTypes.Full, process.startNode.Id, process.endNode.Id, process.GetFullPathEdges());
            }

            //  If allowed return partial path
            if (allowPartialPaths)
            {
                return new Path(PathTypes.Partial, process.startNode.Id, process.endNode.Id, process.GetPartialPathEdges());
            }

            //  No valid path
            return null;
        }

        protected TNode GetNode(TAgent agent, int nodeId)
        {
            return agent.owner.GetNode(nodeId);
        }

        public virtual bool IsEdgeAccessible(TAgent agent, TEdge edge)
        {
            return true;
        }

        abstract public float CalculateHeuristic(TNode node, TNode targetNode);

    }
}
