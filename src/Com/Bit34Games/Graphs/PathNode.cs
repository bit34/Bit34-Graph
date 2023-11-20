namespace Com.Bit34Games.Graphs
{
    internal class PathNode
    {
        public readonly int id;
        public readonly int rid;
        public float        weight;
        public Edge         selectedEdge;
        public bool         isClosed;

        public PathNode(int id, 
                        int rid)
        {
            this.id  = id;
            this.rid = rid;
        }
    }
}
