using System.Collections.Generic;


namespace Com.Bit34Games.Graph.Generic
{
    public class GraphNodeGroup
    {
        //  MEMBERS
        public int NodeCount { get { return _NodeIds.Count; } }
        private HashSet<int> _NodeIds;


        //  CONSTRUCTORS
        public GraphNodeGroup()
        {
            _NodeIds = new HashSet<int>();
        }


        //  METHODS
        public void Add(int nodeId)
        {
            _NodeIds.Add(nodeId);
        }

        public void Remove( int nodeId)
        {
            _NodeIds.Remove(nodeId);
        }

        public void Append(GraphNodeGroup group)
        {
            IEnumerator<int> nodeIds = group.GetEnumerator();
            while (nodeIds.MoveNext())
            {
                _NodeIds.Add(nodeIds.Current);
            }
        }

        public bool Has(int nodeId)
        {
            return _NodeIds.Contains(nodeId);
        }

        public void Clear()
        {
            _NodeIds.Clear();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _NodeIds.GetEnumerator();
        }

    }
}
