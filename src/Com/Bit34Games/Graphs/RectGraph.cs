namespace Com.Bit34Games.Graphs
{
    public class RectGraph<TConfig, TNode, TConnection> : GridGraph<TConfig, TNode, TConnection>
        where TConfig : RectGraphConfig<TNode, TConnection>
        where TNode : RectGraphNode<TConnection>
        where TConnection : RectGraphConnection
    {
        //  MEMBERS
        public readonly int columnCount;
        public readonly int rowCount;
        //      Internal
        private TNode[] _nodes;


        //  CONSTRUCTOR(S)
        public RectGraph(TConfig                             config,
                         IGraphAllocator<TNode, TConnection> allocator,
                         int                                 columnCount,
                         int                                 rowCount) :
            base(config, allocator)
        {
            this.columnCount = columnCount;
            this.rowCount    = rowCount;

            CreateNodes();
            CreateConnections();

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

        private void CreateConnections()
        {
            if (Config.hasStraightConnections == true)
            {
                CreateStraightConnections();
            }

            if (Config.hasDiagonalConnections == true)
            {
                CreateDiagonalConnections();
            }
        }

        private void CreateStraightConnections()
        {
            int horizontalConnection         = (int)RectGraphConnections.RIGHT;
            int horizontalOppositeConnection = GetOppositeConnection(horizontalConnection);

            int verticalConnection         = (Config.isYAxisUp) ? ((int)RectGraphConnections.UP) : ((int)RectGraphConnections.DOWN);
            int verticalOppositeConnection = GetOppositeConnection(verticalConnection);

            for (int c = 0; c < columnCount; c++)
            {
                for (int r = 0; r < rowCount; r++)
                {
                    if (c < columnCount - 1)
                    {
                        AddConnection(GetNodeByLocation(c, r), GetNodeByLocation(c + 1, r), horizontalConnection, horizontalOppositeConnection, true);
                    }

                    if (r < rowCount - 1)
                    {
                        AddConnection(GetNodeByLocation(c, r), GetNodeByLocation(c, r + 1), verticalConnection, verticalOppositeConnection, true);
                    }
                }
            }
        }

        private void CreateDiagonalConnections()
        {
            int rightDiagonalConnection         = (Config.isYAxisUp) ? ((int)RectGraphConnections.RIGHT_UP) : ((int)RectGraphConnections.RIGHT_DOWN);
            int rightDiagonalOppositeConnection = GetOppositeConnection(rightDiagonalConnection);

            int leftDiagonalConnection         = (Config.isYAxisUp) ? ((int)RectGraphConnections.LEFT_UP) : ((int)RectGraphConnections.LEFT_DOWN);
            int leftDiagonalOppositeConnection = GetOppositeConnection(leftDiagonalConnection);

            for (int c = 0; c < columnCount; c++)
            {
                for (int r = 0; r < rowCount; r++)
                {
                    if (c < columnCount - 1 && r < rowCount - 1)
                    {
                        AddConnection(GetNodeByLocation(c, r), GetNodeByLocation(c + 1, r + 1), rightDiagonalConnection, rightDiagonalOppositeConnection, true);
                    }

                    if (c > 0 && r < rowCount - 1)
                    {
                        AddConnection(GetNodeByLocation(c, r), GetNodeByLocation(c - 1, r + 1), leftDiagonalConnection, leftDiagonalOppositeConnection, true);
                    }
                }
            }
        }

    }
}
