module F_matrix.Graph


open QuickGraph
open QuickGraph.Algorithms
open System.IO
open System.Diagnostics
open System.Collections.Generic
open System


let shortestPathDijkstra (ag: AdjacencyGraph<int, Edge<int>>) (mx: _[,]) fromVertex = 
    ag.ShortestPathsDijkstra((fun edge -> mx.[edge.Source, edge.Target]), fromVertex)


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

let runProc filename args= 
    let procStartInfo = 
        ProcessStartInfo(
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            FileName = filename,
            Arguments = args
        )

    let outputs = System.Collections.Generic.List<string>()
    let errors = System.Collections.Generic.List<string>()
    let outputHandler f (_sender:obj) (args:DataReceivedEventArgs) = f args.Data
    let p = new Process(StartInfo = procStartInfo)
    p.OutputDataReceived.AddHandler(DataReceivedEventHandler (outputHandler outputs.Add))
    p.ErrorDataReceived.AddHandler(DataReceivedEventHandler (outputHandler errors.Add))
    let started = 
        try
            p.Start()
        with | ex ->
            reraise()
    if not started then
        failwithf "Failed to start process %s" filename
    printfn "Started %s with pid %i" 
    p.BeginOutputReadLine()
    p.BeginErrorReadLine()
    p.WaitForExit()
    printfn "Finished %s" filename 
    let cleanOut l = l |> Seq.filter (fun o -> String.IsNullOrEmpty o |> not)
    cleanOut outputs,cleanOut errors



let exportToPdf (pathToDot: string) (pathToOutputDir: string) = 
   let filename = Path.GetFileNameWithoutExtension pathToDot
   let outputFile = Path.GetFullPath <| Path.Combine [| pathToOutputDir; filename + ".pdf" |]
   runProc "dot" (sprintf "-Tpdf -o %s %s" outputFile pathToDot)
   outputFile