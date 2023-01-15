using System.Collections.Generic;


namespace Com.Bit34Games.Graphs
{
    public class GraphPath
    {
        //  MEMBERS
        public int StartNodeId { get; private set; }
        public int EndNodeId { get; private set; }
        public LinkedList<GraphEdge> Edges { get; private set; }


        //  CONSTRUCTOR(S)
        public GraphPath()
        {
            Edges = new LinkedList<GraphEdge>();
        }


        //  METHODS
        internal void Init(int startNodeId, int endNodeId)
        {
            StartNodeId = startNodeId;
            EndNodeId   = endNodeId;
            Edges.Clear();
        }

    }
}
