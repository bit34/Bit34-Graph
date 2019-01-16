using System;
using System.Collections.Generic;


namespace Bit34.Unity.Graph
{
    public class Graph<TNodeData, TNode, TPath>
        where TNodeData : new()
        where TNode : GraphNode<TNodeData>, new()
        where TPath : GraphPath<TNodeData, TNode>, new()
    {
        //	MEMBERS
        public bool IsFixed { get; protected set; }
        private IGraphAllocator<TNodeData, TNode> _Allocator;
        private int                    _NodeIdCounter;
        private LinkedList<TNode>      _NodeList;
        private Dictionary<int, TNode> _NodesById;
        private int                    _OperationId;


        //  CONSTRUCTORS
        public Graph(IGraphAllocator<TNodeData, TNode> allocator)
        {
            IsFixed = false;
            _Allocator     = allocator;
            _NodeIdCounter = 0;
            _NodeList      = new LinkedList<TNode>();
            _NodesById     = new Dictionary<int, TNode>();
            _OperationId   = 0;
        }


        //	METHODS
        public TNode CreateNode(int staticEdgeCount)
        {
            if (IsFixed)
            {
                throw new Exception("Graph Exception:Cannot create node on a fixed graph");
            }

            TNode node = _Allocator.CreateNode();
            node.InitializeNode(this, _NodeIdCounter++, staticEdgeCount);
            _NodeList.AddLast(node);
            _NodesById.Add(node.Id, node);

            return node;
        }

        public void RemoveNode(TNode node)
        {
            if (IsFixed)
            {
                throw new Exception("Graph Exception:Cannot remove node from a fixed graph");
            }

            if (node.OwnerGraph != this)
            {
                throw new Exception("Graph Exception:Cannot remove node,it does not belong to this graph");
            }

            //  Remove referencing static edges
            for (int i = 0; i < node.StaticEdgeCount; i++)
            {
                GraphEdge edge = node.GetStaticEdge(i);
                if (edge != null)
                {
                    DeleteEdge(edge);
                }
            }

            //  Remove referencing dynamic edges
            IEnumerator<GraphEdge> edges = node.GetDynamicEdgeEnumerator();
            while (edges.MoveNext() == true)
            {
                GraphEdge edge = edges.Current;
                DeleteEdge(edge);
            }

            //  Remove from graph
            _NodeList.Remove(node);
            _NodesById.Remove(node.Id);
            node.RemovedFromGraph();

            _Allocator.FreeNode(node);
        }

        public IEnumerator<TNode> GetNodeEnumerator()
        {
            return _NodeList.GetEnumerator();
        }

        public TNode GetNodeById(int id)
        {
            return _NodesById[id];
        }

        public GraphEdge CreateEdge(TNode source, TNode target, int sourceEdgeIndex = -1, int targetEdgeIndex = -1, bool createOpposite = true)
        {
            if (source.OwnerGraph != this && target.OwnerGraph != this)
            {
                throw new Exception("Graph Exception:Cannot create edge, node(s) does not belong to this graph");
            }

            //	Create edge
            GraphEdge edge = _Allocator.CreateEdge();

            //	Create opposite edge
            GraphEdge oppositeEdge = null;
            if (createOpposite)
            {
                oppositeEdge = _Allocator.CreateEdge();
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
                    DeleteEdge(source.GetStaticEdge(sourceEdgeIndex));
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
                        DeleteEdge(target.GetStaticEdge(targetEdgeIndex));
                    }
                    target.SetStaticEdge(targetEdgeIndex, oppositeEdge);
                }
            }

            return edge;
        }

        public void DeleteEdge(GraphEdge edge, bool deleteOpposite = true)
        {
            //  Remove edge connections
            if (edge.SourceEdgeIndex == -1)
            {
                _NodesById[edge.SourceNodeId].RemoveDynamicEdge(edge);
            }
            else
            {
                _NodesById[edge.SourceNodeId].SetStaticEdge(edge.SourceEdgeIndex, null);
            }

            //  Remove edge
            GraphEdge oppositeEdge = edge.OppositeEdge;
            edge.Reset();
            _Allocator.FreeEdge(edge);

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

        public TPath FindPath(TNode startNode, TNode endNode, GraphPathConfig config, GraphAgent agent = null)
        {
            //  New operation id
            int openListOperationId = ++_OperationId;
            int closedListOperationId = ++_OperationId;

            //  OpenList
            LinkedList<GraphNode<TNodeData>> openNodeList = new LinkedList<GraphNode<TNodeData>>();

            //  Add start node to open list
            startNode.OperationId = openListOperationId;
            startNode.OperationParam = 0;
            startNode.SelectedEdge = null;
            openNodeList.AddLast(startNode);

            //  Iterate open nodes until end node is reached or no open nodes left in queue
            while (openNodeList.Count > 0)
            {
                //  Remove node and mark as closed
                GraphNode<TNodeData> openNode = (GraphNode<TNodeData>)PickNodeWithLowestOperationParam(openNodeList);
                openNode.OperationId = closedListOperationId;

                ////  Stop when end reached
                //if (openNode==endNode)
                //{
                //    break;
                //}

                //  Iterate static edges of node
                if (config.UseStaticEdges)
                {
                    for (int i = openNode.StaticEdgeCount - 1; i >= 0; i--)
                    {
                        //  Has static edge
                        GraphEdge edge = openNode.GetStaticEdge(i);
                        if (edge != null)
                        {
                            //  Check edge access restriction
                            if (config.IsEdgeAccessible != null && config.IsEdgeAccessible(edge, agent) == false)
                            {
                                continue;
                            }

                            //  begin edge process
                            GraphNode<TNodeData> targetNode = _NodesById[edge.TargetNodeId];
                            float weightToNode = openNode.OperationParam + edge.Weight;

                            //  If node is not visited
                            if (targetNode.OperationId != openListOperationId && targetNode.OperationId != closedListOperationId)
                            {
                                targetNode.OperationId = openListOperationId;
                                targetNode.OperationParam = weightToNode;
                                targetNode.SelectedEdge = edge;
                                openNodeList.AddLast(targetNode);
                            }
                            else if (targetNode.OperationId == openListOperationId)
                            {
                                if (targetNode.OperationParam > weightToNode)
                                {
                                    targetNode.OperationParam = weightToNode;
                                    targetNode.SelectedEdge = edge;
                                }
                            }
                            //  end edge process
                        }
                    }
                }

                //  Iterate dynamic edges on node
                if (config.UseDynamicEdges)
                {
                    IEnumerator<GraphEdge> edges = openNode.GetDynamicEdgeEnumerator();
                    while (edges.MoveNext())
                    {
                        GraphEdge edge = edges.Current;

                        //  Check edge access restriction
                        if (config.IsEdgeAccessible != null && config.IsEdgeAccessible(edge, agent) == false)
                        {
                            continue;
                        }

                        //  begin edge process
                        GraphNode<TNodeData> targetNode = _NodesById[edge.TargetNodeId];
                        float weightToNode = openNode.OperationParam + edge.Weight;

                        //  If node is not visited
                        if (targetNode.OperationId != openListOperationId && targetNode.OperationId != closedListOperationId)
                        {
                            targetNode.OperationId = openListOperationId;
                            targetNode.OperationParam = weightToNode;
                            targetNode.SelectedEdge = edge;
                            openNodeList.AddLast(targetNode);
                        }
                        else if (targetNode.OperationId == openListOperationId)
                        {
                            if (targetNode.OperationParam > weightToNode)
                            {
                                targetNode.OperationParam = weightToNode;
                                targetNode.SelectedEdge = edge;
                            }
                        }
                        //  end edge process
                    }
                }
            }

            //  Is end node reached
            if (endNode.OperationId == closedListOperationId)
            {
                //  Create path item list
                LinkedList<GraphEdge> pathItems = new LinkedList<GraphEdge>();

                //  Backtrack edges from end to start
                GraphEdge edge = endNode.SelectedEdge;

                do
                {
                    pathItems.AddFirst(edge);
                    edge = _NodesById[edge.SourceNodeId].SelectedEdge;
                }
                while (edge != null);

                //  create path by backtracing from end
                TPath path = new TPath();
                path.Set(startNode, endNode, pathItems);
                return path;
            }

            //  No valid path
            return null;
        }

        virtual protected float CalculateEdgeWeight(GraphNode<TNodeData> sourceNode, GraphNode<TNodeData> targetNode)
        {
            return (targetNode.Position - sourceNode.Position).magnitude;
        }

        private GraphNode<TNodeData> PickNodeWithLowestOperationParam(LinkedList<GraphNode<TNodeData>> nodeList)
        {
            LinkedListNode<GraphNode<TNodeData>> lowest = nodeList.First;

            LinkedListNode<GraphNode<TNodeData>> current = lowest.Next;
            while (current != null)
            {
                if (current.Value.OperationParam < lowest.Value.OperationParam)
                {
                    lowest = current;
                }
                current = current.Next;
            }

            GraphNode<TNodeData> node = lowest.Value;
            nodeList.Remove(lowest);
            return node;
        }

    }
}
