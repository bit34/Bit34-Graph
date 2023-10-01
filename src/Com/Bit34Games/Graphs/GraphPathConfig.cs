namespace Com.Bit34Games.Graphs
{
    public class GraphPathConfig
    {
        //  DELEGATES
        public delegate bool ConnectionAgentDelegate(GraphConnection connection, GraphAgent agent);


        //  MEMBERS
        public readonly bool                    useStaticConnections;
        public readonly bool                    useDynamicConnections;
        public readonly ConnectionAgentDelegate isConnectionAccessible;


        //  CONSTRUCTOR(S)
        public GraphPathConfig(bool                    useStaticConnections=true, 
                               bool                    useDynamicConnections=true, 
                               ConnectionAgentDelegate isConnectionAccessible=null)
        {
            this.useStaticConnections   = useStaticConnections;
            this.useDynamicConnections  = useDynamicConnections;
            this.isConnectionAccessible = isConnectionAccessible;
        }

    }
}
