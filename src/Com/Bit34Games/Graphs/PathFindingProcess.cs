using System.Collections.Generic;

namespace Com.Bit34Games.Graphs
{
    internal class PathFindingProcess<TNode, TConnection>
        where TNode : GraphNode<TConnection>
        where TConnection : GraphConnection
    {
        //  MEMBERS
        public readonly TNode                          startNode;
        public readonly TNode                          endNode;
        public readonly PathConfig<TNode, TConnection> pathConfig;
        public readonly Agent<TNode, TConnection>      agent;
        //      Private
        private readonly PathNode[]           _pathNodes;
        private readonly LinkedList<PathNode> _openPathNodeList;

        //  CONSTRUCTORS
        public PathFindingProcess(TNode                          startNode,
                                  TNode                          endNode,
                                  PathConfig<TNode, TConnection> pathConfig,
                                  Agent<TNode, TConnection>      agent)
        {
            this.startNode    = startNode;
            this.endNode      = endNode;
            this.pathConfig   = pathConfig;
            this.agent        = agent;
            _pathNodes        = new PathNode[agent.ownerGraph.NodeRuntimeIndexCounter];
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
            TNode    openNode     = agent.ownerGraph.GetNode(openPathNode.id);
            openPathNode.isClosed = true;

            //  Iterate static connections of node
            if (pathConfig.useStaticConnections)
            {
                for (int i = openNode.StaticConnectionCount - 1; i >= 0; i--)
                {
                    //  Has static connection
                    TConnection connection = (TConnection)openNode.staticConnections[i];
                    if (connection != null)
                    {
                        ProcessConnection(openPathNode, connection);
                    }
                }
            }

            //  Iterate dynamic connections on node
            if (pathConfig.useDynamicConnections)
            {
                IEnumerator<GraphConnection> connections = openNode.dynamicConnections.GetEnumerator();
                while (connections.MoveNext())
                {
                    GraphConnection connection = connections.Current;
                    ProcessConnection(openPathNode, connection);
                }
            }
        }

        public TConnection[] BacktrackConnections()
        {
            LinkedList<TConnection> connections = new LinkedList<TConnection>();

            //  Backtrack connections from end to start
            TConnection connection = (TConnection)_pathNodes[endNode.RuntimeIndex].selectedConnection;

            do
            {
                connections.AddFirst(connection);
                connection = (TConnection)_pathNodes[connection.SourceNodeRuntimeIndex].selectedConnection;
            }
            while (connection != null);

            TConnection[] connectionsArray = new TConnection[connections.Count];
            LinkedListNode<TConnection> connectionNode = connections.First;
            int i = 0;
            while(connectionNode != null)
            {
                connectionsArray[i++] = connectionNode.Value;
                connectionNode = connectionNode.Next;
            }

            return connectionsArray;
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

        private void ProcessConnection(PathNode openNode, GraphConnection connection)
        {
            if (pathConfig.isConnectionAccessible != null && 
                pathConfig.isConnectionAccessible(connection, agent) == false)
            {
                return;
            }

            PathNode targetPathNode     = _pathNodes[connection.TargetNodeRuntimeIndex];
            float    weightToTargetNode = openNode.weight + connection.Weight;

            //  If node is not visited
            if (targetPathNode == null)
            {
                TNode targetNode                   = agent.ownerGraph.GetNode(connection.TargetNodeId);
                targetPathNode                     = new PathNode(targetNode.Id, targetNode.RuntimeIndex);
                targetPathNode.weight              = weightToTargetNode;
                targetPathNode.selectedConnection  = connection;
                _pathNodes[targetPathNode.runtimeIndex] = targetPathNode;
                _openPathNodeList.AddLast(targetPathNode);
            }
            else 
            if (targetPathNode.isClosed == false)
            {
                if (targetPathNode.weight > weightToTargetNode)
                {
                    targetPathNode.weight             = weightToTargetNode;
                    targetPathNode.selectedConnection = connection;
                }
            }
        }
    }
}
