using System.Diagnostics;
using QuickGraph;
using QuickGraph.Algorithms;
using System;
using System.Linq;
using System.Data;
using System.IO;
using System.Collections.Generic;

namespace Graph
{
    public static class GraphHelper
    {
        public static AdjacencyGraph<int, Edge<int>> CreateGraph(int[][] mx)
        {
            var ag = new AdjacencyGraph<int, Edge<int>>();

            for (var i = 0; i < mx.Length; i++)
            {
                for (var j = 0; j < mx[0].Length; j++)
                {
                    if (mx[i][j]!=0)
                    {
                        ag.AddVerticesAndEdge(new Edge<int>(i, j));
                    }
                }
            }

            return ag;
        }

        public static TryFunc<int, IEnumerable<Edge<int>>> ShortestPathDijkstra<T>(AdjacencyGraph<int, Edge<int>> ag,
            int[][] mx, int fromVertex)
        {
            return ag.ShortestPathsDijkstra((edge) => mx[edge.Source][edge.Target], fromVertex);
        }

        public static void CreateDotFileWithShortPath(string filePath, AdjacencyGraph<int, Edge<int>> ag,
            TryFunc<int, IEnumerable<Edge<int>>> tryGetPath)
        {
            var shortestPath = new HashSet<Edge<int>>();
            var vxs = ag.Vertices;
            var edges = ag.Edges;

            foreach (var vx in vxs)
            {
                IEnumerable<Edge<int>> tmpEdges;
                tryGetPath(vx, out tmpEdges);
                if (tmpEdges != null)
                {
                    shortestPath.UnionWith(tmpEdges);
                }
            }


            var edgesString = edges
                .Select(edge => shortestPath.Contains(edge) ? $"{edge} [color=red]" : edge.ToString())
                .Aggregate("", (acc, cur) => $"{acc}{cur};");
            var dot = $"digraph g {{{edgesString}}}";

            File.WriteAllText(filePath, dot);

            Console.WriteLine($"File {filePath} was created");
        }

        public static void CreateDotFile(string filePath, AdjacencyGraph<int, Edge<int>> ag)
        {
            var vxs = ag.Vertices;
            var edges = ag.Edges;

            var edgesString = edges
                .Select(edge => edge.ToString())
                .Aggregate("", (acc, cur) => $"{acc}{cur};");
            var dot = $"digraph g {{{edgesString}}}";

            File.WriteAllText(filePath, dot);

            Console.WriteLine($"File {filePath} was created");
        }

        /**
         * dot(GraphViz) need to be installed locally
         */
        public static void DotToPdf(string from, string to)
        {
            var p = new Process();
            p.StartInfo.FileName = "dot";
            p.StartInfo.Arguments = $"-Tpdf {from} -o {to}";
            p.Start();

            Console.WriteLine($"File {to} was created");
        }

        public static void PrintMatrix<T>(T[][] matrix)
        {
            Console.WriteLine(matrix.Select(
                    ints =>
                        ints.Select(
                            i => $"{i}"
                        ).Aggregate((acc, cur) => $"{acc}, {cur}"))
                .Aggregate((acc, cur) => $"{acc}{Environment.NewLine}{cur}") + Environment.NewLine);
        }

        public static int[][] TransitiveClosure<T>(int[][] matrix)
        {
            var len = matrix.Length;
            var res = new int[len][];
            for (var i = 0; i < len; i++)
            {
                res[i] = new int[len];
                for (var j = 0; j < len; j++)
                {
                    res[i][j] = (matrix[i][j]==0) ? 0 : 1;
                }
            }

            for (var k = 0; k < len; k++)
                for (var i = 0; i < len; i++)
                    for (var j = 0; j < len; j++)
                        res[i][j] = res[i][j] > 0 ? 1 : ((res[i][k] > 0) && (res[k][j] > 0)) ? 1 : 0;

            return res;
        }

        // mutate matrix
        public static double[][] FloydWarshall(int[][] matrix)
        {
            var len = matrix.Length;
            var res = new double[matrix.Length][];

            for (var i = 0; i < len; i++)
            {
                res[i] = new double[matrix[i].Length];
                for (var j = 0; j < len; j++)
                {
                    if (i == j)
                    {
                        res[i][j] = 0;
                    }
                    else if (matrix[i][j]==0)
                    {
                        res[i][j] = Double.PositiveInfinity;
                    }
                    else
                    {
                        res[i][j] = matrix[i][j];
                    }
                }
            }


            for (var k = 0; k < len; k++)
            {
                for (var i = 0; i < len; i++)
                {
                    for (var j = 0; j < len; j++)
                    {
                        if (res[i][j] > res[i][k] + res[k][j])
                        {
                            res[i][j] = res[i][k] + res[k][j];
                        }
                    }
                }
            }

            return res;
        }
    }
}