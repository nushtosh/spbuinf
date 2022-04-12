// Learn more about F# at http://fsharp.org

open System
open F_matrix.Matrix
open F_matrix.Generic
open F_matrix.Graph

[<EntryPoint>]
let main argv =
    try
        let matrixA = "C://Users//olya//source//repos//F_matrix//F_test//test_data//matrix.txt"
        let dotpath="C://Users//olya//source//repos//F_matrix//F_test//test_data//res.dot"
        let rpath = "C://Users//olya//source//repos//F_matrix//F_test//test_data"
        let a = readFromFile matrixA Convert<int>
        let ag= adjacencyGraph a
        let trans=transitiveClosure a
        createDotFileTrc dotpath a trans
        let result=exportToPdf dotpath rpath 
        0

    with e -> 
        printfn "%A" e
        -1
