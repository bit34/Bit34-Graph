namespace Com.Bit34Games.Graphs
{
    public class GraphPathConfig
    {
        //  DELEGATES
        public delegate bool EdgeAgentDelegate(GraphEdge edge, GraphAgent agent);


        //  MEMBERS
        public readonly bool              useStaticEdges;
        public readonly bool              useDynamicEdges;
        public readonly EdgeAgentDelegate isEdgeAccessible;


        //  CONSTRUCTOR(S)
        public GraphPathConfig(bool useStaticEdges=true, bool useDynamicEdges=true, EdgeAgentDelegate isEdgeAccessible=null)
        {
            this.useStaticEdges   = useStaticEdges;
            this.useDynamicEdges  = useDynamicEdges;
            this.isEdgeAccessible = isEdgeAccessible;
        }

    }
}
