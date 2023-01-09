namespace Com.Bit34Games.Graph.Generic
{
    public class GraphEdge
    {
        //  MEMBERS
        public int SourceNodeId { get; private set; }
        public int SourceEdgeIndex { get; private set; }
        public int TargetNodeId { get; private set; }
        public int TargetEdgeIndex { get; private set; }
        public float Weight { get; private set; }
        public GraphEdge OppositeEdge { get; private set; }
        //      Internal
        private object _Data;



        //  CONSTRUCTORS
        public GraphEdge()
        { }


        //  METHODS
        public void UpdateWeight(float weight)
        {
            Weight = weight;
        }

        public TData GetData<TData>()
        {
            return (TData)_Data;
        }

        internal void InitData(object data)
        {
            _Data = data;
        }
        
        internal void Set(int sourceNodeId, int sourceEdgeIndex, int targetNodeId, int targetEdgeIndex, float weight, GraphEdge oppositeEdge)
        {
            SourceNodeId    = sourceNodeId;
            SourceEdgeIndex = sourceEdgeIndex;
            TargetNodeId    = targetNodeId;
            TargetEdgeIndex = targetEdgeIndex;
            Weight          = weight;
            OppositeEdge    = oppositeEdge;
        }

        internal void Reset()
        {
            SourceNodeId    = -1;
            SourceEdgeIndex = -1;
            TargetNodeId    = -1;
            TargetEdgeIndex = -1;
            Weight          = 0;
            OppositeEdge    = null;
        }

        internal void OppositeRemoved()
        {
            OppositeEdge = null;
        }
    }
}
