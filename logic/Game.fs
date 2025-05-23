module Logic.Game

open System
open Models.Board

/// Switch between players
let switchPlayer player =
    match player with
    | X -> O
    | O -> X

let getEmptyCells (board: Cell[,]) =
    [ for x in 0 .. 14 do
        for y in 0 .. 14 do
            if board.[x, y] = Empty then yield (x, y) ]
// Random number generator for CPU
let random = Random()

//get a random move for the CPU from available empty cells
let getCPUMove (board: Cell[,]) =
    let emptyCells = getEmptyCells board
    if emptyCells.Length > 0 then
        emptyCells.[random.Next(emptyCells.Length)]
    else
        (0, 0) // fallback in case the board is full

/// Place a move on the board if the cell is empty
let placeMove (board: Board) (x: int, y: int) (player: Player) =
    if board.[x, y] = Empty then
        board.[x, y] <- Taken player
        true
    else
        false

/// Check for 5 in a row in any direction
let checkWin (board: Board) (player: Player) =
    let directions = [ (1,0); (0,1); (1,1); (1,-1) ]

    // recursive function counts the number of same players makrks in a direction
    let rec countInDirection (x, y) (dx, dy) count =
        if x < 0 || y < 0 || x > 14 || y > 14 || board.[x, y] <> Taken player then
            count
        else
            countInDirection (x + dx, y + dy) (dx, dy) (count + 1)

    //decide whether the player made 5 pieces in a raw
    let hasFiveInARow (x, y) =
        directions
        |> List.exists (fun (dx, dy) ->
            let count =
                (countInDirection (x - dx, y - dy) (-dx, -dy) 0) +  // backward
                (countInDirection (x, y) (dx, dy) 0)               // forward
            count >= 5
        )

    //serarch and catch the wining position
    [ for x in 0 .. 14 do
        for y in 0 .. 14 do
            if board.[x, y] = Taken player && hasFiveInARow (x, y) then
                yield true ]
    |> List.contains true

/// Check if the game is a draw (no empty cells)
let checkDraw (board: Board) =
    board
    |> Seq.cast<Cell>
    |> Seq.forall (function Empty -> false | _ -> true)

/// Ask the user if they want to play again
let rec askReplay () =
    printfn "\nDo you want to play again?"
    printfn "1. Yes"
    printfn "2. No"
    match System.Console.ReadLine() with
    | "1" -> true
    | "2" ->
        printfn "Thanks for playing. Bye!"
        Environment.Exit(0)
        false //this satisfys the function with the return type
    | _ ->
        printfn "Invalid input. Please enter 1 or 2."
        askReplay ()
