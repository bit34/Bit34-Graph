namespace Com.Bit34Games.Graph.Generic
{
    public class RectGraphNode : GridGraphNode
    {
        //  MEMBERS
        public int Column { get; private set; }
        public int Row    { get; private set; }


        //  CONSTRUCTORS
        public RectGraphNode() {}


        //  METHODS   
        internal void SetLocation(int column, int row)
        {
            Column = column;
            Row    = row;
        }
    }
}
