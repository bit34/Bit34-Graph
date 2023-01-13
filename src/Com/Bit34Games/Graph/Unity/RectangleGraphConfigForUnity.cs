using UnityEngine;
using Com.Bit34Games.Graph.Rectangle;
using Com.Bit34Games.Graph.Generic;

namespace Com.Bit34Games.Graph.Unity
{
    public class RectangleGraphConfigForUnity : RectangleGraphConfig
    {
        //  MEMBERS
        public readonly Vector3 xAxis;
        public readonly Vector3 yAxis;


        //  CONSTRUCTORS
        public RectangleGraphConfigForUnity(Vector3 xAxis,
                                            Vector3 yAxis,
                                            bool    isYAxisUp = false,
                                            bool    hasStraightEdges = true,
                                            bool    hasDiagonalEdges = false) : 
            base(isYAxisUp,
                 hasStraightEdges,
                 hasDiagonalEdges)
        {
            this.xAxis = xAxis;
            this.yAxis = yAxis;
        }

        override public float CalculateEdgeWeight(GraphNode sourceNode, GraphNode targetNode)
        {
            RectangleGraphNodeForUnity castedSourceNode = (RectangleGraphNodeForUnity)sourceNode;
            RectangleGraphNodeForUnity castedTargetNode = (RectangleGraphNodeForUnity)targetNode;
            return (castedTargetNode.position - castedSourceNode.position).magnitude;
        }

        override public void InitializeNode(RectangleGraphNode node, int column, int row)
        {
            RectangleGraphNodeForUnity castedNode = (RectangleGraphNodeForUnity)node;
            castedNode.position = GetNodePosition(column, row);
        }

        public Vector3 GetNodePosition(int column, int row)
        {
            return (xAxis * column) + (yAxis * row);
        }
    }
}