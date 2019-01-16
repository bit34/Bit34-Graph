using System.Collections.Generic;


namespace Bit34.Unity.Graph
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

        public void Add(GraphNodeGroup group)
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

        public IEnumerator<int> GetEnumerator()
        {
            return _NodeIds.GetEnumerator();
        }

    }
}
