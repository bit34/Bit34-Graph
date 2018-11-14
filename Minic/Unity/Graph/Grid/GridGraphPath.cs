namespace Minic.Unity.Graph.Grid
{
    public class GridGraphPath<TNodeData, TNode> : GraphPath<TNodeData, TNode>
        where TNodeData : new()
        where TNode : GridGraphNode<TNodeData>, new()
    { }
}
