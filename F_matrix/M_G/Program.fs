open M_G


[<EntryPoint>]
let main argv =
    try
        generateMatrix argv
        0

    with
    | e ->
        printfn "%A" e
        -1