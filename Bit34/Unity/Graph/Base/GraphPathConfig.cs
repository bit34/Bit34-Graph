namespace Bit34.Unity.Graph.Base
{
    public class GraphPathConfig
    {
        //  TYPES
        public delegate bool EdgeAgentDelegate(GraphEdge edge, GraphAgent agent);


        //  MEMBERS
        public readonly bool              UseStaticEdges;
        public readonly bool              UseDynamicEdges;
        public readonly EdgeAgentDelegate IsEdgeAccessible;


        //  CONSTRUCTOR(S)
        public GraphPathConfig(bool useStaticEdges=true, bool useDynamicEdges=true, EdgeAgentDelegate isEdgeAccessible=null)
        {
            UseStaticEdges = useStaticEdges;
            UseDynamicEdges = useDynamicEdges;
            IsEdgeAccessible = isEdgeAccessible;
        }

    }
}
