using UnityEngine;


namespace Bit34.Unity.Graph.Grid
{
    public abstract class GridGraphConfig
    {
        //  MEMBERS
        public readonly int StaticEdgeCount;


        //  CONSTRUCTOR
        public GridGraphConfig(int staticEdgeCount)
        {
            StaticEdgeCount = staticEdgeCount;
        }


        //  METHODS
        public abstract Vector3 GetNodePostion(int column, int row);
    }
}
