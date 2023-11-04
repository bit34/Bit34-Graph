using System.Collections.Generic;
using System.Linq;

namespace Com.Bit34Games.Graphs
{
    public class Agent<TNode, TConnection> : IAgentPathOwner
        where TNode : GraphNode
        where TConnection : GraphConnection
    {
        //  MEMBERS
        internal IAgentOwner<TNode> ownerGraph;
        private int                 _operationId;

        //  CONSTRUCTORS
        public Agent()
        {
            _operationId = 0;
        }

        //  METHODS
        internal void AddedToGraph(IAgentOwner<TNode> ownerGraph)
        {
            this.ownerGraph = ownerGraph;
        }

        internal void RemovedFromGraph()
        {
            ownerGraph = null;
        }

        public AgentPath FindPath(int                                 startNodeId, 
                                  int                                 targetNodeId, 
                                  AgentPathConfig<TNode, TConnection> pathConfig)
        {
            return FindPath(ownerGraph.GetNode(startNodeId), ownerGraph.GetNode(targetNodeId), pathConfig);
        }

        public AgentPath FindPath(TNode                               startNode, 
                                  TNode                               endNode, 
                                  AgentPathConfig<TNode, TConnection> pathConfig)
        {
            PathFindingProcess<TNode, TConnection> process = new PathFindingProcess<TNode, TConnection>(startNode, 
                                                                                                        endNode, 
                                                                                                        pathConfig, 
                                                                                                        this, 
                                                                                                        ++_operationId, 
                                                                                                        ++_operationId);

            //  Iterate open nodes until end node is reached or no open nodes left in queue
            while (process.HasSteps())
            {
                process.PerformStep();
            }

            //  Is end node reached
            if (process.EndReached())
            {
                return new AgentPath(process.startNode.Id, process.endNode.Id, process.BacktrackConnections());
            }

            //  No valid path
            return null;
        }

    }

}
