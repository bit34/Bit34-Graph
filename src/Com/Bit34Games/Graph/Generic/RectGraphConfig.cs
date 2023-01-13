﻿namespace Com.Bit34Games.Graph.Generic
{
    public abstract class RectGraphConfig : GridGraphConfig
    {
        //  MEMBERS
        public readonly bool isYAxisUp;
        public readonly bool hasStraightEdges;
        public readonly bool hasDiagonalEdges;


        //  CONSTRUCTORS
        public RectGraphConfig(bool isYAxisUp,
                               bool hasStraightEdges,
                               bool hasDiagonalEdges) : 
            base(8)
        {
            this.isYAxisUp        = isYAxisUp;
            this.hasStraightEdges = hasStraightEdges;
            this.hasDiagonalEdges = hasDiagonalEdges;
        }

        //  METHODS
        abstract public void InitializeNode(RectGraphNode node, int column, int row);
    }
}
