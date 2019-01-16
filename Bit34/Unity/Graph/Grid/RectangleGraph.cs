namespace Bit34.Unity.Graph.Grid
{
    public class RectangleGraph<TNodeData, TNode, TPath> : GridGraph<TNodeData, TNode, TPath>
        where TNodeData : new()
        where TNode : GridGraphNode<TNodeData>, new()
        where TPath : GridGraphPath<TNodeData, TNode>, new()
    {
        //  MEMBERS
        public readonly int ColumnCount;
        public readonly int RowCount;
        public readonly RectangleGraphConfig Config;
        private TNode[] _Nodes;


        //  CONSTRUCTOR(S)
        public RectangleGraph(int columnCount, int rowCount, RectangleGraphConfig config, IGraphAllocator<TNodeData, TNode> allocator) :
            base(config, allocator)
        {
            ColumnCount = columnCount;
            RowCount = rowCount;
            Config = config;

            CreateNodes();
            CreateEdges();

            IsFixed = true;
        }


        //  METHODS
        override public TNode GetNodeByLocation( int column, int row)
        {
            return _Nodes[column+(row*ColumnCount)];
        }

        override public TNode TryGetNodeByLocation(int column, int row)
        {
            if (column >= 0 && column < ColumnCount && row >= 0 && row < RowCount)
            {
                return _Nodes[column + (row * ColumnCount)];
            }
            return null;
        }

        private void CreateNodes()
        {
            _Nodes = new TNode[ColumnCount* RowCount];

            for (int c = 0; c < ColumnCount; c++)
            {
                for (int r = 0; r < RowCount; r++)
                {
                    TNode node = CreateNode(Config.StaticEdgeCount);
                    node.SetLocation(c, r);
                    node.Position = Config.GetNodePostion(c, r);
                    _Nodes[ColumnCount*r+c] = node;
                }
            }
        }

        private void CreateEdges()
        {
            if (Config.HasStraightEdges == true)
            {
                CreateStraightEdges();
            }

            if (Config.HasDiagonalEdges == true)
            {
                CreateDiagonalEdges();
            }
        }

        private void CreateStraightEdges()
        {
            int horizontalEdge = (int)RectangleGraphEdges.RIGHT;
            int horizontalOppositeEdge = GetOppositeEdge(horizontalEdge);

            int verticalEdge = (Config.IsYAxisUp) ? ((int)RectangleGraphEdges.UP) : ((int)RectangleGraphEdges.DOWN);
            int verticalOppositeEdge = GetOppositeEdge(verticalEdge);

            for (int c = 0; c < ColumnCount; c++)
            {
                for (int r = 0; r < RowCount; r++)
                {
                    if (c < ColumnCount - 1)
                    {
                        CreateEdge(GetNodeByLocation(c, r), GetNodeByLocation(c + 1, r), horizontalEdge, horizontalOppositeEdge, true);
                    }

                    if (r < RowCount - 1)
                    {
                        CreateEdge(GetNodeByLocation(c, r), GetNodeByLocation(c, r + 1), verticalEdge, verticalOppositeEdge, true);
                    }
                }
            }
        }

        private void CreateDiagonalEdges()
        {
            int rightDiagonalEdge = (Config.IsYAxisUp) ? ((int)RectangleGraphEdges.RIGHT_UP) : ((int)RectangleGraphEdges.RIGHT_DOWN);
            int rightDiagonalOppositeEdge = GetOppositeEdge(rightDiagonalEdge);

            int leftDiagonalEdge = (Config.IsYAxisUp) ? ((int)RectangleGraphEdges.LEFT_UP) : ((int)RectangleGraphEdges.LEFT_DOWN);
            int leftDiagonalOppositeEdge = GetOppositeEdge(leftDiagonalEdge);

            for (int c = 0; c < ColumnCount; c++)
            {
                for (int r = 0; r < RowCount; r++)
                {
                    if (c < ColumnCount - 1 && r < RowCount - 1)
                    {
                        CreateEdge(GetNodeByLocation(c, r), GetNodeByLocation(c + 1, r + 1), rightDiagonalEdge, rightDiagonalOppositeEdge, true);
                    }

                    if (c > 0 && r < RowCount - 1)
                    {
                        CreateEdge(GetNodeByLocation(c, r), GetNodeByLocation(c - 1, r + 1), leftDiagonalEdge, leftDiagonalOppositeEdge, true);
                    }
                }
            }
        }

    }
}
