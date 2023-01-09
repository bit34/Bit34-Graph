using Com.Bit34Games.Graph.Generic;
using UnityEngine;


namespace Com.Bit34Games.Graph.Grid
{
    public abstract class GridGraphConfig : GraphConfig
    {
        //  CONSTRUCTOR
        public GridGraphConfig(int staticEdgeCount) : base(staticEdgeCount)
        { }


        //  METHODS
        public abstract Vector3 GetNodePosition(int column, int row);
    }
}
