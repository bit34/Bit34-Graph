namespace Com.Bit34Games.Graphs
{
    public class RectGraphNode<TConnection> : GridGraphNode<TConnection>
        where TConnection : RectGraphConnection
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
