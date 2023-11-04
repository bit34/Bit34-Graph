using System.Collections.Generic;

namespace Com.Bit34Games.Graphs
{
    internal class PathFindingProcess<TNode, TConnection>
        where TNode : GraphNode
        where TConnection : GraphConnection
    {
        //  MEMBERS
        public readonly TNode                               startNode;
        public readonly TNode                               endNode;
        public readonly AgentPathConfig<TNode, TConnection> pathConfig;
        public readonly Agent<TNode, TConnection>           agent;
        public readonly int                                 openListOperationId;
        public readonly int                                 closedListOperationId;
        public readonly LinkedList<TNode>                   openNodeList;

        //  CONSTRUCTORS
        public PathFindingProcess(TNode                               startNode,
                                  TNode                               endNode,
                                  AgentPathConfig<TNode, TConnection> pathConfig,
                                  Agent<TNode, TConnection>           agent,
                                  int                                 openListOperationId,
                                  int                                 closedListOperationId)
        {
            this.startNode             = startNode;
            this.endNode               = endNode;
            this.pathConfig            = pathConfig;
            this.agent                 = agent;
            this.openListOperationId   = openListOperationId;
            this.closedListOperationId = closedListOperationId;
            openNodeList               = new LinkedList<TNode>();

            AddStartNode();
        }

        //  METHODS
        private void AddStartNode()
        {
            //  Add start node to open list
            startNode.operationId        = openListOperationId;
            startNode.operationParam     = 0;
            startNode.selectedConnection = null;
            openNodeList.AddLast(startNode);
        }

        public bool HasSteps()
        {
            return openNodeList.Count > 0;
        }

        public bool EndReached()
        {
            return endNode.operationId == closedListOperationId;
        }

        public void PerformStep()
        {
            //  Remove node and mark as closed
            TNode openNode = (TNode)PickNodeWithLowestOperationParam();
            openNode.operationId = closedListOperationId;

            ////  Stop when end reached
            //if (openNode==endNode)
            //{
            //    break;
            //}

            //  Iterate static connections of node
            if (pathConfig.useStaticConnections)
            {
                for (int i = openNode.StaticConnectionCount - 1; i >= 0; i--)
                {
                    //  Has static connection
                    TConnection connection = (TConnection)openNode.GetStaticConnection(i);
                    if (connection != null)
                    {
                        ProcessConnection(openNode, connection);
                    }
                }
            }

            //  Iterate dynamic connections on node
            if (pathConfig.useDynamicConnections)
            {
                IEnumerator<GraphConnection> connections = openNode.GetDynamicConnectionEnumerator();
                while (connections.MoveNext())
                {
                    GraphConnection connection = connections.Current;
                    ProcessConnection(openNode, connection);
                }
            }
        }

        public TConnection[] BacktrackConnections()
        {
            LinkedList<TConnection> connections = new LinkedList<TConnection>();
            //  Backtrack connections from end to start
            TConnection connection = (TConnection)endNode.selectedConnection;

            do
            {
                connections.AddFirst(connection);
                connection = (TConnection)agent.ownerGraph.GetNode(connection.SourceNodeId).selectedConnection;
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

        private TNode PickNodeWithLowestOperationParam()
        {
            LinkedListNode<TNode> lowest = openNodeList.First;

            LinkedListNode<TNode> current = lowest.Next;
            while (current != null)
            {
                if (current.Value.operationParam < lowest.Value.operationParam)
                {
                    lowest = current;
                }
                current = current.Next;
            }

            TNode node = lowest.Value;
            openNodeList.Remove(lowest);
            return node;
        }

        private void ProcessConnection(TNode openNode, GraphConnection connection)
        {
            if (pathConfig.isConnectionAccessible != null && 
                pathConfig.isConnectionAccessible(connection, agent) == false)
            {
                return;
            }

            //  begin connection process
            TNode targetNode   = agent.ownerGraph.GetNode(connection.TargetNodeId);
            float weightToNode = openNode.operationParam + connection.Weight;

            //  If node is not visited
            if (targetNode.operationId != openListOperationId && targetNode.operationId != closedListOperationId)
            {
                targetNode.operationId        = openListOperationId;
                targetNode.operationParam     = weightToNode;
                targetNode.selectedConnection = connection;
                openNodeList.AddLast(targetNode);
            }
            else if (targetNode.operationId == openListOperationId)
            {
                if (targetNode.operationParam > weightToNode)
                {
                    targetNode.operationParam = weightToNode;
                    targetNode.selectedConnection = connection;
                }
            }
            //  end connection process
        }
    }
}
