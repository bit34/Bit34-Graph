namespace Com.Bit34Games.Graphs
{
    public class HexNode<TEdge> : GridNode<TEdge>
        where TEdge : HexEdge
    {
        //  MEMBERS
        public int Column { get; private set; }
        public int Row    { get; private set; }


        //  CONSTRUCTORS
        public HexNode() {}


        //  METHODS   
        internal void SetLocation(int column, int row)
        {
            Column = column;
            Row    = row;
        }
    }
}
