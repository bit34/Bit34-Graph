namespace Com.Bit34Games.Graphs
{
    public abstract class GraphConfig<TNode>
        where TNode : GraphNode
    {
        //  MEMBERS
        public readonly int staticConnectionCount;


        //  CONSTRUCTORS
        public GraphConfig(int staticConnectionCount)
        {
            this.staticConnectionCount = staticConnectionCount;
        }

        //  METHODS
        abstract public float CalculateConnectionWeight(TNode sourceNode, TNode targetNode);
    }
}
