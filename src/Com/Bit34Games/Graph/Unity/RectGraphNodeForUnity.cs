using UnityEngine;
using Com.Bit34Games.Graph.Generic;


namespace Com.Bit34Games.Graph.Unity
{
    public class RectGraphNodeForUnity : RectGraphNode, IGraphNodeForUnity
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