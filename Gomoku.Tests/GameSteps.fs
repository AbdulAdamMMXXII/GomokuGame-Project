namespace Gomoku.Tests

open TickSpec
open FsUnit.Xunit // For assertions in step definitions
open Models.Board // Access to Board types and functions
open Logic.Game   // Access to game logic functions

// This attribute is essential for TickSpec to discover scenarios in F# modules.
// It tells TickSpec to run the scenarios defined within this module.
type GomokuGameSteps() =

    // Define mutable state for the scenario 
    // This allows steps to share and modify data during a scenario's execution.
    let mutable board : Board = createBoard()
    let mutable currentPlayer = X
    let mutable winDetected = false
    let mutable drawDetected = false
    let mutable lastMoveSuccess = false

    // Given step: Initializes a new, empty Gomoku board for the scenario.
    [<Given("an empty board")>]
    let ``Given an empty board`` () =
        board <- createBoard() // Reset board to empty state
        currentPlayer <- X     // Reset current player to X (starting player)
        winDetected <- false   // Reset win detection
        drawDetected <- false  // Reset draw detection
        lastMoveSuccess <- false // Reset last move success

    // Given step: Places a piece for player X at specified coordinates.
    // The regex (.*) captures the coordinates as strings.
    [<Given("player X has placed a piece at (.*), (.*)")>]
    let ``Given player X has placed a piece at (x:int), (y:int)`` (xStr: string, yStr: string) =
        let x = int xStr
        let y = int yStr
        placeMove board (x, y) X |> ignore // Place the move, ignore success boolean
        winDetected <- checkWin board X    // Check for win after placement
        drawDetected <- checkDraw board    // Check for draw after placement

    // Given step: Places a piece for player O at specified coordinates.
    [<Given("player O has placed a piece at (.*), (.*)")>]
    let ``Given player O has placed a piece at (x:int), (y:int)`` (xStr: string, yStr: string) =
        let x = int xStr
        let y = int yStr
        placeMove board (x, y) O |> ignore // Place the move
        winDetected <- checkWin board O    // Check for win after placement
        drawDetected <- checkDraw board    // Check for draw after placement

    // Given step: Sets up a board that is almost full, leaving one empty cell for the final move.
    [<Given("the board is almost full with no winner")>]
    let ``Given the board is almost full with no winner`` () =
        board <- createBoard()
        // Fill most of the board, leaving one cell empty (e.g., (0,0))
        for y in 0 .. 14 do
            for x in 0 .. 14 do
                if not (x = 0 && y = 0) then // Leave (0,0) empty
                    if (x + y) % 2 = 0 then
                        placeMove board (x, y) X |> ignore
                    else
                        placeMove board (x, y) O |> ignore
        winDetected <- false // Ensure no win has been created by the setup
        drawDetected <- false

    // When step: Player X attempts to place a piece at specified coordinates.
    [<When("player X places at (.*), (.*)")>]
    let ``When player X places at (x:int), (y:int)`` (xStr: string, yStr: string) =
        let x = int xStr
        let y = int yStr
        lastMoveSuccess <- placeMove board (x, y) X // Store the success of the move
        if lastMoveSuccess then // Only check win/draw if the move was valid
            winDetected <- checkWin board X
            drawDetected <- checkDraw board
        currentPlayer <- X // Set current player for context (though not strictly needed for this step)

    // When step: Player O attempts to place a piece at specified coordinates.
    [<When("player O places at (.*), (.*)")>]
    let ``When player O places at (x:int), (y:int)`` (xStr: string, yStr: string) =
        let x = int xStr
        let y = int yStr
        lastMoveSuccess <- placeMove board (x, y) O
        if lastMoveSuccess then
            winDetected <- checkWin board O
            drawDetected <- checkDraw board
        currentPlayer <- O

    // When step: The last empty cell on the board is filled.
    [<When("the last empty cell is filled")>]
    let ``When the last empty cell is filled`` () =
        let emptyCells = getEmptyCells board
        if emptyCells.Length = 1 then
            let (x, y) = emptyCells.[0]
            // It doesn't matter which player fills the last cell for a draw check
            lastMoveSuccess <- placeMove board (x, y) X
            if lastMoveSuccess then
                drawDetected <- checkDraw board
                winDetected <- checkWin board X || checkWin board O // Check if anyone won at the very last move
        else
            failwith "Precondition for 'the last empty cell is filled' failed: Board does not have exactly one empty cell."

    // Then step: Asserts that a specific player is the winner.
    [<Then("player (.*) should be the winner")>]
    let ``Then player should be the winner`` (p: string) =
        let player = if p = "X" then X else O
        winDetected |> should be True // Check if the win flag was set
        checkWin board player |> should be True // Double-check the actual win condition on the board

    // Then step: Asserts that the game is a draw.
    [<Then("the game should be a draw")>]
    let ``Then the game should be a draw`` () =
        drawDetected |> should be True
        checkDraw board |> should be True

    // Then step: Asserts that the move was successful.
    [<Then("the move should be successful")>]
    let ``Then the move should be successful`` () =
        lastMoveSuccess |> should be True

    // Then step: Asserts that the move failed.
    [<Then("the move should fail")>]
    let ``Then the move should fail`` () =
        lastMoveSuccess |> should be False