﻿namespace Com.Bit34Games.Graphs
{
    public class Path
    {
        //  MEMBERS
        public readonly int startNodeId;
        public readonly int endNodeId;
        public int          EdgeCount { get { return _edges.Length; } }
        //      private
        private Edge[] _edges;


        //  CONSTRUCTOR(S)
        public Path(int    startNodeId, 
                    int    endNodeId,
                    Edge[] edges)
        {
            this.startNodeId = startNodeId;
            this.endNodeId   = endNodeId;
            _edges           = edges;
        }


        //  METHODS
        public Edge GetEdge(int index)
        {
            return _edges[index];
        }

    }
}
