using System.Collections.Generic;

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

       public bool FindPath(int startNodeId, int targetNodeId, AgentPathConfig<TNode, TConnection> pathConfig, AgentPath path, Agent<TNode, TConnection> agent = null)
       {
           return FindPath(ownerGraph.GetNode(startNodeId), ownerGraph.GetNode(targetNodeId), pathConfig, path, agent);
       }

        public bool FindPath(TNode startNode, TNode endNode, AgentPathConfig<TNode, TConnection> pathConfig, AgentPath path, Agent<TNode, TConnection> agent = null)
        {
            //  New operation id
            int openListOperationId   = ++_operationId;
            int closedListOperationId = ++_operationId;

            //  OpenList
            LinkedList<TNode> openNodeList = new LinkedList<TNode>();

            //  Add start node to open list
            startNode.operationId = openListOperationId;
            startNode.operationParam = 0;
            startNode.selectedConnection = null;
            openNodeList.AddLast(startNode);

            //  Iterate open nodes until end node is reached or no open nodes left in queue
            while (openNodeList.Count > 0)
            {
                //  Remove node and mark as closed
                TNode openNode = (TNode)PickNodeWithLowestOperationParam(openNodeList);
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
                            //  Check connection access restriction
                            if (pathConfig.isConnectionAccessible != null && pathConfig.isConnectionAccessible(connection, agent) == false)
                            {
                                continue;
                            }

                            //  begin connection process
                            TNode targetNode   = ownerGraph.GetNode(connection.TargetNodeId);
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

                //  Iterate dynamic connections on node
                if (pathConfig.useDynamicConnections)
                {
                    IEnumerator<GraphConnection> connections = openNode.GetDynamicConnectionEnumerator();
                    while (connections.MoveNext())
                    {
                        GraphConnection connection = connections.Current;

                        //  Check connection access restriction
                        if (pathConfig.isConnectionAccessible != null && pathConfig.isConnectionAccessible(connection, agent) == false)
                        {
                            continue;
                        }

                        //  begin connection process
                        TNode targetNode   = ownerGraph.GetNode(connection.TargetNodeId);
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

            //  Is end node reached
            if (endNode.operationId == closedListOperationId)
            {
                //  Init path
                path.Init(startNode.Id, endNode.Id);

                //  Backtrack connections from end to start
                TConnection connection = (TConnection)endNode.selectedConnection;

                do
                {
                    path.Connections.AddFirst(connection);
                    connection = (TConnection)ownerGraph.GetNode(connection.SourceNodeId).selectedConnection;
                }
                while (connection != null);

                return true;
            }

            //  No valid path
            return false;
        }

        private TNode PickNodeWithLowestOperationParam(LinkedList<TNode> nodeList)
        {
            LinkedListNode<TNode> lowest = nodeList.First;

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
            nodeList.Remove(lowest);
            return node;
        }
    }
}
