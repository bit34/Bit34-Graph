using System.Collections.Generic;


namespace Bit34.Unity.Graph
{
    public class GraphPath<TNodeData, TNode>
        where TNodeData : new()
        where TNode : GraphNode<TNodeData>, new()
    {
        //  MEMBERS
        public TNode StartNode { get; private set; }
        public TNode EndNode { get; private set; }
        public LinkedList<GraphEdge> Edges { get; private set; }


        //  CONSTRUCTOR(S)
        public GraphPath()
        { }


        //  METHODS
        internal void Set(TNode startNode, TNode endNode, LinkedList<GraphEdge> edges)
        {
            StartNode = startNode;
            EndNode   = endNode;
            Edges     = edges;
        }

    }
}
