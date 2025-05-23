open Models.Board
open Models.Menu
open Logic.Game

// Prompts the player to enter a mave
//it checks and validates the player input
let rec getMove player =
    printfn "\nPlayer %A, enter your move as X Y (0–14):" player
    match System.Console.ReadLine().Split() |> Array.toList with
    | [ xStr; yStr ] ->
        match System.Int32.TryParse(xStr), System.Int32.TryParse(yStr) with
        | (true, x), (true, y) when x >= 0 && x <= 14 && y >= 0 && y <= 14 -> (x, y)
        | _ -> 
            printfn "Invalid range or non-numeric input. Try again."
            getMove player
    | _ ->
        printfn "Invalid input. Please use the format: X Y"
        getMove player

//This starts a two-player game loop
let rec twoPlayerGame () =
    let board = createBoard () // creates a new empty board
    let rec loop currentPlayer =
        printBoard board // shows the current board state
        let move = getMove currentPlayer // this gets the player's move
        if placeMove board move currentPlayer then
            if checkWin board currentPlayer then
                printBoard board
                printfn "\nPlayer %A wins!" currentPlayer
                if askReplay () then twoPlayerGame ()
            elif checkDraw board then
                printBoard board
                printfn "\nThe game is a draw!"
                if askReplay () then twoPlayerGame ()
            else
                loop (switchPlayer currentPlayer)
        else
            printfn "Cell already taken. Try again."
            loop currentPlayer
    loop X

// starts a single-player against CPU
let rec singlePlayerGame () =
    let board = createBoard ()
    let rec loop currentPlayer =
        printBoard board
        let move =
            if currentPlayer = X then getMove currentPlayer //Human's turn
            else
                printfn "\nCPU is thinking..."
                System.Threading.Thread.Sleep(500) //Simulate the CPU thinking time
                getCPUMove board //CPU chooses a move
        if placeMove board move currentPlayer then
            if checkWin board currentPlayer then
                printBoard board
                if currentPlayer = X then
                    printfn "\nYou win!"
                else
                    printfn "\nCPU wins!"
                if askReplay () then singlePlayerGame ()
            elif checkDraw board then
                printBoard board
                printfn "\nThe game is a draw!"
                if askReplay () then singlePlayerGame ()
            else
                loop (switchPlayer currentPlayer)
        else
            if currentPlayer = X then
                printfn "Cell already taken. Try again."
            loop currentPlayer
    loop X //player X always starts first

// helper to map user input to MenuChoice
let parseMenuChoice input =
    match input with
    | "1" -> Some SinglePlayer
    | "2" -> Some TwoPlayer
    | "3" -> Some Exit
    | _ -> None

//Main menu display to prompt the user to choose a game mode
let rec mainMenu () =
    printfn "\nWelcome to the Gomoku Game!"
    printfn "Please enter a number to start (1, 2 or 3):"
    printfn "1. Single-player"
    printfn "2. Two-player"
    printfn "3. Exit"
    match parseMenuChoice (System.Console.ReadLine()) with
    | Some SinglePlayer -> singlePlayerGame (); mainMenu ()
    | Some TwoPlayer -> twoPlayerGame (); mainMenu ()
    | Some Exit ->
        printfn "Exiting game. Goodbye!"
    | None ->
        printfn "Invalid option."
        mainMenu ()

//Entry point of the game application
[<EntryPoint>]
let main _ =
    mainMenu ()
    0
