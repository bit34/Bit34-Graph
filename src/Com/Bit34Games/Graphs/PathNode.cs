namespace Com.Bit34Games.Graphs
{
    internal class PathNode
    {
        public readonly int id;
        public readonly int runtimeIndex;
        public float        weight;
        public Edge         selectedEdge;
        public bool         isClosed;

        public PathNode(int id, 
                        int runtimeIndex)
        {
            this.id           = id;
            this.runtimeIndex = runtimeIndex;
        }
    }
}
