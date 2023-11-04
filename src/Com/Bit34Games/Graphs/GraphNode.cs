using System.Collections.Generic;


namespace Com.Bit34Games.Graphs
{

    public class GraphNode
    {
        //  MEMBERS
        public int Id                     { get; private set; }
        public int RuntimeIndex           { get; private set; }
        public int StaticConnectionCount  { get { return _staticConnections.Length; } }
        public int DynamicConnectionCount { get { return _dynamicConnections.Count; } }
        //      Internal
        internal IGraphNodeOwner ownerGraph;
        //      private
        private GraphConnection[]           _staticConnections;
        private LinkedList<GraphConnection> _dynamicConnections;


        //  CONSTRUCTORS
        public GraphNode()
        {
            _dynamicConnections = new LinkedList<GraphConnection>();
        }


        //  METHODS
        public GraphConnection GetStaticConnection(int connectionIndex)
        {
            return _staticConnections[connectionIndex];
        }

        public GraphConnection GetStaticConnectionTo(int nodeId)
        {
            for (int i = 0; i < _staticConnections.Length; i++)
            {
                if (_staticConnections[i] != null && _staticConnections[i].TargetNodeId == nodeId)
                {
                    return _staticConnections[i];
                }
            }
            return null;
        }

        public IEnumerator<GraphConnection> GetDynamicConnectionEnumerator()
        {
            return _dynamicConnections.GetEnumerator();
        }

        public GraphConnection GetDynamicConnectionTo(int nodeId)
        {
            IEnumerator<GraphConnection> connections = _dynamicConnections.GetEnumerator();

            while (connections.MoveNext() == true)
            {
                if (connections.Current.TargetNodeId == nodeId)
                {
                    return connections.Current;
                }
            }
            return null;
        }
        
        internal void AddedToGraph(IGraphNodeOwner owner, int id, int runtimeIndex, int staticConnectionCount)
        {
            this.ownerGraph    = owner;
            Id                 = id;
            RuntimeIndex       = runtimeIndex;
            _staticConnections = new GraphConnection[staticConnectionCount];
        }

        internal void RemovedFromGraph()
        {
            ownerGraph         = null;
            Id                 = -1;
            _staticConnections = null;
        }

        internal void SetStaticConnection(int connectionIndex, GraphConnection connection)
        {
            _staticConnections[connectionIndex] = connection;
        }

        internal void AddDynamicConnection(GraphConnection connection)
        {
            _dynamicConnections.AddLast(connection);
        }

        internal void RemoveDynamicConnection(GraphConnection connection)
        {
            _dynamicConnections.Remove(connection);
        }

        internal GraphConnection GetFirstDynamicConnection()
        {
            return _dynamicConnections.First.Value;
        }

    }
}
