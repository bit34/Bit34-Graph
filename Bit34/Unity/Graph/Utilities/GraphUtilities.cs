using System.Collections.Generic;
using UnityEngine;
using Bit34.Unity.Graph.Base;


namespace Bit34.Unity.Graph.Utilities
{
    public static class GraphUtilities
    {
        static public void DrawStaticEdges<TNode, TEdge>(Graph<TNode, TEdge> graph, Material edgeMaterial, Matrix4x4 matrix)
            where TNode : GraphNode
            where TEdge : GraphEdge
        {
            edgeMaterial.SetPass(0);
            GL.Begin(GL.LINES);

            IEnumerator<TNode> nodes = graph.GetNodeEnumerator();
            while (nodes.MoveNext() == true)
            {
                TNode node = nodes.Current;

                for(int i=0; i<node.StaticEdgeCount; i++)
                {
                    GraphEdge edge = node.GetStaticEdge(i);
                    
                    if(edge!=null)
                    {
                        GL.Vertex( matrix.MultiplyPoint( graph.GetNodeById(edge.SourceNodeId).Position ) );
                        GL.Vertex( matrix.MultiplyPoint( graph.GetNodeById(edge.TargetNodeId).Position ) );
                    }
                }
            }

            GL.End();
        }

        static public void DrawDynamicEdges<TNode, TEdge>(Graph<TNode, TEdge> graph, Material edgeMaterial, Matrix4x4 matrix)
            where TNode : GraphNode
            where TEdge : GraphEdge
        {
            edgeMaterial.SetPass(0);
            GL.Begin(GL.LINES);

            IEnumerator<TNode> nodes = graph.GetNodeEnumerator();
            while (nodes.MoveNext() == true)
            {
                TNode node = nodes.Current;

                IEnumerator<GraphEdge>edges = node.GetDynamicEdgeEnumerator();
                while(edges.MoveNext())
                {
                    GraphEdge edge = edges.Current;
                    
                    GL.Vertex( matrix.MultiplyPoint( graph.GetNodeById(edge.SourceNodeId).Position ) );
                    GL.Vertex( matrix.MultiplyPoint( graph.GetNodeById(edge.TargetNodeId).Position ) );
                }
            }

            GL.End();
        }

        static public void DrawPath<TNode, TEdge>(Graph<TNode, TEdge> graph, GraphPath path, Material edgeMaterial, Matrix4x4 matrix)
            where TNode : GraphNode
            where TEdge : GraphEdge
        {
            edgeMaterial.SetPass(0);
            GL.Begin(GL.LINES);

            IEnumerator<GraphEdge> edges = path.Edges.GetEnumerator();
            while (edges.MoveNext() == true)
            {
                GraphEdge edge = edges.Current;

                GL.Vertex( matrix.MultiplyPoint( graph.GetNodeById(edge.SourceNodeId).Position ) );
                GL.Vertex( matrix.MultiplyPoint( graph.GetNodeById(edge.TargetNodeId).Position ) );
            }

            GL.End();
        }
    }
}