module F_test

open NUnit.Framework
open System.IO
open Expecto
open F_matrix.Matrix
open F_matrix.Generic
open F_matrix.Graph
open System.Threading
open System.Globalization

[<SetUp>]
let Setup () =
    ()

[<Test>]
let multtestint () =
    let _testDir = $"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}\\..\\..\\test_data";
    let matrixA = $"{_testDir}\\matrixint1.txt"
    let matrixB = $"{_testDir}\\matrixint2.txt"
    let expectedPath = $"{_testDir}\\res_exp.txt"

    try
        let a = readFromFile matrixA Convert<int>
        let b = readFromFile matrixB Convert<int>
        let actual = mulMatrix a b  
        let expected = readFromFile expectedPath Convert<int>

        "Actual and expected matrix should be equal"
        |> Expect.sequenceEqual
            (actual |> flat2Darray)
            (expected |> flat2Darray)

    
    with e -> 
        printfn "%A" e

[<Test>]
let multtestdouble () =
    let _testDir = $"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}\\..\\..\\test_data";
    let matrixA = $"{_testDir}\\matrixdouble1.txt"
    let matrixB = $"{_testDir}\\matrixdouble2.txt"
    let expectedPath = $"{_testDir}\\res_double.txt"

    try
        let a = readFromFile matrixA Convert<double>
        let b = readFromFile matrixB Convert<double>
        let actual = mulMatrix a b  
        let expected = readFromFile expectedPath Convert<double>

        "Actual and expected matrix should be equal"
        |> Expect.sequenceEqual
            (actual |> flat2Darray)
            (expected |> flat2Darray)

    with e -> 
        printfn "%A" e

[<Test>]
let multtestbool () =
    let _testDir = $"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}\\..\\..\\test_data";
    let matrixA = $"{_testDir}\\matrixbool1.txt"
    let matrixB = $"{_testDir}\\matrixbool2.txt"
    let expectedPath = $"{_testDir}\\res_bool.txt"

    try
        let a = readFromFile matrixA Convert<bool>
        let b = readFromFile matrixB Convert<bool>
        let actual = mulMatrix a b  
        let expected = readFromFile expectedPath Convert<bool>

        "Actual and expected matrix should be equal"
        |> Expect.sequenceEqual
            (actual |> flat2Darray)
            (expected |> flat2Darray)

    with e -> 
        printfn "%A" e

[<Test>]
let graphpdftest () =
    let _testDir = $"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}\\..\\..\\test_data";
    let _DotPath = $"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}\\..\\..\\test_data\\res.dot";
    let _PDFPath = $"{Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)}\\..\\..\\test_data\\res.pdf";
    let matrix = $"{_testDir}\\matrix.txt"

    try
        let a = readFromFile matrix Convert<int>
        let ag= adjacencyGraph a
        let trans=transitiveClosure a
        createDotFileTrc _DotPath a trans
        let result=exportToPdf _DotPath _testDir 
        Expect.isTrue (File.Exists _PDFPath) "Pdf should exists"


    with e -> 
        printfn "%A" e

