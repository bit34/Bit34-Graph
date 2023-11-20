namespace Com.Bit34Games.Graphs
{
    public class RectNode<TEdge> : GridNode<TEdge>
        where TEdge : RectEdge
    {
        //  MEMBERS
        public int Column { get; private set; }
        public int Row    { get; private set; }


        //  CONSTRUCTORS
        public RectNode() {}


        //  METHODS   
        internal void SetLocation(int column, int row)
        {
            Column = column;
            Row    = row;
        }
    }
}
