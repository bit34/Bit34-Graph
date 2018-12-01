using UnityEngine;
using System.Collections.Generic;


namespace Minic.Unity.Graph
{

    public class GraphNode<TData>
        where TData : new()
    {
        //  MEMBERS
        public readonly TData Data;
        public int            Id { get; private set; }
        public Vector3        Position;
        public int            StaticEdgeCount { get { return _StaticEdges.Length; } }
        public int            DynamicEdgeCount { get { return _DynamicEdges.Count; } }
        private GraphEdge[]           _StaticEdges;
        private LinkedList<GraphEdge> _DynamicEdges;
        //      Fields for internal operations
        internal object    OwnerGraph;
        internal int       OperationId;
        internal float     OperationParam;
        internal GraphEdge SelectedEdge;


        //  CONSTRUCTORS
        public GraphNode()
        {
            Data = new TData();
            OperationId = 0;
            OperationParam = 0;
        }


        //  METHODS
        public GraphEdge GetStaticEdge(int edgeIndex)
        {
            return _StaticEdges[edgeIndex];
        }

        public bool HasStaticEdgeTo(GraphNode<TData> node)
        {
            for (int i = 0; i < _StaticEdges.Length; i++)
            {
                if (_StaticEdges[i] != null && _StaticEdges[i].TargetNodeId == node.Id)
                {
                    return true;
                }
            }
            return false;
        }

        public GraphEdge GetStaticEdgeTo(GraphNode<TData> node)
        {
            for (int i = 0; i < _StaticEdges.Length; i++)
            {
                if (_StaticEdges[i] != null && _StaticEdges[i].TargetNodeId == node.Id)
                {
                    return _StaticEdges[i];
                }
            }
            return null;
        }

        public IEnumerator<GraphEdge> GetDynamicEdgeEnumerator()
        {
            return _DynamicEdges.GetEnumerator();
        }

        public bool HasDynamicEdgeTo(GraphNode<TData> node)
        {
            IEnumerator<GraphEdge> edges = _DynamicEdges.GetEnumerator();

            while (edges.MoveNext() == true)
            {
                if (edges.Current.TargetNodeId == node.Id)
                {
                    return true;
                }
            }
            return false;
        }

        public GraphEdge GetDynamicEdgeTo(GraphNode<TData> node)
        {
            IEnumerator<GraphEdge> edges = _DynamicEdges.GetEnumerator();

            while (edges.MoveNext() == true)
            {
                if (edges.Current.TargetNodeId == node.Id)
                {
                    return edges.Current;
                }
            }
            return null;
        }

        internal void InitializeNode(object ownerGraph, int id, int staticEdgeCount)
        {
            OwnerGraph = ownerGraph;
            Id = id;

            _StaticEdges = new GraphEdge[staticEdgeCount];
            _DynamicEdges = new LinkedList<GraphEdge>();
        }

        internal void SetStaticEdge(int edgeIndex, GraphEdge edge)
        {
            _StaticEdges[edgeIndex] = edge;
        }

        internal void AddDynamicEdge(GraphEdge edge)
        {
            _DynamicEdges.AddLast(edge);
        }

        internal void RemoveDynamicEdge(GraphEdge edge)
        {
            _DynamicEdges.Remove(edge);
        }

        internal void RemovedFromGraph()
        {
            OwnerGraph = null;
        }
    }
}
