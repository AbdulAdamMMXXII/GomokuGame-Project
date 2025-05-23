# Gomoku.Tests/Game.feature
Feature: Gomoku Game Play

  As a player
  I want to play Gomoku
  So that I can have fun and experience different game outcomes.

  Scenario: Player X wins by horizontal line
    Given an empty board
    When player X places at 0, 0
    And player X places at 1, 0
    And player X places at 2, 0
    And player X places at 3, 0
    And player X places at 4, 0
    Then player X should be the winner

  Scenario: Player O wins by vertical line
    Given an empty board
    When player O places at 5, 5
    And player O places at 5, 6
    And player O places at 5, 7
    And player O places at 5, 8
    And player O places at 5, 9
    Then player O should be the winner

  Scenario: Player X wins by diagonal (top-left to bottom-right)
    Given an empty board
    When player X places at 0, 0
    And player X places at 1, 1
    And player X places at 2, 2
    And player X places at 3, 3
    And player X places at 4, 4
    Then player X should be the winner

  Scenario: Player O wins by diagonal (bottom-left to top-right)
    Given an empty board
    When player O places at 0, 4
    And player O places at 1, 3
    And player O places at 2, 2
    And player O places at 3, 1
    And player O places at 4, 0
    Then player O should be the winner

  Scenario: Cannot place a move on an already taken cell
    Given an empty board
    When player X places at 0, 0
    And player O places at 0, 0
    Then the move should fail
    And player X should not be the winner
    And player O should not be the winner

  Scenario: Game ends in a draw when board is full
    Given an empty board
    And the board is almost full with no winner
    When the last empty cell is filled
    Then the game should be a draw