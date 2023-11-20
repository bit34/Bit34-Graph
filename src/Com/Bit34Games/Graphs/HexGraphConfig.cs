namespace Com.Bit34Games.Graphs
{
    public abstract class HexGraphConfig<TNode, TEdge> : GridGraphConfig<TNode, TEdge>
        where TNode : HexNode<TEdge>
        where TEdge : HexEdge
    {
        //  MEMBERS
        public readonly bool isYAxisUp;


        //  CONSTRUCTORS
        public HexGraphConfig(bool isYAxisUp) : 
            base(6)
        {
            this.isYAxisUp      = isYAxisUp;
        }

        //  METHODS
        abstract public void InitializeNode(HexNode<TEdge> node, int column, int row);
    }
}
