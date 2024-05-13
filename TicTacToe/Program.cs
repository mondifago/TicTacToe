using System;
using System.Linq;
using System.Collections.Generic;

namespace TicTacToe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            char[,] board = new char[TTTConstants.BOARD_ROW_DIM, TTTConstants.BOARD_COLUMN_DIM];
            bool isPlayer1Turn = true;
            
            UIMethods.DisplayWelcomeMessage();
            int gameMode = UIMethods.ChooseGameMode();

            for (int row = TTTConstants.ZERO_BASED_INDEX; row < board.GetLength(0); row++)
            {
                for (int col = TTTConstants.ZERO_BASED_INDEX; col < board.GetLength(1); col++)
                {
                    board[row, col] = TTTConstants.BOARD_EMPTY_SPACE;
                }
            }

            UIMethods.PrintBoard(board);

            while (true)
            {
                string currentPlayer = isPlayer1Turn ? TTTConstants.HUMAN_PLAYER : TTTConstants.COMPUTER_PLAYER;
                UIMethods.DisplayCurrentPlayer(currentPlayer);

                if (currentPlayer == TTTConstants.HUMAN_PLAYER)
                {
                    UIMethods.GetCoordinatesForHumanMove(board);
                }
                if (currentPlayer == TTTConstants.COMPUTER_PLAYER)
                {
                    ComputerDecidesMoveBasedOnGameMode(board, gameMode);
                }

                UIMethods.PrintBoard(board);

                if (CheckForTheWinningPlayer(board, TTTConstants.HUMAN_SYMBOL))
                {
                    UIMethods.DisplayThatHumanHasWon();
                    break;
                }
                if (CheckForTheWinningPlayer(board, TTTConstants.COMPUTER_SYMBOL))
                {
                    UIMethods.DisplayThatComputerHasWon();
                    break;
                }
                if (IsBoardFull(board))
                {
                    UIMethods.DisplayDraw();
                    break;
                }
                isPlayer1Turn = !isPlayer1Turn;
            }
        }

        public static void ComputerDecidesMoveBasedOnGameMode(char[,] board, int gameMode)
        {
            if (gameMode == TTTConstants.EASY_MODE)
            {
                (int row, int col) = ComputerMakeMoveBasedOnEasyMode(board);
                board[row, col] = TTTConstants.COMPUTER_SYMBOL;
            }
            if (gameMode == TTTConstants.DIFFICULT_MODE)
            {
                (int row, int col) = ComputerMakeMoveBasedOnDifficultMode(board);
                board[row, col] = TTTConstants.COMPUTER_SYMBOL;
            }
        }

        public static (int, int) ComputerMakeMoveBasedOnEasyMode(char[,] board)
        {
            Random random = new Random();
            int row, col;
            do
            {
                row = random.Next(TTTConstants.ZERO_BASED_INDEX, TTTConstants.BOARD_ROW_DIM);
                col = random.Next(TTTConstants.ZERO_BASED_INDEX, TTTConstants.BOARD_COLUMN_DIM);
            } while (board[row, col] != TTTConstants.BOARD_EMPTY_SPACE);

            return (row, col);
        }

        public static (int, int) ComputerMakeMoveBasedOnDifficultMode(char[,] board)
        {
            int bestScore = int.MinValue;
            int bestRow = TTTConstants.INITIAL_INVALID_VALUE;
            int bestCol = TTTConstants.INITIAL_INVALID_VALUE;
            char currentPlayerSymbol = TTTConstants.COMPUTER_SYMBOL;

            Func<char[,], char, int> evaluateScore = (currentBoard, currentPlayer) =>
            {
                char winner = CheckForTheWinningSymbol(currentBoard);
                if (winner == currentPlayerSymbol) return TTTConstants.MINIMAX_WIN_SCORE;
                if (winner != TTTConstants.BOARD_EMPTY_SPACE) return TTTConstants.MINIMAX_LOOSE_SCORE;
                if (IsBoardFull(currentBoard)) return TTTConstants.MINIMAX_DRAW_SCORE;
                return TTTConstants.MINIMAX_DRAW_SCORE;
            };

            Func<char[,], int, bool, int> minimax = null;
            minimax = (currentBoard, depth, isMaximizing) =>
            {
                int score = evaluateScore(currentBoard, currentPlayerSymbol);
                if (score != TTTConstants.MINIMAX_DRAW_SCORE) return score - depth;

                if (isMaximizing)
                {
                    int maxScore = int.MinValue;
                    for (int row = TTTConstants.ZERO_BASED_INDEX; row < currentBoard.GetLength(0); row++)
                    {
                        for (int col = TTTConstants.ZERO_BASED_INDEX; col < currentBoard.GetLength(1); col++)
                        {
                            if (currentBoard[row, col] == TTTConstants.BOARD_EMPTY_SPACE)
                            {
                                currentBoard[row, col] = currentPlayerSymbol;
                                int currentScore = minimax(currentBoard, depth + 1, false);
                                currentBoard[row, col] = TTTConstants.BOARD_EMPTY_SPACE;
                                maxScore = Math.Max(maxScore, currentScore);
                            }
                        }
                    }
                    return maxScore;
                }
                else
                {
                    int minScore = int.MaxValue;
                    char opponentSymbol = (currentPlayerSymbol == TTTConstants.HUMAN_SYMBOL) ? TTTConstants.COMPUTER_SYMBOL : TTTConstants.HUMAN_SYMBOL;
                    for (int row = TTTConstants.ZERO_BASED_INDEX; row < currentBoard.GetLength(0); row++)
                    {
                        for (int col = TTTConstants.ZERO_BASED_INDEX; col < currentBoard.GetLength(1); col++)
                        {
                            if (currentBoard[row, col] == TTTConstants.BOARD_EMPTY_SPACE)
                            {
                                currentBoard[row, col] = opponentSymbol;
                                int currentScore = minimax(currentBoard, depth + 1, true);
                                currentBoard[row, col] = TTTConstants.BOARD_EMPTY_SPACE;
                                minScore = Math.Min(minScore, currentScore);
                            }
                        }
                    }
                    return minScore;
                }
            };

            for (int row = TTTConstants.ZERO_BASED_INDEX; row < board.GetLength(0); row++)
            {
                for (int col = TTTConstants.ZERO_BASED_INDEX; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == TTTConstants.BOARD_EMPTY_SPACE)
                    {
                        board[row, col] = currentPlayerSymbol;
                        int score = minimax(board, TTTConstants.MINIMAX_DRAW_SCORE, false);
                        board[row, col] = TTTConstants.BOARD_EMPTY_SPACE;
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestRow = row;
                            bestCol = col;
                        }
                    }
                }
            }
            return (bestRow, bestCol);
        }

        public static bool CheckForTheWinningPlayer(char[,] board, char symbol)
        {
            Func<int, int, bool> checkLine = (x, y) =>
            {
                return Enumerable.Range(TTTConstants.ZERO_BASED_INDEX, board.GetLength(0)).All(i => board[x, i] == symbol) ||
                       Enumerable.Range(TTTConstants.ZERO_BASED_INDEX, board.GetLength(1)).All(i => board[i, y] == symbol);
            };

            Func<bool> checkDiagonals = () =>
            {
                return Enumerable.Range(TTTConstants.ZERO_BASED_INDEX, board.GetLength(0)).All(i => board[i, i] == symbol) ||
                       Enumerable.Range(TTTConstants.ZERO_BASED_INDEX, board.GetLength(1)).All(i => board[i, TTTConstants.THIRD_INDEX - i] == symbol);
            };
            return Enumerable.Range(TTTConstants.ZERO_BASED_INDEX, board.GetLength(0)).Any(i => checkLine(i, i)) || checkDiagonals();
        }

        public static char CheckForTheWinningSymbol(char[,] board)
        {
            // Check horizontal lines
            for (int i = TTTConstants.ZERO_BASED_INDEX; i < board.GetLength(0); i++)
            {
                char winningChar = CheckLine(board, Enumerable.Range(TTTConstants.ZERO_BASED_INDEX, board.GetLength(0)).Select(j => (i, j)));
                if (winningChar != TTTConstants.BOARD_EMPTY_SPACE)
                {
                    return winningChar;
                }
            }
            // Check vertical lines
            for (int j = TTTConstants.ZERO_BASED_INDEX; j < board.GetLength(0); j++)
            {
                char winningChar = CheckLine(board, Enumerable.Range(TTTConstants.ZERO_BASED_INDEX, board.GetLength(0)).Select(i => (i, j)));
                if (winningChar != TTTConstants.BOARD_EMPTY_SPACE)
                {
                    return winningChar;
                }
            }
            char diagonal1WinningChar = CheckLine(board, Enumerable.Range(TTTConstants.ZERO_BASED_INDEX, board.GetLength(0)).Select(i => (i, i)));
            if (diagonal1WinningChar != TTTConstants.BOARD_EMPTY_SPACE)
            {
                return diagonal1WinningChar;
            }
            char diagonal2WinningChar = CheckLine(board, Enumerable.Range(TTTConstants.ZERO_BASED_INDEX, board.GetLength(0)).Select(i => (i, board.GetLength(0) - 1 - i)));
            if (diagonal2WinningChar != TTTConstants.BOARD_EMPTY_SPACE)
            {
                return diagonal2WinningChar;
            }
            // No winner found
            return TTTConstants.BOARD_EMPTY_SPACE;
        }

        static char CheckLine(char[,] board, IEnumerable<(int, int)> indices)
        {
            char firstChar = TTTConstants.BOARD_EMPTY_SPACE;
            foreach (var (i, j) in indices)
            {
                if (firstChar == TTTConstants.BOARD_EMPTY_SPACE)
                {
                    firstChar = board[i, j];
                }
                else if (board[i, j] != firstChar)
                {
                    return TTTConstants.BOARD_EMPTY_SPACE; // Not all characters in the line are the same
                }
            }
            return firstChar; // All characters in the line are the same
        }

        public static bool IsBoardFull(char[,] board)
        {
            foreach (char cell in board)
            {
                if (cell == TTTConstants.BOARD_EMPTY_SPACE)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
