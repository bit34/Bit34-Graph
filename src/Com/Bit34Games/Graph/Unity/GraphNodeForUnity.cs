using Com.Bit34Games.Graph.Generic;
using UnityEngine;


namespace Com.Bit34Games.Graph.Unity
{
    public class GraphNodeForUnity : GraphNode, IGraphNodeForUnity
    {
        //  MEMBERS
        public Vector3 position;

        //  METHODS
        public Vector3 GetPosition()
        {
            return position;
        }
    }
}