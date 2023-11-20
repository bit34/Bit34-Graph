namespace Com.Bit34Games.Graphs
{
    public class HexGraph<TConfig, TNode, TEdge> : GridGraph<TConfig, TNode, TEdge>
        where TConfig : HexGraphConfig<TNode, TEdge>
        where TNode : HexNode<TEdge>
        where TEdge : HexEdge
    {
        //  MEMBERS
        public readonly int columnCount;
        public readonly int rowCount;
        //      Internal
        private TNode[] _nodes;


        //  CONSTRUCTOR(S)
        public HexGraph(TConfig                       config,
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
            int horizontalEdge         = (int)HexEdgeDirections.RIGHT;
            int horizontalOppositeEdge = GetOppositeEdge(horizontalEdge);

            for (int c = 0; c < columnCount; c++)
            {
                for (int r = 0; r < rowCount; r++)
                {
				    //	horizontal edges
                    if (c < columnCount - 1)
                    {
                        AddEdge(GetNodeByLocation(c, r), GetNodeByLocation(c + 1, r), horizontalEdge, horizontalOppositeEdge, true);
                    }

                    //	vertical edges
                    if ( r < rowCount - 1 )
                    {
                        //	on even rows
                        if ( (r & 0x01) == 0 )
                        {
                            AddEdge( GetNodeByLocation(c, r), GetNodeByLocation(c, r + 1), (int)HexEdgeDirections.RIGHT_DOWN, (int)HexEdgeDirections.LEFT_UP, true );
                            if ( c > 0 )
                            {
                                AddEdge( GetNodeByLocation(c, r), GetNodeByLocation(c - 1, r + 1), (int)HexEdgeDirections.LEFT_DOWN, (int)HexEdgeDirections.RIGHT_UP, true );
                            }
                        }
                        //	on odd rows
                        else
                        {
                            AddEdge( GetNodeByLocation(c, r), GetNodeByLocation(c, r + 1), (int)HexEdgeDirections.LEFT_DOWN, (int)HexEdgeDirections.RIGHT_UP, true );
                            if ( c<columnCount-1 )
                            {
                                AddEdge( GetNodeByLocation(c, r), GetNodeByLocation(c + 1, r + 1), (int)HexEdgeDirections.RIGHT_DOWN, (int)HexEdgeDirections.LEFT_UP, true );
                            }
                        }
                    }
                }
            }
        }

    }
}
