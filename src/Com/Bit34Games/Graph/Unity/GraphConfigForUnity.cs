using Com.Bit34Games.Graph.Generic;

namespace Com.Bit34Games.Graph.Unity
{
    public class GraphConfigForUnity : GraphConfig
    {
        //  CONSTRUCTOR
        public GraphConfigForUnity(int staticEdgeCount) : 
            base(staticEdgeCount)
        {}

        //  METHODS
        override public float CalculateEdgeWeight(GraphNode sourceNode, GraphNode targetNode)
        {
            GraphNodeForUnity castedSourceNode = (GraphNodeForUnity)sourceNode;
            GraphNodeForUnity castedTargetNode = (GraphNodeForUnity)targetNode;
            return (castedTargetNode.position - castedSourceNode.position).magnitude;
        }
    }
}