module Mailboxes.Actions

open System
open System.IO
open Mailboxes.Types


let createMatrix (cmd: CreateCommand) : MatrixInstance =
    let matrixStr =
        match cmd.Type with
        | Type.Int ->
            F_matrix.Matrix.toStringMatrix (
                F_matrix.Matrix.generateMatrix cmd.Size cmd.Size 1000 F_matrix.RandomGenerators.randomInt
            )
        | Type.Bool ->
            F_matrix.Matrix.toStringMatrix (
                F_matrix.Matrix.generateMatrix cmd.Size cmd.Size true F_matrix.RandomGenerators.randomBool
            )
        | Type.Double ->
            F_matrix.Matrix.toStringMatrix (
                F_matrix.Matrix.generateMatrix cmd.Size cmd.Size 1000.0 F_matrix.RandomGenerators.randomDouble
            )
        | Type.Extended ->
            F_matrix.Matrix.toStringMatrix (
                F_matrix.Matrix.generateMatrix
                    cmd.Size
                    cmd.Size
                    1000.0
                    F_matrix.RandomGenerators.randomExtendedDouble
            )

    { Content = matrixStr
      _type = cmd.Type
      size = cmd.Size }


let readMatrix (cmd: ReadCommand) : MatrixInstance =
    let matrix =
        $"{cmd.Path}{Path.DirectorySeparatorChar}{cmd.FileName}"
        |> File.ReadAllText
        |> F_matrix.Matrix.parseMatrix

    { Content = matrix
      _type = cmd.Type
      size = matrix.Length }


let writeMatrix (cmd: WriteCommand) (cache: Map<string, MatrixInstance>) =
    let mx = cache.[cmd.Name]
    let matrixStr = F_matrix.Matrix.stringify mx.Content

    let path =
        $"{cmd.Path}{Path.DirectorySeparatorChar}{cmd.Name}"

    F_matrix.Matrix.writePlain matrixStr path
    path



let _innerMultiplyMatrix<'T> (cmd: MultiplyCommand) (cache: Map<string, MatrixInstance>) : MatrixInstance =
    let mx1 = cache.[cmd.LeftName]
    let mx2 = cache.[cmd.RightName]

    let m1 =
        F_matrix.Matrix.convert<'T> mx1.Content

    let m2 =
        F_matrix.Matrix.convert<'T> mx2.Content

    let mxRes = F_matrix.Matrix.mulMatrix<'T> m1 m2
    let mxResStr = F_matrix.Matrix.toStringMatrix mxRes

    { Content = mxResStr
      _type = cmd.Type
      size = mxResStr.Length }


let multiplyMatrix (cmd: MultiplyCommand) (cache: Map<string, MatrixInstance>) : MatrixInstance =
    match cmd.Type with
    | Type.Int -> _innerMultiplyMatrix<int> cmd cache
    | Type.Bool -> _innerMultiplyMatrix<bool> cmd cache
    | Type.Double -> _innerMultiplyMatrix<double> cmd cache
    | Type.Extended -> _innerMultiplyMatrix<double> cmd cache


let findTrc (cmd: FindTrcCommand) (cache: Map<string, MatrixInstance>) : MatrixInstance =
    let mx = cache.[cmd.Name]

    let res =
        match mx._type with
        | Type.Int ->
            mx.Content
            |> F_matrix.Matrix.convert<int>
            |> F_matrix.Matrix.transitiveClosure
        | Type.Bool ->
            mx.Content
            |> F_matrix.Matrix.convert<bool>
            |> F_matrix.Matrix.transitiveClosure
        | Type.Double ->
            mx.Content
            |> F_matrix.Matrix.convert<double>
            |> F_matrix.Matrix.transitiveClosure
        | Extended ->
            mx.Content
            |> F_matrix.Matrix.convert<double>
            |> F_matrix.Matrix.transitiveClosure


    { Content = F_matrix.Matrix.toStringMatrix (res)
      _type = Type.Bool
      size = res.Length }