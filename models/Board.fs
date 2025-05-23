module Models.Board

/// Represents a player
type Player = 
    | X 
    | O

/// Represents the state of a cell
type Cell = 
    | Empty 
    | Taken of Player

/// The Gomoku board is a 15x15 grid size
type Board = Cell[,]

/// Create a new empty board
let createBoard () : Board =
    Array2D.create 15 15 Empty

/// Print the board to the console
let printBoard (board: Board) =
    printfn "\n    %s" (String.concat " " [for i in 0 .. 14 -> sprintf "%2d" i])
    for y in 0 .. 14 do
        printf "%2d |" y
        for x in 0 .. 14 do
            match board.[x, y] with
            | Empty -> printf "%3s" "."
            | Taken X -> printf "%3s" "X"
            | Taken O -> printf "%3s" "O"
        printfn ""
