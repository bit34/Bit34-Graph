using System.Collections.Generic;


namespace Com.Bit34Games.Graphs
{

    public class Node<TConnection>
        where TConnection : Connection
    {
        //  MEMBERS
        public int Id                     { get; private set; }
        public int RuntimeIndex           { get; private set; }
        public int StaticConnectionCount  { get { return staticConnections.Length; } }
        public int DynamicConnectionCount { get { return dynamicConnections.Count; } }
        //      Internal
        internal INodeOwner         ownerGraph;
        internal TConnection[]           staticConnections;
        internal LinkedList<TConnection> dynamicConnections;


        //  CONSTRUCTORS
        public Node()
        {
            dynamicConnections = new LinkedList<TConnection>();
        }


        //  METHODS

        public TConnection GetStaticConnection(int connectionIndex)
        {
            return staticConnections[connectionIndex];
        }

        public TConnection GetStaticConnectionTo(int nodeId)
        {
            for (int i = 0; i < staticConnections.Length; i++)
            {
                if (staticConnections[i] != null && staticConnections[i].TargetNodeId == nodeId)
                {
                    return staticConnections[i];
                }
            }
            return null;
        }

        public IEnumerator<TConnection> GetDynamicConnectionEnumerator()
        {
            return dynamicConnections.GetEnumerator();
        }

        public Connection GetDynamicConnectionTo(int nodeId)
        {
            IEnumerator<TConnection> connections = dynamicConnections.GetEnumerator();

            while (connections.MoveNext() == true)
            {
                if (connections.Current.TargetNodeId == nodeId)
                {
                    return connections.Current;
                }
            }
            return null;
        }
        
        internal void AddedToGraph(INodeOwner owner, int id, int runtimeIndex, int staticConnectionCount)
        {
            this.ownerGraph    = owner;
            Id                 = id;
            RuntimeIndex       = runtimeIndex;
            staticConnections = new TConnection[staticConnectionCount];
        }

        internal void RemovedFromGraph()
        {
            ownerGraph         = null;
            Id                 = -1;
            staticConnections = null;
        }

        internal void SetStaticConnection(int connectionIndex, TConnection connection)
        {
            staticConnections[connectionIndex] = connection;
        }

        internal void AddDynamicConnection(TConnection connection)
        {
            dynamicConnections.AddLast(connection);
        }

        internal void RemoveDynamicConnection(TConnection connection)
        {
            dynamicConnections.Remove(connection);
        }

        internal TConnection GetFirstDynamicConnection()
        {
            return dynamicConnections.First.Value;
        }

    }
}
