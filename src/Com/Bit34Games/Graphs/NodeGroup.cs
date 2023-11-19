using System.Collections.Generic;


namespace Com.Bit34Games.Graphs
{
    public class NodeGroup
    {
        //  MEMBERS
        public int NodeCount { get { return _nodeIds.Count; } }
        private HashSet<int> _nodeIds;


        //  CONSTRUCTORS
        public NodeGroup()
        {
            _nodeIds = new HashSet<int>();
        }


        //  METHODS
        public void Add(int nodeId)
        {
            _nodeIds.Add(nodeId);
        }

        public void Remove( int nodeId)
        {
            _nodeIds.Remove(nodeId);
        }

        public void Append(NodeGroup group)
        {
            IEnumerator<int> nodeIds = group.GetEnumerator();
            while (nodeIds.MoveNext())
            {
                _nodeIds.Add(nodeIds.Current);
            }
        }

        public bool Has(int nodeId)
        {
            return _nodeIds.Contains(nodeId);
        }

        public void Clear()
        {
            _nodeIds.Clear();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _nodeIds.GetEnumerator();
        }

    }
}
