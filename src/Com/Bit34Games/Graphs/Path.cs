namespace Com.Bit34Games.Graphs
{
    public class Path
    {
        //  MEMBERS
        public readonly int startNodeId;
        public readonly int endNodeId;
        public int          ConnectionCount { get { return _connections.Length; } }
        //      private
        private Connection[] _connections;


        //  CONSTRUCTOR(S)
        public Path(int               startNodeId, 
                    int               endNodeId,
                    Connection[] connections)
        {
            this.startNodeId = startNodeId;
            this.endNodeId   = endNodeId;
            _connections     = connections;
        }


        //  METHODS
        public Connection Getconnection(int index)
        {
            return _connections[index];
        }

    }
}
