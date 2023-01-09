﻿using Com.Bit34Games.Graph.Generic;
using Com.Bit34Games.Graph.Grid;


namespace Com.Bit34Games.Graph.Rectangle
{
    public class RectangleGraph<TNode, TEdge> : GridGraph<TNode, TEdge>
        where TNode : GridGraphNode
        where TEdge : GraphEdge
    {
        //  MEMBERS
        public readonly RectangleGraphConfig rectangleConfig;
        public readonly int                  columnCount;
        public readonly int                  rowCount;
        //      Internal
        private TNode[] _nodes;


        //  CONSTRUCTOR(S)
        public RectangleGraph(RectangleGraphConfig          rectangleConfig,
                              IGraphAllocator<TNode, TEdge> allocator,
                              int                           columnCount,
                              int                           rowCount) :
            base(rectangleConfig, allocator)
        {
            this.rectangleConfig = rectangleConfig;
            this.columnCount     = columnCount;
            this.rowCount        = rowCount;

            CreateNodes();
            CreateEdges();

            IsFixed = true;
        }


        //  METHODS
        override public TNode GetNodeByLocation( int column, int row)
        {
            return _nodes[column+(row*columnCount)];
        }

        override public TNode TryGetNodeByLocation(int column, int row)
        {
            if (column >= 0 && column < columnCount && row >= 0 && row < rowCount)
            {
                return _nodes[column + (row * columnCount)];
            }
            return null;
        }

        private void CreateNodes()
        {
            _nodes = new TNode[columnCount* rowCount];

            for (int c = 0; c < columnCount; c++)
            {
                for (int r = 0; r < rowCount; r++)
                {
                    TNode node = CreateNode();
                    node.SetLocation(c, r);
                    node.Position = rectangleConfig.GetNodePosition(c, r);
                    _nodes[columnCount*r+c] = node;
                }
            }
        }

        private void CreateEdges()
        {
            if (rectangleConfig.hasStraightEdges == true)
            {
                CreateStraightEdges();
            }

            if (rectangleConfig.hasDiagonalEdges == true)
            {
                CreateDiagonalEdges();
            }
        }

        private void CreateStraightEdges()
        {
            int horizontalEdge = (int)RectangleGraphEdges.RIGHT;
            int horizontalOppositeEdge = GetOppositeEdge(horizontalEdge);

            int verticalEdge = (rectangleConfig.isYAxisUp) ? ((int)RectangleGraphEdges.UP) : ((int)RectangleGraphEdges.DOWN);
            int verticalOppositeEdge = GetOppositeEdge(verticalEdge);

            for (int c = 0; c < columnCount; c++)
            {
                for (int r = 0; r < rowCount; r++)
                {
                    if (c < columnCount - 1)
                    {
                        CreateEdge(GetNodeByLocation(c, r), GetNodeByLocation(c + 1, r), horizontalEdge, horizontalOppositeEdge, true);
                    }

                    if (r < rowCount - 1)
                    {
                        CreateEdge(GetNodeByLocation(c, r), GetNodeByLocation(c, r + 1), verticalEdge, verticalOppositeEdge, true);
                    }
                }
            }
        }

        private void CreateDiagonalEdges()
        {
            int rightDiagonalEdge = (rectangleConfig.isYAxisUp) ? ((int)RectangleGraphEdges.RIGHT_UP) : ((int)RectangleGraphEdges.RIGHT_DOWN);
            int rightDiagonalOppositeEdge = GetOppositeEdge(rightDiagonalEdge);

            int leftDiagonalEdge = (rectangleConfig.isYAxisUp) ? ((int)RectangleGraphEdges.LEFT_UP) : ((int)RectangleGraphEdges.LEFT_DOWN);
            int leftDiagonalOppositeEdge = GetOppositeEdge(leftDiagonalEdge);

            for (int c = 0; c < columnCount; c++)
            {
                for (int r = 0; r < rowCount; r++)
                {
                    if (c < columnCount - 1 && r < rowCount - 1)
                    {
                        CreateEdge(GetNodeByLocation(c, r), GetNodeByLocation(c + 1, r + 1), rightDiagonalEdge, rightDiagonalOppositeEdge, true);
                    }

                    if (c > 0 && r < rowCount - 1)
                    {
                        CreateEdge(GetNodeByLocation(c, r), GetNodeByLocation(c - 1, r + 1), leftDiagonalEdge, leftDiagonalOppositeEdge, true);
                    }
                }
            }
        }

    }
}