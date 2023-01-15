using UnityEngine;


namespace Com.Bit34Games.Graphs.Unity
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