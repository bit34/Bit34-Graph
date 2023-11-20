namespace Com.Bit34Games.Graphs
{
    public abstract class RectGraphConfig<TNode, TEdge> : GridGraphConfig<TNode, TEdge>
        where TNode : RectNode<TEdge>
        where TEdge : RectEdge
    {
        //  MEMBERS
        public readonly bool isYAxisUp;
        public readonly bool hasStraightEdges;
        public readonly bool hasDiagonalEdges;


        //  CONSTRUCTORS
        public RectGraphConfig(bool isYAxisUp,
                               bool hasStraightEdges,
                               bool hasDiagonalEdges) : 
            base(8)
        {
            this.isYAxisUp        = isYAxisUp;
            this.hasStraightEdges = hasStraightEdges;
            this.hasDiagonalEdges = hasDiagonalEdges;
        }

        //  METHODS
        abstract public void InitializeNode(RectNode<TEdge> node, int column, int row);
    }
}
