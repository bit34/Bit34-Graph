using System.Collections.Generic;
using UnityEngine;


namespace Bit34.Unity.Graph.Base
{

    public class GraphNode
    {
        //  MEMBERS
        public int       Id { get; private set; }
        public Vector3   Position;
        public int       StaticEdgeCount  { get { return _StaticEdges.Length; } }
        public int       DynamicEdgeCount { get { return _DynamicEdges.Count; } }
        //      Internal
        private object                _Data;
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
            OperationId = 0;
            OperationParam = 0;
            _DynamicEdges = new LinkedList<GraphEdge>();
        }


        //  METHODS
        public TData GetData<TData>()
        {
            return (TData)_Data;
        }

        public GraphEdge GetStaticEdge(int edgeIndex)
        {
            return _StaticEdges[edgeIndex];
        }

        public bool HasStaticEdgeTo(int nodeId)
        {
            for (int i = 0; i < _StaticEdges.Length; i++)
            {
                if (_StaticEdges[i] != null && _StaticEdges[i].TargetNodeId == nodeId)
                {
                    return true;
                }
            }
            return false;
        }

        public GraphEdge GetStaticEdgeTo(int nodeId)
        {
            for (int i = 0; i < _StaticEdges.Length; i++)
            {
                if (_StaticEdges[i] != null && _StaticEdges[i].TargetNodeId == nodeId)
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

        public bool HasDynamicEdgeTo(int nodeId)
        {
            IEnumerator<GraphEdge> edges = _DynamicEdges.GetEnumerator();

            while (edges.MoveNext() == true)
            {
                if (edges.Current.TargetNodeId == nodeId)
                {
                    return true;
                }
            }
            return false;
        }

        public GraphEdge GetDynamicEdgeTo(int nodeId)
        {
            IEnumerator<GraphEdge> edges = _DynamicEdges.GetEnumerator();

            while (edges.MoveNext() == true)
            {
                if (edges.Current.TargetNodeId == nodeId)
                {
                    return edges.Current;
                }
            }
            return null;
        }

        internal void InitData(object data)
        {
            _Data = data;
        }
        
        internal void AddedToGraph(object ownerGraph, int id, int staticEdgeCount)
        {
            OwnerGraph = ownerGraph;
            Id = id;
            _StaticEdges = new GraphEdge[staticEdgeCount];
        }

        internal void RemovedFromGraph()
        {
            OwnerGraph = null;
            Id = -1;
            _StaticEdges = null;
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

        internal GraphEdge GetFirstDynamicEdge()
        {
            return _DynamicEdges.First.Value;
        }

    }
}
