using System;
using System.Collections.Generic;


namespace Com.Bit34Games.Graphs
{
    public class Graph<TConfig, TNode, TEdge> : IAgentOwner<TNode, TEdge>,
                                                INodeOwner
        where TConfig : GraphConfig<TNode, TEdge>
        where TNode : Node<TEdge>
        where TEdge : Edge
    {
        //	MEMBERS
        public bool             IsFixed { get; protected set; }
        public readonly TConfig Config;
        public int              NodeCount { get { return _nodes.Count; } }
        public int              NodeIdCounter { get; private set; }
        public int              NodeRuntimeIndexCounter { get; private set;}
        //      Private
        private readonly IGraphAllocator<TNode, TEdge> _allocator;
        private readonly Dictionary<int, TNode>        _nodes;
        private LinkedList<int>                        _freeNodeRuntimeIndices;
        private LinkedList<Agent<TNode, TEdge>>        _agents;


        //  CONSTRUCTORS
        public Graph(TConfig config, IGraphAllocator<TNode, TEdge> allocator)
        {
            IsFixed        = false;
            Config         = config;
            _allocator     = allocator;
            _nodes         = new Dictionary<int, TNode>();
            NodeIdCounter  = -1;

            NodeRuntimeIndexCounter = 0;
            _freeNodeRuntimeIndices  = new LinkedList<int>();

            _agents = new LinkedList<Agent<TNode, TEdge>>();
        }


        //	METHODS
#region Node Methods
    
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

            if (_freeNodeRuntimeIndices.Count>0)
            {
                runtimeIndex = _freeNodeRuntimeIndices.Last.Value;
                _freeNodeRuntimeIndices.RemoveLast();
            }
            else
            {
                runtimeIndex = NodeRuntimeIndexCounter++;
            }

            TNode node = _allocator.CreateNode();
            node.AddedToGraph(this, nodeId, runtimeIndex, Config.statiEdgeCount);
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

            if (node.owner != this)
            {
                throw new Exception("Graph Exception:Cannot remove node, it does not belong to this graph");
            }

            //  Remove referencing static edges
            for (int i = 0; i < node.StaticEdgeCount; i++)
            {
                TEdge edge = (TEdge)node.staticEdges[i];
                if (edge != null)
                {
                    RemoveEdge(edge);
                }
            }

            //  Remove referencing dynamic edges
            while (node.DynamicEdgeCount>0)
            {
                TEdge edge = (TEdge)node.GetFirstDynamicEdge();
                RemoveEdge(edge);
            }

            //  Remove from graph
            _freeNodeRuntimeIndices.AddLast(node.RuntimeIndex);
            _nodes.Remove(node.Id);
            node.RemovedFromGraph();

            _allocator.FreeNode(node);
        }

#endregion

#region Edge Methods

        protected TEdge AddEdge(int  sourceNodeId,
                                int  targetNodeId,
                                int  sourceEdgeIndex = -1,
                                int  targetEdgeIndex = -1,
                                bool createOpposite = true)
        {
            return AddEdge(_nodes[sourceNodeId],
                           _nodes[targetNodeId],
                           sourceEdgeIndex,
                           targetEdgeIndex,
                           createOpposite);
        }

        protected TEdge AddEdge(TNode source,
                                TNode target,
                                int   sourceEdgeIndex = -1,
                                int   targetEdgeIndex = -1,
                                bool  createOpposite = true)
        {
            if (source.owner != this || target.owner != this)
            {
                throw new Exception("Graph Exception:Cannot create edge, node(s) does not belong to this graph");
            }

            //	Create edge
            TEdge edge = _allocator.CreateEdge();

            //	Create opposite edge
            TEdge oppositeEdge = null;
            if (createOpposite)
            {
                oppositeEdge = _allocator.CreateEdge();
            }

            //  Set Edges
            edge.Set(source.Id, source.RuntimeIndex, sourceEdgeIndex, 
                     target.Id, target.RuntimeIndex, targetEdgeIndex, 
                     Config.CalculateEdgeWeight(source, target), 
                     oppositeEdge);

            if (sourceEdgeIndex == -1)
            {
                source.AddDynamicEdge(edge);
            }
            else
            {
                if (source.staticEdges[sourceEdgeIndex] != null)
                {
                    RemoveEdge((TEdge)source.staticEdges[sourceEdgeIndex]);
                }
                source.SetStaticEdge(sourceEdgeIndex, edge);
            }

            //  Set opposite edges
            if (createOpposite)
            {
                oppositeEdge.Set(target.Id, target.RuntimeIndex, targetEdgeIndex, 
                                       source.Id, source.RuntimeIndex, sourceEdgeIndex, 
                                       Config.CalculateEdgeWeight(target, source), 
                                       edge);

                if (targetEdgeIndex == -1)
                {
                    target.AddDynamicEdge(oppositeEdge);
                }
                else
                {
                    if (target.staticEdges[targetEdgeIndex] != null)
                    {
                        RemoveEdge((TEdge)target.staticEdges[targetEdgeIndex]);
                    }
                    target.SetStaticEdge(targetEdgeIndex, oppositeEdge);
                }
            }

            return edge;
        }

        protected void RemoveEdge(TEdge edge, bool deleteOpposite = true)
        {
            //  invalidate edges
            if (edge.SourceEdgeIndex == -1)
            {
                _nodes[edge.SourceNodeId].RemoveDynamicEdge(edge);
            }
            else
            {
                _nodes[edge.SourceNodeId].SetStaticEdge(edge.SourceEdgeIndex, null);
            }

            //  Remove edge
            TEdge oppositeEdge = (TEdge)edge.Opposite;
            edge.Reset();
            _allocator.FreeEdge(edge);

            //  Has an opposite
            if (oppositeEdge != null)
            {
                //  Inform edge removal
                oppositeEdge.OppositeRemoved();

                //  If requested remove opposite, too
                if (deleteOpposite == true)
                {
                    RemoveEdge(oppositeEdge);
                }
            }
        }
        
#endregion

#region Agent Methods
    
        public void AddAgent(Agent<TNode, TEdge> agent)
        {
            _agents.AddLast(agent);
            agent.AddedToGraph(this);
        }

        public void RemoveAgent(Agent<TNode, TEdge> agent)
        {
            _agents.Remove(agent);
            agent.RemovedFromGraph();
        }

#endregion
    }
}
