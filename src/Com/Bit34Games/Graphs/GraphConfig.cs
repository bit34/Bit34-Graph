namespace Com.Bit34Games.Graphs
{
    public abstract class GraphConfig<TNode, TEdge>
        where TNode : Node<TEdge>
        where TEdge : Edge
    {
        //  MEMBERS
        public readonly int statiEdgeCount;


        //  CONSTRUCTORS
        public GraphConfig(int staticEdgeCount)
        {
            this.statiEdgeCount = staticEdgeCount;
        }

        //  METHODS
        abstract public float CalculateEdgeWeight(TNode sourceNode, TNode targetNode);
    }
}
