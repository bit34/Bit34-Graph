namespace Com.Bit34Games.Graphs
{
    public class Edge
    {
        //  MEMBERS
        public int   SourceNodeId { get; private set; }
        public int   SourceNodeRid { get; private set; }
        public int   SourceEdgeIndex { get; private set; }
        public int   TargetNodeId { get; private set; }
        public int   TargetNodeRid { get; private set; }
        public int   TargetEdgeIndex { get; private set; }
        public float Weight { get; private set; }
        public Edge  Opposite { get; private set; }


        //  CONSTRUCTORS
        public Edge()
        { }


        //  METHODS
        public void UpdateWeight(float weight)
        {
            Weight = weight;
        }
        
        internal void Set(int   sourceNodeId,
                          int   sourceNodeRid,
                          int   sourceEdgeIndex,
                          int   targetNodeId,
                          int   targetNodeRid,
                          int   targetEdgeIndex,
                          float weight,
                          Edge  opposite)
        {
            SourceNodeId    = sourceNodeId;
            SourceNodeRid   = sourceNodeRid;
            SourceEdgeIndex = sourceEdgeIndex;
            TargetNodeId    = targetNodeId;
            TargetNodeRid   = targetNodeRid;
            TargetEdgeIndex = targetEdgeIndex;
            Weight          = weight;
            Opposite        = opposite;
        }

        internal void Reset()
        {
            SourceNodeId    = -1;
            SourceNodeRid   = -1;
            SourceEdgeIndex = -1;
            TargetNodeId    = -1;
            TargetNodeRid   = -1;
            TargetEdgeIndex = -1;
            Weight          = 0;
            Opposite        = null;
        }

        internal void OppositeRemoved()
        {
            Opposite = null;
        }
    }
}
