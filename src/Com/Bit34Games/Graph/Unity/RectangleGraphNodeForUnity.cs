using UnityEngine;
using Com.Bit34Games.Graph.Rectangle;


namespace Com.Bit34Games.Graph.Unity
{
    public class RectangleGraphNodeForUnity : RectangleGraphNode, IGraphNodeForUnity
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