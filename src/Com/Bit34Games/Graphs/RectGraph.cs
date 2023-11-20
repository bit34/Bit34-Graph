namespace Com.Bit34Games.Graphs
{
    public class RectGraph<TConfig, TNode, TEdge> : GridGraph<TConfig, TNode, TEdge>
        where TConfig : RectGraphConfig<TNode, TEdge>
        where TNode : RectNode<TEdge>
        where TEdge : RectEdge
    {
        //  MEMBERS
        public readonly int columnCount;
        public readonly int rowCount;
        //      Internal
        private TNode[] _nodes;


        //  CONSTRUCTOR(S)
        public RectGraph(TConfig                       config,
                         IGraphAllocator<TNode, TEdge> allocator,
                         int                           columnCount,
                         int                           rowCount) :
            base(config, allocator)
        {
            this.columnCount = columnCount;
            this.rowCount    = rowCount;

            CreateNodes();
            CreateEdges();

            IsFixed = true;
        }


        //  METHODS
        public TNode GetNodeByLocation( int column, int row)
        {
            return _nodes[column+(row*columnCount)];
        }

        public TNode TryGetNodeByLocation(int column, int row)
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
                    TNode node = AddNode();
                    node.SetLocation(c, r);
                    Config.InitializeNode(node, c, r);
                    _nodes[columnCount*r+c] = node;
                }
            }
        }

        private void CreateEdges()
        {
            if (Config.hasStraightEdges == true)
            {
                CreateStraightEdges();
            }

            if (Config.hasDiagonalEdges == true)
            {
                CreateDiagonalEdges();
            }
        }

        private void CreateStraightEdges()
        {
            int horizontalEdge         = (int)RectEdgeDirections.RIGHT;
            int horizontalOppositeEdge = GetOppositeEdge(horizontalEdge);

            int verticalEdge         = (Config.isYAxisUp) ? ((int)RectEdgeDirections.UP) : ((int)RectEdgeDirections.DOWN);
            int verticalOppositeEdge = GetOppositeEdge(verticalEdge);

            for (int c = 0; c < columnCount; c++)
            {
                for (int r = 0; r < rowCount; r++)
                {
                    if (c < columnCount - 1)
                    {
                        AddEdge(GetNodeByLocation(c, r), GetNodeByLocation(c + 1, r), horizontalEdge, horizontalOppositeEdge, true);
                    }

                    if (r < rowCount - 1)
                    {
                        AddEdge(GetNodeByLocation(c, r), GetNodeByLocation(c, r + 1), verticalEdge, verticalOppositeEdge, true);
                    }
                }
            }
        }

        private void CreateDiagonalEdges()
        {
            int rightDiagonalEdge         = (Config.isYAxisUp) ? ((int)RectEdgeDirections.RIGHT_UP) : ((int)RectEdgeDirections.RIGHT_DOWN);
            int rightDiagonalOppositeEdge = GetOppositeEdge(rightDiagonalEdge);

            int leftDiagonalEdge         = (Config.isYAxisUp) ? ((int)RectEdgeDirections.LEFT_UP) : ((int)RectEdgeDirections.LEFT_DOWN);
            int leftDiagonalOppositeEdge = GetOppositeEdge(leftDiagonalEdge);

            for (int c = 0; c < columnCount; c++)
            {
                for (int r = 0; r < rowCount; r++)
                {
                    if (c < columnCount - 1 && r < rowCount - 1)
                    {
                        AddEdge(GetNodeByLocation(c, r), GetNodeByLocation(c + 1, r + 1), rightDiagonalEdge, rightDiagonalOppositeEdge, true);
                    }

                    if (c > 0 && r < rowCount - 1)
                    {
                        AddEdge(GetNodeByLocation(c, r), GetNodeByLocation(c - 1, r + 1), leftDiagonalEdge, leftDiagonalOppositeEdge, true);
                    }
                }
            }
        }

    }
}
