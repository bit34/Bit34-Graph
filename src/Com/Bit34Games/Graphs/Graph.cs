using System;
using System.Collections.Generic;


namespace Com.Bit34Games.Graphs
{
    public class Graph<TConfig, TNode, TConnection>
        where TConfig : GraphConfig<TNode>
        where TNode : GraphNode
        where TConnection : GraphConnection
    {
        //	MEMBERS
        public int  NodeCount     { get { return _nodes.Count; } }
        public int  NodeIdCounter { get; private set; }
        public bool IsFixed       { get; protected set; }
        public readonly TConfig Config;
        //      Private
        private readonly IGraphAllocator<TNode, TConnection> _allocator;
        private readonly Dictionary<int, TNode>              _nodes;
        private int                                          _operationId;


        //  CONSTRUCTORS
        public Graph(TConfig config, IGraphAllocator<TNode, TConnection> allocator)
        {
            IsFixed        = false;
            Config         = config;
            _allocator     = allocator;
            _nodes         = new Dictionary<int, TNode>();
            _operationId   = 0;
            NodeIdCounter  = -1;
        }


        //	METHODS
        public TNode GetNode(int id)
        {
            return _nodes[id];
        }

        public IEnumerator<TNode> GetNodeEnumerator()
        {
            return _nodes.Values.GetEnumerator();
        }

        protected TNode AddNode()
        {
            return AddNode(NodeIdCounter+1);
        }

        protected TNode AddNode(int nodeId)
        {
            if (IsFixed)
            {
                throw new Exception("Graph Exception:Cannot create node on a fixed graph");
            }

            TNode node = _allocator.CreateNode();
            node.AddedToGraph(this, nodeId, Config.staticConnectionCount);
            _nodes.Add(node.Id, node);
            NodeIdCounter = Math.Max(NodeIdCounter, node.Id);

            return node;
        }

        protected void RemoveNode(int nodeId)
        {
            TNode node;
            if (_nodes.TryGetValue(nodeId, out node) == false)
            {
                throw new Exception("Graph Exception:Cannot remove node, node with id:" + nodeId + " does note exist");
            }
            RemoveNode(node);
        }

        protected void RemoveNode(TNode node)
        {
            if (IsFixed)
            {
                throw new Exception("Graph Exception:Cannot remove node from a fixed graph");
            }

            if (node.ownerGraph != this)
            {
                throw new Exception("Graph Exception:Cannot remove node, it does not belong to this graph");
            }

            //  Remove referencing static connections
            for (int i = 0; i < node.StaticConnectionCount; i++)
            {
                TConnection connection = (TConnection)node.GetStaticConnection(i);
                if (connection != null)
                {
                    RemoveConnection(connection);
                }
            }

            //  Remove referencing dynamic connections
            while (node.DynamicConnectionCount>0)
            {
                TConnection connection = (TConnection)node.GetFirstDynamicConnection();
                RemoveConnection(connection);
            }

            //  Remove from graph
            _nodes.Remove(node.Id);
            node.RemovedFromGraph();

            _allocator.FreeNode(node);
        }

        protected TConnection AddConnection(int  sourceNodeId,
                                            int  targetNodeId,
                                            int  sourceConnectionIndex = -1,
                                            int  targetConnectionIndex = -1,
                                            bool createOpposite = true)
        {
            return AddConnection(_nodes[sourceNodeId],
                                 _nodes[targetNodeId],
                                 sourceConnectionIndex,
                                 targetConnectionIndex,
                                 createOpposite);
        }

        protected TConnection AddConnection(TNode source,
                                            TNode target,
                                            int   sourceConnectionIndex = -1,
                                            int   targetConnectionIndex = -1,
                                            bool  createOpposite = true)
        {
            if (source.ownerGraph != this || target.ownerGraph != this)
            {
                throw new Exception("Graph Exception:Cannot create connection, node(s) does not belong to this graph");
            }

            //	Create connection
            TConnection connection = _allocator.CreateConnection();

            //	Create opposite connection
            TConnection oppositeConnection = null;
            if (createOpposite)
            {
                oppositeConnection = _allocator.CreateConnection();
            }

            //  Set connections
            connection.Set(source.Id, sourceConnectionIndex, target.Id, targetConnectionIndex, Config.CalculateConnectionWeight(source, target), oppositeConnection);

            if (sourceConnectionIndex == -1)
            {
                source.AddDynamicConnection(connection);
            }
            else
            {
                if (source.GetStaticConnection(sourceConnectionIndex) != null)
                {
                    RemoveConnection((TConnection)source.GetStaticConnection(sourceConnectionIndex));
                }
                source.SetStaticConnection(sourceConnectionIndex, connection);
            }

            //  Set opposite connections
            if (createOpposite)
            {
                oppositeConnection.Set(target.Id, targetConnectionIndex, source.Id, sourceConnectionIndex, Config.CalculateConnectionWeight(target, source), connection);

                if (targetConnectionIndex == -1)
                {
                    target.AddDynamicConnection(oppositeConnection);
                }
                else
                {
                    if (target.GetStaticConnection(targetConnectionIndex) != null)
                    {
                        RemoveConnection((TConnection)target.GetStaticConnection(targetConnectionIndex));
                    }
                    target.SetStaticConnection(targetConnectionIndex, oppositeConnection);
                }
            }

            return connection;
        }

        protected void RemoveConnection(TConnection connection, bool deleteOpposite = true)
        {
            //  invalidate connections
            if (connection.SourceConnectionIndex == -1)
            {
                _nodes[connection.SourceNodeId].RemoveDynamicConnection(connection);
            }
            else
            {
                _nodes[connection.SourceNodeId].SetStaticConnection(connection.SourceConnectionIndex, null);
            }

            //  Remove connection
            TConnection oppositeConnection = (TConnection)connection.Opposite;
            connection.Reset();
            _allocator.FreeConnection(connection);

            //  Has an opposite
            if (oppositeConnection != null)
            {
                //  Inform connection removal
                oppositeConnection.OppositeRemoved();

                //  If requested remove opposite, too
                if (deleteOpposite == true)
                {
                    RemoveConnection(oppositeConnection);
                }
            }
        }

        public bool FindPath(int startNodeId, int targetNodeId, GraphPathConfig pathConfig, GraphPath path, GraphAgent agent = null)
        {
            return FindPath(_nodes[startNodeId], _nodes[targetNodeId], pathConfig, path, agent);
        }

        public bool FindPath(TNode startNode, TNode endNode, GraphPathConfig pathConfig, GraphPath path, GraphAgent agent = null)
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
                            TNode targetNode   = _nodes[connection.TargetNodeId];
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
                        TNode targetNode   = _nodes[connection.TargetNodeId];
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
                    connection = (TConnection)_nodes[connection.SourceNodeId].selectedConnection;
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
