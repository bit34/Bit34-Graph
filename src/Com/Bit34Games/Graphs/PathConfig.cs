using System;

namespace Com.Bit34Games.Graphs
{
    public class PathConfig<TNode, TEdge>
        where TNode : Node<TEdge>
        where TEdge : Edge
    {
        //  DELEGATES
        public delegate bool EdgeAgentDelegate(Edge edge, IPathOwner pathOwner);


        //  MEMBERS
        public readonly bool                       useStaticEdges;
        public readonly bool                       useDynamicEdges;
        public readonly Func<Edge,IPathOwner,bool> isEdgeAccessible;


        //  CONSTRUCTOR(S)
        public PathConfig(bool                       useStaticEdges=true, 
                          bool                       useDynamicEdges=true, 
                          Func<Edge,IPathOwner,bool> isEdgeAccessible=null)
        {
            this.useStaticEdges   = useStaticEdges;
            this.useDynamicEdges  = useDynamicEdges;
            this.isEdgeAccessible = isEdgeAccessible;
        }

    }
}
