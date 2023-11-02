using System.Collections.Generic;


namespace Com.Bit34Games.Graphs
{
    public class AgentPath
    {
        //  MEMBERS
        public int StartNodeId { get; private set; }
        public int EndNodeId { get; private set; }
        public LinkedList<GraphConnection> Connections { get; private set; }


        //  CONSTRUCTOR(S)
        public AgentPath()
        {
            Connections = new LinkedList<GraphConnection>();
        }


        //  METHODS
        internal void Init(int startNodeId, int endNodeId)
        {
            StartNodeId = startNodeId;
            EndNodeId   = endNodeId;
            Connections.Clear();
        }

    }
}
