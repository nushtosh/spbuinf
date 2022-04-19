module F_matrix.Matrix


open System
open System.IO
open System.Text

type Types =
    | Int of int
    | Double of double
    | Boolean of bool

let private arrayToString array =
    let concat acc x = if acc = "" then (string x) else acc + ", " + (string x)
    Array.fold concat "" array


let mulMatrix<'T> (matrix1: 'T[,]) (matrix2: 'T[,]) =
    let result_row = matrix1.GetLength 0
    let result_column = matrix2.GetLength 1
    
    let ret = Array2D.create result_row result_column Unchecked.defaultof<'T>

    for i in 0 .. result_row - 1 do
        for j in 0 .. result_column - 1 do
            let mutable acc = Unchecked.defaultof<'T>

            for k in 0 .. (matrix1.GetLength 1) - 1 do
                let a = matrix1.[i, k]
                let b = matrix2.[k, j]
                let tmpMul = Generic.Mul a b
                acc <- Generic.Add acc tmpMul

            ret.[i, j] <- acc
    ret
    
let transitiveClosure<'T when 'T: equality> (matrix1: 'T[,]) =
    let len = matrix1.GetLength 1
    let ret = Array2D.create len len 0
    
    for i in 0 .. len - 1 do
        for j in 0 .. len - 1 do
            if matrix1.[i,j] = Unchecked.defaultof<'T> then ret.[i,j] <- 0
            else ret.[i,j] <- 1
    
    for k in 0 .. len - 1 do
        for i in 0 .. len - 1 do
            for j in 0 .. len - 1 do
                if ret.[i,j] > 0 then ret.[i,j] <- 1
                elif ret.[i,k] > 0 && ret.[k,j] > 0 then ret.[i,j] <- 1
                else ret.[i,j] <- 0
    ret

let generateMatrix<'T, 'A> numRows numColumns (max: 'T) (randomGenerator: 'T -> 'A) =
    Array2D.init numRows numColumns (fun _ _ -> randomGenerator max)
    
let stringify<'T> (matrix: 'T[,])=
    let len1 = Array2D.length1 matrix
    let len2 = Array2D.length2 matrix
    let resStr = StringBuilder()

    for i in [0 .. len1-1] do
        for j in [0 .. len2-1] do
            resStr.Append $"%8O{matrix.[i,j]}" |> ignore
            if (j <> len2-1) then resStr.Append(" ")|>ignore
        resStr.Append(Environment.NewLine)|>ignore
    resStr.ToString()
    
let convert<'T> (matrix: string[,]) : 'T[,]=
    matrix |> Array2D.map Generic.Convert
    
let printMatrix matrix=
    printfn "%O" <| stringify matrix

let parseMatrix (str: string) =
    let lines = str.Trim(Environment.NewLine.ToCharArray()).Trim().Split(Environment.NewLine)
    let arrays = lines |> Seq.map (fun line -> line.Trim().Split " " |> Seq.map (fun e -> e.Trim()))
    let arr2d = arrays |> array2D
    
    arr2d

let toStringMatrix<'T> (matrix: 'T[,]) : string[,] =
    matrix |> Array2D.map (fun e -> $"{e}")

let writePlain (matrix: string) (path: string) =
    Directory.CreateDirectory(Path.GetDirectoryName(path)) |> ignore
    File.WriteAllText(path, matrix)

let writeFile (matrix: string) path _type size fileName =
    let s = Path.DirectorySeparatorChar
    let filePath = $"{path}{s}{_type}{s}{size}{s}{fileName}"
    writePlain matrix filePath
    printfn $"Write matrix to {filePath}"

let readFromFile(fullPathToMatrix: string) (convertFromString: string -> 'a) = 
    fullPathToMatrix
    |> File.ReadLines
    |> Seq.map (fun line -> line.Trim().Split " ")
    |> array2D
    |> Array2D.map convertFromString

let flat2Darray array2D = 
    seq { for x in [0..(Array2D.length1 array2D) - 1] do 
              for y in [0..(Array2D.length2 array2D) - 1] do 
                  yield array2D.[x, y] }
 
 