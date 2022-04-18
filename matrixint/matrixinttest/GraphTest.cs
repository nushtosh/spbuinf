using System;
using System.IO;
using Graph;
using matrixint;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class GraphTest
    {
        private static readonly string FilesPath = $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}\\test_data";
        private static readonly string FilesPathRes = $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}\\test_res";

        [Test]
        public void IntGraph()
        {
            var tmpDotFile = $"{FilesPathRes}\\graph.dot";
            var tmpPdfFile = $"{FilesPathRes}\\graph.pdf";

            var rw = new ReaderWriter<int>();
            var matrix = rw.ReadFile($"{FilesPath}\\adjacency_matrix.txt");

            Console.WriteLine("Adjacency matrix:");
            GraphHelper.PrintMatrix(matrix);

            var graph = GraphHelper.CreateGraph(matrix);
            var tryFunc = GraphHelper.ShortestPathDijkstra(graph, matrix, 0);
            GraphHelper.CreateDotFileWithShortPath(tmpDotFile, graph, tryFunc);
            GraphHelper.DotToPdf(tmpDotFile, tmpPdfFile);
        }


        [Test]
        public void ShortestPathMatrixTest()
        {
            var rw = new ReaderWriter<int>();
            var matrix = rw.ReadFile($"{FilesPath}\\adjacency_matrix.txt");


            Console.WriteLine("Adjacency matrix:");
            GraphHelper.PrintMatrix(matrix);

            var shortestPathMatrix = GraphHelper.FloydWarshall(matrix);
            Console.WriteLine("Shortest path matrix:");
            GraphHelper.PrintMatrix(shortestPathMatrix);
        }
    }
}