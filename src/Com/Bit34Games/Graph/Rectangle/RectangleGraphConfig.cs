using UnityEngine;
using Com.Bit34Games.Graph.Grid;


namespace Com.Bit34Games.Graph.Rectangle
{
    public class RectangleGraphConfig : GridGraphConfig
    {
        //  MEMBERS
        public readonly Vector3 xAxis;
        public readonly Vector3 yAxis;
        public readonly bool    isYAxisUp;
        public readonly bool    hasStraightEdges;
        public readonly bool    hasDiagonalEdges;


        //  CONSTRUCTOR(S)
        public RectangleGraphConfig(
            Vector3 xAxis,
            Vector3 yAxis,
            bool    isYAxisUp = false,
            bool    hasStraightEdges = true,
            bool    hasDiagonalEdges = false) : 
            base(8)
        {
            this.xAxis            = xAxis;
            this.yAxis            = yAxis;
            this.isYAxisUp        = isYAxisUp;
            this.hasStraightEdges = hasStraightEdges;
            this.hasDiagonalEdges = hasDiagonalEdges;
        }

        public override Vector3 GetNodePosition(int column, int row)
        {
            return (xAxis * column) + (yAxis * row);
        }
    }
}
