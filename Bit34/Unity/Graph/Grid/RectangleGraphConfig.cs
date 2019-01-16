using UnityEngine;


namespace Bit34.Unity.Graph.Grid
{
    public class RectangleGraphConfig : GridGraphConfig
    {
        //  MEMBERS
        public readonly Vector3 XAxis;
        public readonly Vector3 YAxis;
        public readonly bool    IsYAxisUp;
        public readonly bool    HasStraightEdges;
        public readonly bool    HasDiagonalEdges;


        //  CONSTRUCTOR(S)
        public RectangleGraphConfig(
            Vector3 xAxis,
            Vector3 yAxis,
            bool    isYAxisUp = false,
            bool    hasStraightEdges = true,
            bool    hasDiagonalEdges = false) : 
            base(8)
        {
            XAxis = xAxis;
            YAxis = yAxis;
            IsYAxisUp = isYAxisUp;
            HasStraightEdges = hasStraightEdges;
            HasDiagonalEdges = hasDiagonalEdges;
        }

        public override Vector3 GetNodePostion(int column, int row)
        {
            return (XAxis * column) + (YAxis * row);
        }
    }
}
