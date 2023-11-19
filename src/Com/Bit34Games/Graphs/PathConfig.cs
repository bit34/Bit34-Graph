using System;

namespace Com.Bit34Games.Graphs
{
    public class PathConfig<TNode, TConnection>
        where TNode : Node<TConnection>
        where TConnection : Connection
    {
        //  DELEGATES
        public delegate bool ConnectionAgentDelegate(Connection connection, IPathOwner pathOwner);


        //  MEMBERS
        public readonly bool useStaticConnections;
        public readonly bool useDynamicConnections;
        public readonly Func<Connection,IPathOwner,bool> isConnectionAccessible;


        //  CONSTRUCTOR(S)
        public PathConfig(bool useStaticConnections=true, 
                          bool useDynamicConnections=true, 
                          Func<Connection,IPathOwner,bool> isConnectionAccessible=null)
        {
            this.useStaticConnections   = useStaticConnections;
            this.useDynamicConnections  = useDynamicConnections;
            this.isConnectionAccessible = isConnectionAccessible;
        }

    }
}
