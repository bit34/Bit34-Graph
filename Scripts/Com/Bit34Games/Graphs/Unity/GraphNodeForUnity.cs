using UnityEngine;


namespace Com.Bit34Games.Graphs.Unity
{
    public class GraphNodeForUnity : GraphNode, IGraphNodeForUnity
    {
        //  MEMBERS
        public Vector3 position;
        public object  data;

        //  METHODS
        public Vector3 GetPosition()
        {
            return position;
        }
    }
}