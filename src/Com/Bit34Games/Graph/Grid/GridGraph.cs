﻿using Com.Bit34Games.Graph.Generic;


namespace Com.Bit34Games.Graph.Grid
{
    public abstract class GridGraph<TNode, TEdge> : Graph<TNode, TEdge>
        where TNode : GridGraphNode
        where TEdge : GridGraphEdge
    {
        //  MEMBERS
        private readonly GridGraphConfig _gridConfig;


        //  CONSTRUCTORS
        public GridGraph(GridGraphConfig gridConfig, IGraphAllocator<TNode, TEdge> allocator) :
            base(gridConfig, allocator)
        {
            _gridConfig = gridConfig;
        }


        //  METHODS
        public int GetOppositeEdge(int edge)
        {
            return (edge + (_gridConfig.staticEdgeCount / 2)) % _gridConfig.staticEdgeCount;
        }

    }
}
