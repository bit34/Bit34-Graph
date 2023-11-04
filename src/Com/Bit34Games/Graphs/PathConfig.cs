using System;

namespace Com.Bit34Games.Graphs
{
    public class PathConfig<TNode, TConnection>
        where TNode : GraphNode
        where TConnection : GraphConnection
    {
        //  DELEGATES
        public delegate bool ConnectionAgentDelegate(GraphConnection connection, IAgentPathOwner pathOwner);


        //  MEMBERS
        public readonly bool useStaticConnections;
        public readonly bool useDynamicConnections;
        public readonly Func<GraphConnection,IAgentPathOwner,bool> isConnectionAccessible;


        //  CONSTRUCTOR(S)
        public PathConfig(bool useStaticConnections=true, 
                          bool useDynamicConnections=true, 
                          Func<GraphConnection,IAgentPathOwner,bool> isConnectionAccessible=null)
        {
            this.useStaticConnections   = useStaticConnections;
            this.useDynamicConnections  = useDynamicConnections;
            this.isConnectionAccessible = isConnectionAccessible;
        }

    }
}
