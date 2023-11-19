namespace Com.Bit34Games.Graphs
{
    public abstract class RectGraphConfig<TNode, TConnection> : GridGraphConfig<TNode, TConnection>
        where TNode : RectGraphNode<TConnection>
        where TConnection : RectGraphConnection
    {
        //  MEMBERS
        public readonly bool isYAxisUp;
        public readonly bool hasStraightConnections;
        public readonly bool hasDiagonalConnections;


        //  CONSTRUCTORS
        public RectGraphConfig(bool isYAxisUp,
                               bool hasStraightConnections,
                               bool hasDiagonalConnections) : 
            base(8)
        {
            this.isYAxisUp        = isYAxisUp;
            this.hasStraightConnections = hasStraightConnections;
            this.hasDiagonalConnections = hasDiagonalConnections;
        }

        //  METHODS
        abstract public void InitializeNode(RectGraphNode<TConnection> node, int column, int row);
    }
}
