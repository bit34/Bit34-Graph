namespace Bit34.Unity.Graph.Grid
{
    public class GridGraphNode<TNodeData> : GraphNode<TNodeData>
        where TNodeData : new()
    {
        //  MEMBERS
        public int Column { get; private set; }
        public int Row { get; private set; }


        //  CONSTRUCTORS
        public GridGraphNode() {}


        //  METHODS   
        internal void SetLocation(int column, int row)
        {
            Column = column;
            Row = row;
        }
    }
}
