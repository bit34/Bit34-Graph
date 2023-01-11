using System.Collections.Generic;
using UnityEngine;


namespace Com.Bit34Games.Graph.Generic
{

    public class GraphNode
    {
        //  MEMBERS
        public int       Id { get; private set; }
        public Vector3   position;
        public int       StaticEdgeCount  { get { return _staticEdges.Length; } }
        public int       DynamicEdgeCount { get { return _dynamicEdges.Count; } }
        //      Internal
        private GraphEdge[]           _staticEdges;
        private LinkedList<GraphEdge> _dynamicEdges;
        //      Fields for internal operations
        internal object    ownerGraph;
        internal int       operationId;
        internal float     operationParam;
        internal GraphEdge selectedEdge;


        //  CONSTRUCTORS
        public GraphNode()
        {
            operationId    = 0;
            operationParam = 0;
            _dynamicEdges  = new LinkedList<GraphEdge>();
        }


        //  METHODS
        public GraphEdge GetStaticEdge(int edgeIndex)
        {
            return _staticEdges[edgeIndex];
        }

        public bool HasStaticEdgeTo(int nodeId)
        {
            for (int i = 0; i < _staticEdges.Length; i++)
            {
                if (_staticEdges[i] != null && _staticEdges[i].TargetNodeId == nodeId)
                {
                    return true;
                }
            }
            return false;
        }

        public GraphEdge GetStaticEdgeTo(int nodeId)
        {
            for (int i = 0; i < _staticEdges.Length; i++)
            {
                if (_staticEdges[i] != null && _staticEdges[i].TargetNodeId == nodeId)
                {
                    return _staticEdges[i];
                }
            }
            return null;
        }

        public IEnumerator<GraphEdge> GetDynamicEdgeEnumerator()
        {
            return _dynamicEdges.GetEnumerator();
        }

        public bool HasDynamicEdgeTo(int nodeId)
        {
            IEnumerator<GraphEdge> edges = _dynamicEdges.GetEnumerator();

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
            IEnumerator<GraphEdge> edges = _dynamicEdges.GetEnumerator();

            while (edges.MoveNext() == true)
            {
                if (edges.Current.TargetNodeId == nodeId)
                {
                    return edges.Current;
                }
            }
            return null;
        }
        
        internal void AddedToGraph(object ownerGraph, int id, int staticEdgeCount)
        {
            this.ownerGraph = ownerGraph;
            Id = id;
            _staticEdges = new GraphEdge[staticEdgeCount];
        }

        internal void RemovedFromGraph()
        {
            ownerGraph = null;
            Id = -1;
            _staticEdges = null;
        }

        internal void SetStaticEdge(int edgeIndex, GraphEdge edge)
        {
            _staticEdges[edgeIndex] = edge;
        }

        internal void AddDynamicEdge(GraphEdge edge)
        {
            _dynamicEdges.AddLast(edge);
        }

        internal void RemoveDynamicEdge(GraphEdge edge)
        {
            _dynamicEdges.Remove(edge);
        }

        internal GraphEdge GetFirstDynamicEdge()
        {
            return _dynamicEdges.First.Value;
        }

    }
}
