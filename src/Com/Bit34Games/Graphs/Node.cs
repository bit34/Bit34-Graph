using System.Collections.Generic;


namespace Com.Bit34Games.Graphs
{

    public class Node<TEdge>
        where TEdge : Edge
    {
        //  MEMBERS
        public int Id               { get; private set; }
        public int Rid              { get; private set; }
        public int StaticEdgeCount  { get { return staticEdges.Length; } }
        public int DynamicEdgeCount { get { return dynamicEdges.Count; } }
        //      Internal
        internal INodeOwner        owner;
        internal TEdge[]           staticEdges;
        internal LinkedList<TEdge> dynamicEdges;


        //  CONSTRUCTORS
        public Node()
        {
            dynamicEdges = new LinkedList<TEdge>();
        }


        //  METHODS

        public TEdge GetStaticEdge(int index)
        {
            return staticEdges[index];
        }

        public TEdge GetStaticEdgeTo(int nodeId)
        {
            for (int i = 0; i < staticEdges.Length; i++)
            {
                if (staticEdges[i] != null && staticEdges[i].TargetNodeId == nodeId)
                {
                    return staticEdges[i];
                }
            }
            return null;
        }

        public IEnumerator<TEdge> GetDynamicEdgeEnumerator()
        {
            return dynamicEdges.GetEnumerator();
        }

        public Edge GetDynamicEdgeTo(int nodeId)
        {
            IEnumerator<TEdge> edges = dynamicEdges.GetEnumerator();

            while (edges.MoveNext() == true)
            {
                if (edges.Current.TargetNodeId == nodeId)
                {
                    return edges.Current;
                }
            }
            return null;
        }
        
        internal void AddedToGraph(INodeOwner owner, int id, int rid, int statiEdgeCount)
        {
            this.owner  = owner;
            Id          = id;
            Rid         = rid;
            staticEdges = new TEdge[statiEdgeCount];
        }

        internal void RemovedFromGraph()
        {
            owner       = null;
            Id          = -1;
            staticEdges = null;
        }

        internal void SetStaticEdge(int index, TEdge edge)
        {
            staticEdges[index] = edge;
        }

        internal void AddDynamicEdge(TEdge edge)
        {
            dynamicEdges.AddLast(edge);
        }

        internal void RemoveDynamicEdge(TEdge edge)
        {
            dynamicEdges.Remove(edge);
        }

        internal TEdge GetFirstDynamicEdge()
        {
            return dynamicEdges.First.Value;
        }

    }
}
