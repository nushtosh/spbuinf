module M_G

open System
open F_matrix
open Argu
open mTypes
open System.IO

let MAX_VALUE_INT = 1000
let MAX_VALUE_DOUBLE = 1000.0
let MAX_VALUE_BOOLEAN = true

type Arguments =
    | [<Mandatory>] Path of path:string
    | [<Mandatory>] Size of size:int
    | [<Mandatory>] Quant of quantity:int
    | [<Mandatory>]Type of types: mtypes
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Path _ -> "specify a working directory."
            | Size _ -> "specify size of a matrix to be created."
            | Quant _ -> "quantity of generated matrices."
            | Type _ -> "type of the following matrix(int, bool, double, extended)."

let parser = ArgumentParser.Create<Arguments>(programName = "M_G")

let toString et =
    match et with
    | INT -> "int"
    | BOOL -> "boolean"
    | DOUBLE -> "double"
    | EXTENDED -> "extended"

let usage = parser.PrintUsage()

let generateMatrix argv =
    try
        let options = parser.ParseCommandLine argv
        let path, size, quantity, types =
            options.GetResult Path, options.GetResult Size, options.GetResult Quant, options.GetResult Type
        
        let path = Path.Combine [|path; toString types; string size|]
        (ignore << Directory.CreateDirectory) path
        let path = Path.Combine (path, "matrix")
        for i = 1 to quantity do
            match types with
            | INT -> 
                Matrix.writeFile 
                    (Matrix.stringify (
                        Matrix.generateMatrix size size MAX_VALUE_INT RandomGenerators.randomInt
                    ))
                    path
                    types
                    size
                    $"matrix_{i}"
                |> ignore
            | BOOL -> 
                Matrix.writeFile 
                    (Matrix.stringify (
                        Matrix.generateMatrix size size MAX_VALUE_BOOLEAN RandomGenerators.randomBool
                    ))
                    path
                    types
                    size
                    $"matrix_{i}"
                |> ignore
            | DOUBLE -> 
                Matrix.writeFile 
                    (Matrix.stringify (
                        Matrix.generateMatrix size size MAX_VALUE_DOUBLE RandomGenerators.randomDouble
                    ))
                    path
                    types
                    size
                    $"matrix_{i}"
                |> ignore
            | EXTENDED -> 
                Matrix.writeFile 
                    (Matrix.stringify (
                        Matrix.generateMatrix size size MAX_VALUE_DOUBLE RandomGenerators.randomExtendedDouble
                    ))
                    path
                    types
                    size
                    $"matrix_{i}"
                |> ignore
    with e -> eprintfn "%s" e.Message
    0