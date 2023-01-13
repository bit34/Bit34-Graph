using UnityEngine;
using Com.Bit34Games.Graph.Generic;

namespace Com.Bit34Games.Graph.Unity
{
    public class RectGraphConfigForUnity : RectGraphConfig
    {
        //  MEMBERS
        public readonly Vector3 xAxis;
        public readonly Vector3 yAxis;


        //  CONSTRUCTORS
        public RectGraphConfigForUnity(Vector3 xAxis,
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
            RectGraphNodeForUnity castedSourceNode = (RectGraphNodeForUnity)sourceNode;
            RectGraphNodeForUnity castedTargetNode = (RectGraphNodeForUnity)targetNode;
            return (castedTargetNode.position - castedSourceNode.position).magnitude;
        }

        override public void InitializeNode(RectGraphNode node, int column, int row)
        {
            RectGraphNodeForUnity castedNode = (RectGraphNodeForUnity)node;
            castedNode.position = GetNodePosition(column, row);
        }

        public Vector3 GetNodePosition(int column, int row)
        {
            return (xAxis * column) + (yAxis * row);
        }
    }
}