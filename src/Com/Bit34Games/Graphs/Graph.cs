using System;
using System.Collections.Generic;


namespace Com.Bit34Games.Graphs
{
    public class Graph<TConfig, TNode, TConnection> : IAgentOwner<TNode>
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
        private int                                          _runtimeIndexCounter;
        private LinkedList<int>                              _freeRuntimeIndices;
        private LinkedList<Agent<TNode, TConnection>>   _agent;


        //  CONSTRUCTORS
        public Graph(TConfig config, IGraphAllocator<TNode, TConnection> allocator)
        {
            IsFixed        = false;
            Config         = config;
            _allocator     = allocator;
            _nodes         = new Dictionary<int, TNode>();
            NodeIdCounter  = -1;

            _runtimeIndexCounter = 0;
            _freeRuntimeIndices  = new LinkedList<int>();

            _agent = new LinkedList<Agent<TNode, TConnection>>();
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

            int runtimeIndex;
            if (_freeRuntimeIndices.Count>0)
            {
                runtimeIndex = _freeRuntimeIndices.Last.Value;
                _freeRuntimeIndices.RemoveLast();
            }
            else
            {
                runtimeIndex = _runtimeIndexCounter++;
            }

            TNode node = _allocator.CreateNode();
            node.AddedToGraph(this, nodeId, runtimeIndex, Config.staticConnectionCount);
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
            _freeRuntimeIndices.AddLast(node.RuntimeIndex);
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

        public void AddAgent(Agent<TNode, TConnection> agent)
        {
            _agent.AddLast(agent);
            agent.AddedToGraph(this);
        }

        public void RemoveAgent(Agent<TNode, TConnection> agent)
        {
            _agent.Remove(agent);
            agent.RemovedFromGraph();
        }

    }
}
