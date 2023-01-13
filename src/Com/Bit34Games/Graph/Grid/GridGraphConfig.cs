using Com.Bit34Games.Graph.Generic;


namespace Com.Bit34Games.Graph.Grid
{
    public abstract class GridGraphConfig : GraphConfig
    {
        //  CONSTRUCTOR
        public GridGraphConfig(int staticEdgeCount) : 
            base(staticEdgeCount)
        {}
    }
}
