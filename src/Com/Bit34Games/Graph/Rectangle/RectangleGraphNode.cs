using Com.Bit34Games.Graph.Grid;


namespace Com.Bit34Games.Graph.Rectangle
{
    public class RectangleGraphNode : GridGraphNode
    {
        //  MEMBERS
        public int Column { get; private set; }
        public int Row    { get; private set; }


        //  CONSTRUCTORS
        public RectangleGraphNode() {}


        //  METHODS   
        internal void SetLocation(int column, int row)
        {
            Column = column;
            Row    = row;
        }
    }
}
