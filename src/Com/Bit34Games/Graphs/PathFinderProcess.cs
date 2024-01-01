using System.Collections.Generic;

namespace Com.Bit34Games.Graphs
{
    internal class PathFinderProcess<TAgent, TNode, TEdge>
        where TAgent : Agent<TNode, TEdge>
        where TNode : Node<TEdge>
        where TEdge : Edge
    {
        //  MEMBERS
        public readonly PathFinder<TAgent, TNode, TEdge> pathFinder;
        public readonly TAgent                           agent;
        public readonly TNode                            startNode;
        public readonly TNode                            endNode;
        //      Private
        private readonly PathNode[]           _pathNodes;
        private readonly LinkedList<PathNode> _openPathNodeList;

        //  CONSTRUCTORS
        public PathFinderProcess(PathFinder<TAgent, TNode, TEdge> pathFinder,
                                  TAgent                          agent,
                                  TNode                           startNode,
                                  TNode                           endNode)
        {
            this.pathFinder   = pathFinder;
            this.endNode      = endNode;
            this.startNode    = startNode;
            this.agent        = agent;
            _pathNodes        = new PathNode[agent.owner.NodeRidCounter];
            _openPathNodeList = new LinkedList<PathNode>();

            //  Initialize
            _pathNodes[startNode.Rid] = new PathNode(startNode.Id, startNode.Rid);
            _openPathNodeList.AddLast(_pathNodes[startNode.Rid]);
        }

        //  METHODS
        public bool HasSteps()
        {
            return _openPathNodeList.Count > 0;
        }

        public bool EndReached()
        {
            return _pathNodes[endNode.Rid] != null && 
                   _pathNodes[endNode.Rid].isClosed;
        }

        public void PerformStep()
        {
            //  Remove node and mark as closed
            PathNode openPathNode = PickOpenNodeWithLowestWeight();
            TNode    openNode     = agent.owner.GetNode(openPathNode.id);
            openPathNode.isClosed = true;

            //  Iterate static edges of node
            if (pathFinder.useStaticEdges)
            {
                for (int i = openNode.StaticEdgeCount - 1; i >= 0; i--)
                {
                    //  Has static edge
                    TEdge edge = (TEdge)openNode.staticEdges[i];
                    if (edge != null)
                    {
                        ProcessEdge(openPathNode, edge);
                    }
                }
            }

            //  Iterate dynamic edges on node
            if (pathFinder.useDynamicEdges)
            {
                IEnumerator<TEdge> edge = openNode.dynamicEdges.GetEnumerator();
                while (edge.MoveNext())
                {
                    ProcessEdge(openPathNode, edge.Current);
                }
            }
        }

        public TEdge[] BacktrackEdges()
        {
            LinkedList<TEdge> edges = new LinkedList<TEdge>();

            //  Backtrack connections from end to start
            TEdge edge = (TEdge)_pathNodes[endNode.Rid].selectedEdge;

            do
            {
                edges.AddFirst(edge);
                edge = (TEdge)_pathNodes[edge.SourceNodeRid].selectedEdge;
            }
            while (edge != null);

            TEdge[] edgesArray = new TEdge[edges.Count];
            LinkedListNode<TEdge> edgeNode = edges.First;
            int i = 0;
            while(edgeNode != null)
            {
                edgesArray[i++] = edgeNode.Value;
                edgeNode = edgeNode.Next;
            }

            return edgesArray;
        }

        private PathNode PickOpenNodeWithLowestWeight()
        {
            LinkedListNode<PathNode> lowest = _openPathNodeList.First;

            LinkedListNode<PathNode> current = lowest.Next;
            while (current != null)
            {
                if (current.Value.weight < lowest.Value.weight)
                {
                    lowest = current;
                }
                current = current.Next;
            }

            PathNode node = lowest.Value;
            _openPathNodeList.Remove(lowest);
            return node;
        }

        private void ProcessEdge(PathNode openNode, TEdge edge)
        {
            if (pathFinder.CanAgentAccessEdge(agent, edge) == false)
            {
                return;
            }

            PathNode targetPathNode     = _pathNodes[edge.TargetNodeRid];
            float    weightToTargetNode = openNode.weight + edge.Weight;

            //  If node is not visited
            if (targetPathNode == null)
            {
                TNode targetNode            = agent.owner.GetNode(edge.TargetNodeId);
                targetPathNode              = new PathNode(targetNode.Id, targetNode.Rid);
                targetPathNode.weight       = weightToTargetNode;
                targetPathNode.selectedEdge = edge;
                _pathNodes[targetPathNode.rid] = targetPathNode;
                _openPathNodeList.AddLast(targetPathNode);
            }
            else 
            if (targetPathNode.isClosed == false)
            {
                if (targetPathNode.weight > weightToTargetNode)
                {
                    targetPathNode.weight       = weightToTargetNode;
                    targetPathNode.selectedEdge = edge;
                }
            }
        }
    }
}
