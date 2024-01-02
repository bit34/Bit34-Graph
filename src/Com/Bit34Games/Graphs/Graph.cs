using System;
using System.Collections.Generic;


namespace Com.Bit34Games.Graphs
{
    public abstract class Graph<TNode, TEdge, TAgent> : IAgentOwner<TNode, TEdge>,
                                                        INodeOwner
        where TNode : Node<TEdge>
        where TEdge : Edge
        where TAgent : Agent<TNode, TEdge>
    {
        //	MEMBERS
        public bool         IsFixed { get; protected set; }
        public int          NodeCount { get { return _nodes.Count; } }
        public int          NodeIdCounter { get; private set; }
        public int          NodeRidCounter { get; private set;}
        public readonly int staticEdgeCount;
        //      Private
        private readonly IGraphAllocator<TNode, TEdge> _allocator;
        private readonly Dictionary<int, TNode>        _nodes;
        private LinkedList<int>                        _freeNodeRids;
        private LinkedList<Agent<TNode, TEdge>>        _agents;


        //  CONSTRUCTORS
        public Graph(IGraphAllocator<TNode, TEdge> allocator, int staticEdgeCount)
        {
            IsFixed        = false;
            _allocator     = allocator;
            _nodes         = new Dictionary<int, TNode>();
            NodeIdCounter  = -1;

            NodeRidCounter      = 0;
            this.staticEdgeCount = staticEdgeCount;
            _freeNodeRids       = new LinkedList<int>();

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

            int rid;

            if (_freeNodeRids.Count>0)
            {
                rid = _freeNodeRids.Last.Value;
                _freeNodeRids.RemoveLast();
            }
            else
            {
                rid = NodeRidCounter++;
            }

            TNode node = _allocator.CreateNode();
            node.AddedToGraph(this, nodeId, rid, staticEdgeCount);
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
            _freeNodeRids.AddLast(node.Rid);
            _nodes.Remove(node.Id);
            node.RemovedFromGraph();

            _allocator.FreeNode(node);
        }

#endregion

#region Edge Methods

        abstract protected float CalculateEdgeWeight(TNode sourceNode, TNode targetNode);

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
            edge.Set(source.Id, source.Rid, sourceEdgeIndex, 
                     target.Id, target.Rid, targetEdgeIndex, 
                     CalculateEdgeWeight(source, target), 
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
                oppositeEdge.Set(target.Id, target.Rid, targetEdgeIndex, 
                                       source.Id, source.Rid, sourceEdgeIndex, 
                                       CalculateEdgeWeight(target, source), 
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
    
        public void AddAgent(TAgent agent)
        {
            _agents.AddLast(agent);
            agent.AddedToGraph(this);
        }

        public void RemoveAgent(TAgent agent)
        {
            _agents.Remove(agent);
            agent.RemovedFromGraph();
        }

#endregion
    }
}
