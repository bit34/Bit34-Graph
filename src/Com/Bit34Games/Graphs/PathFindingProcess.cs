using System.Collections.Generic;

namespace Com.Bit34Games.Graphs
{
    internal class PathFindingProcess<TNode, TEdge>
        where TNode : Node<TEdge>
        where TEdge : Edge
    {
        //  MEMBERS
        public readonly TNode                    startNode;
        public readonly TNode                    endNode;
        public readonly PathConfig<TNode, TEdge> pathConfig;
        public readonly Agent<TNode, TEdge>      agent;
        //      Private
        private readonly PathNode[]           _pathNodes;
        private readonly LinkedList<PathNode> _openPathNodeList;

        //  CONSTRUCTORS
        public PathFindingProcess(TNode                    startNode,
                                  TNode                    endNode,
                                  PathConfig<TNode, TEdge> pathConfig,
                                  Agent<TNode, TEdge>      agent)
        {
            this.startNode    = startNode;
            this.endNode      = endNode;
            this.pathConfig   = pathConfig;
            this.agent        = agent;
            _pathNodes        = new PathNode[agent.owner.NodeRuntimeIndexCounter];
            _openPathNodeList = new LinkedList<PathNode>();

            Initialize();
        }

        //  METHODS
        private void Initialize()
        {
            _pathNodes[startNode.RuntimeIndex] = new PathNode(startNode.Id, startNode.RuntimeIndex);
            _openPathNodeList.AddLast(_pathNodes[startNode.RuntimeIndex]);
        }

        public bool HasSteps()
        {
            return _openPathNodeList.Count > 0;
        }

        public bool EndReached()
        {
            return _pathNodes[endNode.RuntimeIndex] != null && 
                   _pathNodes[endNode.RuntimeIndex].isClosed;
        }

        public void PerformStep()
        {
            //  Remove node and mark as closed
            PathNode openPathNode = PickOpenNodeWithLowestWeight();
            TNode    openNode     = agent.owner.GetNode(openPathNode.id);
            openPathNode.isClosed = true;

            //  Iterate static edges of node
            if (pathConfig.useStaticEdges)
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
            if (pathConfig.useDynamicEdges)
            {
                IEnumerator<Edge> edge = openNode.dynamicEdges.GetEnumerator();
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
            TEdge edge = (TEdge)_pathNodes[endNode.RuntimeIndex].selectedEdge;

            do
            {
                edges.AddFirst(edge);
                edge = (TEdge)_pathNodes[edge.SourceNodeRuntimeIndex].selectedEdge;
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

        private void ProcessEdge(PathNode openNode, Edge edge)
        {
            if (pathConfig.isEdgeAccessible != null && 
                pathConfig.isEdgeAccessible(edge, agent) == false)
            {
                return;
            }

            PathNode targetPathNode     = _pathNodes[edge.TargetNodeRuntimeIndex];
            float    weightToTargetNode = openNode.weight + edge.Weight;

            //  If node is not visited
            if (targetPathNode == null)
            {
                TNode targetNode            = agent.owner.GetNode(edge.TargetNodeId);
                targetPathNode              = new PathNode(targetNode.Id, targetNode.RuntimeIndex);
                targetPathNode.weight       = weightToTargetNode;
                targetPathNode.selectedEdge = edge;
                _pathNodes[targetPathNode.runtimeIndex] = targetPathNode;
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
