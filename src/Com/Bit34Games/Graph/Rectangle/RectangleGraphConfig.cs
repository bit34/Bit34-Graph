using Com.Bit34Games.Graph.Grid;


namespace Com.Bit34Games.Graph.Rectangle
{
    public abstract class RectangleGraphConfig : GridGraphConfig
    {
        //  MEMBERS
        public readonly bool    isYAxisUp;
        public readonly bool    hasStraightEdges;
        public readonly bool    hasDiagonalEdges;


        //  CONSTRUCTORS
        public RectangleGraphConfig(
            bool    isYAxisUp,
            bool    hasStraightEdges,
            bool    hasDiagonalEdges) : 
            base(8)
        {
            this.isYAxisUp        = isYAxisUp;
            this.hasStraightEdges = hasStraightEdges;
            this.hasDiagonalEdges = hasDiagonalEdges;
        }

        //  METHODS
        abstract public void InitializeNode(RectangleGraphNode node, int column, int row);
    }
}
