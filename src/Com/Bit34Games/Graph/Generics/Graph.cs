using System;
using System.Collections.Generic;


namespace Com.Bit34Games.Graph.Generic
{
    public class Graph<TNode, TEdge>
        where TNode : GraphNode
        where TEdge : GraphEdge
    {
        //	MEMBERS
        public bool IsFixed { get; protected set; }
        //      Internal
        private readonly GraphConfig          _config;
        private IGraphAllocator<TNode, TEdge> _allocator;
        private int                           _nodeIdCounter;
        private Dictionary<int, TNode>        _nodes;
        private int                           _operationId;


        //  CONSTRUCTORS
        public Graph(GraphConfig config, IGraphAllocator<TNode, TEdge> allocator)
        {
            IsFixed        = false;
            _config        = config;
            _allocator     = allocator;
            _nodeIdCounter = 0;
            _nodes         = new Dictionary<int, TNode>();
            _operationId   = 0;
        }


        //	METHODS
        public TNode CreateNode()
        {
            if (IsFixed)
            {
                throw new Exception("Graph Exception:Cannot create node on a fixed graph");
            }

            TNode node = _allocator.CreateNode();
            node.AddedToGraph(this, _nodeIdCounter++, _config.staticEdgeCount);
            _nodes.Add(node.Id, node);

            return node;
        }

        public void RemoveNode(int nodeId)
        {
            if (IsFixed)
            {
                throw new Exception("Graph Exception:Cannot remove node from a fixed graph");
            }

            TNode node;
            if (_nodes.TryGetValue(nodeId, out node) == false)
            {
                throw new Exception("Graph Exception:Cannot remove node, node with id:" + nodeId + " does note exist");
            }

            if (node.ownerGraph != this)
            {
                throw new Exception("Graph Exception:Cannot remove node, it does not belong to this graph");
            }

            //  Remove referencing static edges
            for (int i = 0; i < node.StaticEdgeCount; i++)
            {
                TEdge edge = (TEdge)node.GetStaticEdge(i);
                if (edge != null)
                {
                    DeleteEdge(edge);
                }
            }

            //  Remove referencing dynamic edges
            while (node.DynamicEdgeCount>0)
            {
                TEdge edge = (TEdge)node.GetFirstDynamicEdge();
                DeleteEdge(edge);
            }

            //  Remove from graph
            _nodes.Remove(node.Id);
            node.RemovedFromGraph();

            _allocator.FreeNode(node);
        }

        public IEnumerator<TNode> GetNodeEnumerator()
        {
            return _nodes.Values.GetEnumerator();
        }

        public TNode GetNode(int id)
        {
            return _nodes[id];
        }

        public TEdge CreateEdge(int  sourceNodeId,
                                int  targetNodeId,
                                int  sourceEdgeIndex = -1,
                                int  targetEdgeIndex = -1,
                                bool createOpposite = true)
        {
            return CreateEdge(_nodes[sourceNodeId],
                              _nodes[targetNodeId],
                              sourceEdgeIndex,
                              targetEdgeIndex,
                              createOpposite);
        }
        
        public TEdge CreateEdge(TNode source,
                                TNode target,
                                int   sourceEdgeIndex = -1,
                                int   targetEdgeIndex = -1,
                                bool  createOpposite = true)
        {
            if (source.ownerGraph != this || target.ownerGraph != this)
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

            //  Set edge connections
            edge.Set(source.Id, sourceEdgeIndex, target.Id, targetEdgeIndex, CalculateEdgeWeight(source, target), oppositeEdge);

            if (sourceEdgeIndex == -1)
            {
                source.AddDynamicEdge(edge);
            }
            else
            {
                if (source.GetStaticEdge(sourceEdgeIndex) != null)
                {
                    DeleteEdge((TEdge)source.GetStaticEdge(sourceEdgeIndex));
                }
                source.SetStaticEdge(sourceEdgeIndex, edge);
            }

            //  Set opposite edge connections
            if (createOpposite)
            {
                oppositeEdge.Set(target.Id, targetEdgeIndex, source.Id, sourceEdgeIndex, CalculateEdgeWeight(target, source), edge);

                if (targetEdgeIndex == -1)
                {
                    target.AddDynamicEdge(oppositeEdge);
                }
                else
                {
                    if (target.GetStaticEdge(targetEdgeIndex) != null)
                    {
                        DeleteEdge((TEdge)target.GetStaticEdge(targetEdgeIndex));
                    }
                    target.SetStaticEdge(targetEdgeIndex, oppositeEdge);
                }
            }

            return edge;
        }

        public void DeleteEdge(TEdge edge, bool deleteOpposite = true)
        {
            //  Remove edge connections
            if (edge.SourceEdgeIndex == -1)
            {
                _nodes[edge.SourceNodeId].RemoveDynamicEdge(edge);
            }
            else
            {
                _nodes[edge.SourceNodeId].SetStaticEdge(edge.SourceEdgeIndex, null);
            }

            //  Remove edge
            TEdge oppositeEdge = (TEdge)edge.OppositeEdge;
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
                    DeleteEdge(oppositeEdge);
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
            int openListOperationId = ++_operationId;
            int closedListOperationId = ++_operationId;

            //  OpenList
            LinkedList<TNode> openNodeList = new LinkedList<TNode>();

            //  Add start node to open list
            startNode.operationId = openListOperationId;
            startNode.operationParam = 0;
            startNode.selectedEdge = null;
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

                //  Iterate static edges of node
                if (pathConfig.useStaticEdges)
                {
                    for (int i = openNode.StaticEdgeCount - 1; i >= 0; i--)
                    {
                        //  Has static edge
                        TEdge edge = (TEdge)openNode.GetStaticEdge(i);
                        if (edge != null)
                        {
                            //  Check edge access restriction
                            if (pathConfig.isEdgeAccessible != null && pathConfig.isEdgeAccessible(edge, agent) == false)
                            {
                                continue;
                            }

                            //  begin edge process
                            TNode targetNode = _nodes[edge.TargetNodeId];
                            float weightToNode = openNode.operationParam + edge.Weight;

                            //  If node is not visited
                            if (targetNode.operationId != openListOperationId && targetNode.operationId != closedListOperationId)
                            {
                                targetNode.operationId = openListOperationId;
                                targetNode.operationParam = weightToNode;
                                targetNode.selectedEdge = edge;
                                openNodeList.AddLast(targetNode);
                            }
                            else if (targetNode.operationId == openListOperationId)
                            {
                                if (targetNode.operationParam > weightToNode)
                                {
                                    targetNode.operationParam = weightToNode;
                                    targetNode.selectedEdge = edge;
                                }
                            }
                            //  end edge process
                        }
                    }
                }

                //  Iterate dynamic edges on node
                if (pathConfig.useDynamicEdges)
                {
                    IEnumerator<GraphEdge> edges = openNode.GetDynamicEdgeEnumerator();
                    while (edges.MoveNext())
                    {
                        GraphEdge edge = edges.Current;

                        //  Check edge access restriction
                        if (pathConfig.isEdgeAccessible != null && pathConfig.isEdgeAccessible(edge, agent) == false)
                        {
                            continue;
                        }

                        //  begin edge process
                        TNode targetNode = _nodes[edge.TargetNodeId];
                        float weightToNode = openNode.operationParam + edge.Weight;

                        //  If node is not visited
                        if (targetNode.operationId != openListOperationId && targetNode.operationId != closedListOperationId)
                        {
                            targetNode.operationId = openListOperationId;
                            targetNode.operationParam = weightToNode;
                            targetNode.selectedEdge = edge;
                            openNodeList.AddLast(targetNode);
                        }
                        else if (targetNode.operationId == openListOperationId)
                        {
                            if (targetNode.operationParam > weightToNode)
                            {
                                targetNode.operationParam = weightToNode;
                                targetNode.selectedEdge = edge;
                            }
                        }
                        //  end edge process
                    }
                }
            }

            //  Is end node reached
            if (endNode.operationId == closedListOperationId)
            {
                //  Init path
                path.Init(startNode.Id, endNode.Id);

                //  Backtrack edges from end to start
                TEdge edge = (TEdge)endNode.selectedEdge;

                do
                {
                    path.Edges.AddFirst(edge);
                    edge = (TEdge)_nodes[edge.SourceNodeId].selectedEdge;
                }
                while (edge != null);

                return true;
            }

            //  No valid path
            return false;
        }

        virtual protected float CalculateEdgeWeight(TNode sourceNode, TNode targetNode)
        {
            return (targetNode.position - sourceNode.position).magnitude;
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
