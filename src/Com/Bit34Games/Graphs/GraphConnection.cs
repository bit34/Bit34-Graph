namespace Com.Bit34Games.Graphs
{
    public class GraphConnection
    {
        //  MEMBERS
        public int             SourceNodeId { get; private set; }
        public int             SourceConnectionIndex { get; private set; }
        public int             TargetNodeId { get; private set; }
        public int             TargetConnectionIndex { get; private set; }
        public float           Weight { get; private set; }
        public GraphConnection Opposite { get; private set; }


        //  CONSTRUCTORS
        public GraphConnection()
        { }


        //  METHODS
        public void UpdateWeight(float weight)
        {
            Weight = weight;
        }
        
        internal void Set(int             sourceNodeId,
                          int             sourceConnectionIndex,
                          int             targetNodeId,
                          int             targetConnectionIndex,
                          float           weight,
                          GraphConnection opposite)
        {
            SourceNodeId          = sourceNodeId;
            SourceConnectionIndex = sourceConnectionIndex;
            TargetNodeId          = targetNodeId;
            TargetConnectionIndex = targetConnectionIndex;
            Weight                = weight;
            Opposite              = opposite;
        }

        internal void Reset()
        {
            SourceNodeId          = -1;
            SourceConnectionIndex = -1;
            TargetNodeId          = -1;
            TargetConnectionIndex = -1;
            Weight                = 0;
            Opposite              = null;
        }

        internal void OppositeRemoved()
        {
            Opposite = null;
        }
    }
}
