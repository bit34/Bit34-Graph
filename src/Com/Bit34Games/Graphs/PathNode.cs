namespace Com.Bit34Games.Graphs
{
    internal class PathNode
    {
        //  MEMBERS
        public readonly int   id;
        public readonly int   rid;
        public readonly float heuristic;
        public float          weight;
        public Edge           selectedEdge;
        public bool           isClosed;

        //  CONSTRUCTORS
        public PathNode(int   id, 
                        int   rid,
                        float heuristic)
        {
            this.id        = id;
            this.rid       = rid;
            this.heuristic = heuristic;
        }
    }
}
