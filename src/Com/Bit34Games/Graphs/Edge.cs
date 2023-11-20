namespace Com.Bit34Games.Graphs
{
    public class Edge
    {
        //  MEMBERS
        public int   SourceNodeId { get; private set; }
        public int   SourceNodeRuntimeIndex { get; private set; }
        public int   SourceEdgeIndex { get; private set; }
        public int   TargetNodeId { get; private set; }
        public int   TargetNodeRuntimeIndex { get; private set; }
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
                          int   sourceNodeRuntimeIndex,
                          int   sourceEdgeIndex,
                          int   targetNodeId,
                          int   targetNodeRuntimeIndex,
                          int   targetEdgeIndex,
                          float weight,
                          Edge  opposite)
        {
            SourceNodeId           = sourceNodeId;
            SourceNodeRuntimeIndex = sourceNodeRuntimeIndex;
            SourceEdgeIndex        = sourceEdgeIndex;
            TargetNodeId           = targetNodeId;
            TargetNodeRuntimeIndex = targetNodeRuntimeIndex;
            TargetEdgeIndex        = targetEdgeIndex;
            Weight                 = weight;
            Opposite               = opposite;
        }

        internal void Reset()
        {
            SourceNodeId           = -1;
            SourceNodeRuntimeIndex = -1;
            SourceEdgeIndex        = -1;
            TargetNodeId           = -1;
            TargetNodeRuntimeIndex = -1;
            TargetEdgeIndex        = -1;
            Weight                 = 0;
            Opposite               = null;
        }

        internal void OppositeRemoved()
        {
            Opposite = null;
        }
    }
}
