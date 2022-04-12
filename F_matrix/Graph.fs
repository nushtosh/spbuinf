module F_matrix.Graph


open QuickGraph
open QuickGraph.Algorithms
open System.IO
open System.Diagnostics
open System.Collections.Generic


let shortestPathDijkstra (ag: AdjacencyGraph<int, Edge<int>>) (mx: _[,]) fromVertex = 
    ag.ShortestPathsDijkstra((fun edge -> mx.[edge.Source, edge.Target]), fromVertex)

let exportToPdf (pathToDot: string) (pathToOutputDir: string) = 
   let filename = Path.GetFileNameWithoutExtension pathToDot
   let outputFile = Path.GetFullPath <| Path.Combine [| pathToOutputDir; filename + ".pdf" |]
   let pInfo = new ProcessStartInfo()
   pInfo.FileName <- "dot"
   pInfo.Arguments  <- sprintf "-Tpdf -o %s %s" outputFile pathToDot
   use p = Process.Start(pInfo)
   p.WaitForExit(0) |> ignore
   outputFile

let adjacencyGraph (mx: _[,]) =
   let len = mx.GetLength 0
   let ag = AdjacencyGraph<int, Edge<int>>()
   for i in 0 .. len - 1 do
         for j in 0 .. len - 1 do
             if mx.[i,j] > 0 then ag.AddVerticesAndEdge(Edge(i,j)) |> ignore
   ag

 
    
let createDotFileTrc (filename: string) matrix (trc:int[,]) =
    use writer = new StreamWriter(filename)

    fprintfn writer "digraph G {"
    Array2D.iteri (fun i j v ->
        if v>0 then
            fprintfn writer "\t%i -> %i;" i j
        elif trc.[i, j]=1 then
            fprintfn writer "\t%i -> %i  [color=red];" i j
    ) matrix
    fprintfn writer "}"