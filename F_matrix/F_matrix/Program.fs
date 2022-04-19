// Learn more about F# at http://fsharp.org

open System
open F_matrix.Matrix
open F_matrix.Generic
open F_matrix.Graph
open System.IO

[<EntryPoint>]
let main argv =
    try
        let _testDir = $"{Path.GetDirectoryName(Environment.CurrentDirectory)}\\..\\..\\test_data";
        let matrixA = $"{_testDir}\\matrix.txt"
        let dotpath= $"{_testDir}\\res.dot"
        let rpath = $"{_testDir}"
        let a = readFromFile matrixA Convert<int>
        let ag= adjacencyGraph a
        let trans=transitiveClosure a
        createDotFileTrc dotpath a trans
        let result=exportToPdf dotpath rpath 
        0

    with e -> 
        printfn "%A" e
        -1
