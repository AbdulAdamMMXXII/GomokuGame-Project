namespace Gomoku.Tests
// This attribute is for xUnit to discover tests in F# projects.
// It specifies how test collections are managed. CollectionPerAssembly is common.
[<assembly: Xunit.CollectionBehavior(Xunit.CollectionBehavior.CollectionPerAssembly)>] 
do ()

open Xunit
open FsUnit.Xunit 
open Models.Board 
open Logic.Game   

module GameTests =

    // Test for player switching logic
    [<Fact>]
    let ``Switch player alternates between X and O`` () =
        // Switching from X should yield O
        switchPlayer X |> should equal O
        // Switching from O should yield X
        switchPlayer O |> should equal X

    // Test for placing a move on an empty cell
    [<Fact>]
    let ``Place move on empty cell should succeed and update board`` () =
        // Arrange: Create a new empty board
        let board = createBoard()
        // Act: Attempt to place player X's piece at (0,0)
        let success = placeMove board (0, 0) X
        // Assert: The operation should succeed, and the cell should now be taken by X
        success |> should be True
        board.[0, 0] |> should equal (Taken X)

    // Test for attempting to place a move on an already taken cell
    [<Fact>]
    let ``Placing on taken cell should fail and not change content`` () =
        // Arrange: Create a board and place a piece at (0,0) for player X
        let board = createBoard()
        placeMove board (0, 0) X |> ignore // Ignore the boolean result as we only care about the board state
        
        // Act: Attempt to place player O's piece at the same (0,0) cell
        let success = placeMove board (0, 0) O
        // Assert: The operation should fail, and the cell should still be taken by X
        success |> should be False
        board.[0, 0] |> should equal (Taken X)

    // Test to detect a horizontal win condition for Player X
    [<Fact>]
    let ``Detect horizontal win for Player X`` () =
        let board = createBoard()
        for i in 0 .. 4 do placeMove board (i, 0) X |> ignore
        // Check if Player X has won
        checkWin board X |> should be True
        // Ensure Player O has not won
        checkWin board O |> should be False

    // Test to detect a vertical win condition for Player O
    [<Fact>]
    let ``Detect vertical win for Player O`` () =
        // Arrange: Create a board and place 5 'O' pieces vertically at column 0
        let board = createBoard()
        for i in 0 .. 4 do placeMove board (0, i) O |> ignore
        // Act & Assert: Check if Player O has won. It should be true.
        checkWin board O |> should be True
        // Ensure Player X has not won
        checkWin board X |> should be False

    // Test to detect a diagonal win (top-left to bottom-right) for Player X
    [<Fact>]
    let ``Detect diagonal win (top-left to bottom-right) for Player X`` () =
        // Arrange: Create a board and place 5 'X' pieces diagonally
        let board = createBoard()
        for i in 0 .. 4 do placeMove board (i, i) X |> ignore
        // Act & Assert: Check if Player X has won. It should be true.
        checkWin board X |> should be True

    // Test to detect a diagonal win (bottom-left to top-right) for Player O
    [<Fact>]
    let ``Detect diagonal win (bottom-left to top-right) for Player O`` () =
        // Arrange: Create a board and place 5 'O' pieces diagonally
        let board = createBoard()
        for i in 0 .. 4 do placeMove board (i, 4 - i) O |> ignore // (0,4), (1,3), (2,2), (3,1), (4,0)
        // Act & Assert: Check if Player O has won. It should be true.
        checkWin board O |> should be True

    // Test that checkWin returns false when there is no win
    [<Fact>]
    let ``checkWin returns false when no win condition is met`` () =
        // Arrange: Create a board and place some scattered pieces without forming a win
        let board = createBoard()
        placeMove board (0, 0) X |> ignore
        placeMove board (1, 1) O |> ignore
        placeMove board (0, 1) X |> ignore
        placeMove board (2, 0) O |> ignore
        // Act & Assert: Check for win for both players. Both should be false.
        checkWin board X |> should be False
        checkWin board O |> should be False

    // Test for detecting a draw condition when the board is full
    [<Fact>]
    let ``Detect draw when board is completely full with no winner`` () =
        // Create a board and fill every cell and ensurs no player has won
        let board = createBoard()
        // A simple way to fill the board without creating a win is to alternate players
        for y in 0 .. 14 do
            for x in 0 .. 14 do
                if (x + y) % 2 = 0 then
                    placeMove board (x, y) X |> ignore
                else
                    placeMove board (x, y) O |> ignore
        //Check if the game is a draw and it should be true.
        checkDraw board |> should be True

    // Test that checkDraw returns false when the board is not full
    [<Fact>]
    let ``checkDraw returns false when board is not full`` () =
        // Arrange: Create a board and place just one piece, leaving most cells empty
        let board = createBoard()
        placeMove board (7, 7) X |> ignore
        // Act & Assert: Check if the game is a draw. It should be false.
        checkDraw board |> should be False

    // Test that checkDraw returns false immediately after createBoard
    [<Fact>]
    let ``checkDraw returns false for a newly created empty board`` () =
        // Arrange: Create a new board (which is empty)
        let board = createBoard()
        // Act & Assert: Check for draw. It should be false.
        checkDraw board |> should be False

    // Test for getEmptyCells on a brand new board
    [<Fact>]
    let ``getEmptyCells returns all 225 cells for an empty board`` () =
        // Arrange: Create an empty board
        let board = createBoard()
        // Act: Get the list of empty cells
        let emptyCells = getEmptyCells board
        // Assert: The count of empty cells should be 15 * 15 = 225
        emptyCells.Length |> should equal (15 * 15)

    // Test for getEmptyCells after some moves have been made
    [<Fact>]
    let ``getEmptyCells returns correct count after some moves`` () =
        // Arrange: Create a board and place two pieces
        let board = createBoard()
        placeMove board (0, 0) X |> ignore
        placeMove board (1, 1) O |> ignore
        // Act: Get the list of empty cells
        let emptyCells = getEmptyCells board
        // Assert: The count should be 225 - 2 = 223
        emptyCells.Length |> should equal ( (15 * 15) - 2 )