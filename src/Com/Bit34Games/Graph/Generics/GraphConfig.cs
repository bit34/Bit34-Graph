namespace Com.Bit34Games.Graph.Generic
{
    public abstract class GraphConfig
    {
        //  MEMBERS
        public readonly int staticEdgeCount;


        //  CONSTRUCTORS
        public GraphConfig(int staticEdgeCount)
        {
            this.staticEdgeCount = staticEdgeCount;
        }

        //  METHODS
        abstract public float CalculateEdgeWeight(GraphNode sourceNode, GraphNode targetNode);
    }
}
