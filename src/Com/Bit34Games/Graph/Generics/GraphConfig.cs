namespace Com.Bit34Games.Graph.Generic
{
    public abstract class GraphConfig
    {
        //  MEMBERS
        public readonly int staticEdgeCount;


        //  CONSTRUCTOR
        public GraphConfig(int staticEdgeCount)
        {
            this.staticEdgeCount = staticEdgeCount;
        }
    }
}
