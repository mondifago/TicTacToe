using System;
using System.Linq;
using System.Collections.Generic;

namespace TicTacToe
{
    public class Program
    {
        

        public static void Main(string[] args)
        {
            char[,] board = new char[UIMethod.BOARD_ROW_DIM, UIMethod.BOARD_COLUMN_DIM];
            bool isPlayer1Turn = true;
            
            UIMethod.DisplayWelcomeMessage();
            int gameMode = UIMethod.ChooseGameMode();

            for (int row = UIMethod.ZERO_BASED_INDEX; row < board.GetLength(0); row++)
            {
                for (int col = UIMethod.ZERO_BASED_INDEX; col < board.GetLength(1); col++)
                {
                    board[row, col] = UIMethod.BOARD_EMPTY_SPACE;
                }
            }

            UIMethod.PrintBoard(board);

            while (true)
            {
                string currentPlayer = isPlayer1Turn ? UIMethod.PLAYER1 : UIMethod.PLAYER2;
                UIMethod.DisplayCurrentPlayer(currentPlayer);

                if (currentPlayer == UIMethod.PLAYER1)
                {
                    UIMethod.GetCoordinatesForHumanMove(board);
                }
                if (currentPlayer == UIMethod.PLAYER2)
                {
                    ComputerDecidesMoveBasedOnGameMode(board, gameMode);
                }

                UIMethod.PrintBoard(board);

                if (CheckForTheWinningPlayer(board, UIMethod.HUMAN_SYMBOL))
                {
                    UIMethod.DisplayThatHumanHasWon();
                    break;
                }
                if (CheckForTheWinningPlayer(board, UIMethod.COMPUTER_SYMBOL))
                {
                    UIMethod.DisplayThatComputerHasWon();
                    break;
                }
                if (IsBoardFull(board))
                {
                    UIMethod.DisplayDraw();
                    break;
                }
                isPlayer1Turn = !isPlayer1Turn;
            }
        }

        public static void ComputerDecidesMoveBasedOnGameMode(char[,] board, int gameMode)
        {
            if (gameMode == UIMethod.EASY_MODE)
            {
                (int row, int col) = ComputerMakeMoveBasedOnEasyMode(board);
                board[row, col] = UIMethod.COMPUTER_SYMBOL;
            }
            if (gameMode == UIMethod.DIFFICULT_MODE)
            {
                (int row, int col) = ComputerMakeMoveBasedOnDifficultMode(board);
                board[row, col] = UIMethod.COMPUTER_SYMBOL;
            }
        }

        public static (int, int) ComputerMakeMoveBasedOnEasyMode(char[,] board)
        {
            Random random = new Random();
            int row, col;
            do
            {
                row = random.Next(UIMethod.ZERO_BASED_INDEX, UIMethod.BOARD_ROW_DIM);
                col = random.Next(UIMethod.ZERO_BASED_INDEX, UIMethod.BOARD_COLUMN_DIM);
            } while (board[row, col] != UIMethod.BOARD_EMPTY_SPACE);

            return (row, col);
        }

        public static (int, int) ComputerMakeMoveBasedOnDifficultMode(char[,] board)
        {
            int bestScore = int.MinValue;
            int bestRow = UIMethod.INITIAL_INVALID_VALUE;
            int bestCol = UIMethod.INITIAL_INVALID_VALUE;
            char currentPlayerSymbol = UIMethod.COMPUTER_SYMBOL;

            Func<char[,], char, int> evaluateScore = (currentBoard, currentPlayer) =>
            {
                char winner = CheckForTheWinningSymbol(currentBoard);
                if (winner == currentPlayerSymbol) return UIMethod.MINIMAX_WIN_SCORE;
                if (winner != UIMethod.BOARD_EMPTY_SPACE) return UIMethod.MINIMAX_LOOSE_SCORE;
                if (IsBoardFull(currentBoard)) return UIMethod.MINIMAX_DRAW_SCORE;
                return UIMethod.MINIMAX_DRAW_SCORE;
            };

            Func<char[,], int, bool, int> minimax = null;
            minimax = (currentBoard, depth, isMaximizing) =>
            {
                int score = evaluateScore(currentBoard, currentPlayerSymbol);
                if (score != UIMethod.MINIMAX_DRAW_SCORE) return score - depth;

                if (isMaximizing)
                {
                    int maxScore = int.MinValue;
                    for (int row = UIMethod.ZERO_BASED_INDEX; row < currentBoard.GetLength(0); row++)
                    {
                        for (int col = UIMethod.ZERO_BASED_INDEX; col < currentBoard.GetLength(1); col++)
                        {
                            if (currentBoard[row, col] == UIMethod.BOARD_EMPTY_SPACE)
                            {
                                currentBoard[row, col] = currentPlayerSymbol;
                                int currentScore = minimax(currentBoard, depth + 1, false);
                                currentBoard[row, col] = UIMethod.BOARD_EMPTY_SPACE;
                                maxScore = Math.Max(maxScore, currentScore);
                            }
                        }
                    }
                    return maxScore;
                }
                else
                {
                    int minScore = int.MaxValue;
                    char opponentSymbol = (currentPlayerSymbol == UIMethod.HUMAN_SYMBOL) ? UIMethod.COMPUTER_SYMBOL : UIMethod.HUMAN_SYMBOL;
                    for (int row = UIMethod.ZERO_BASED_INDEX; row < currentBoard.GetLength(0); row++)
                    {
                        for (int col = UIMethod.ZERO_BASED_INDEX; col < currentBoard.GetLength(1); col++)
                        {
                            if (currentBoard[row, col] == UIMethod.BOARD_EMPTY_SPACE)
                            {
                                currentBoard[row, col] = opponentSymbol;
                                int currentScore = minimax(currentBoard, depth + 1, true);
                                currentBoard[row, col] = UIMethod.BOARD_EMPTY_SPACE;
                                minScore = Math.Min(minScore, currentScore);
                            }
                        }
                    }
                    return minScore;
                }
            };

            for (int row = UIMethod.ZERO_BASED_INDEX; row < board.GetLength(0); row++)
            {
                for (int col = UIMethod.ZERO_BASED_INDEX; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == UIMethod.BOARD_EMPTY_SPACE)
                    {
                        board[row, col] = currentPlayerSymbol;
                        int score = minimax(board, UIMethod.MINIMAX_DRAW_SCORE, false);
                        board[row, col] = UIMethod.BOARD_EMPTY_SPACE;
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
                return Enumerable.Range(UIMethod.ZERO_BASED_INDEX, board.GetLength(0)).All(i => board[x, i] == symbol) ||
                       Enumerable.Range(UIMethod.ZERO_BASED_INDEX, board.GetLength(1)).All(i => board[i, y] == symbol);
            };

            Func<bool> checkDiagonals = () =>
            {
                return Enumerable.Range(UIMethod.ZERO_BASED_INDEX, board.GetLength(0)).All(i => board[i, i] == symbol) ||
                       Enumerable.Range(UIMethod.ZERO_BASED_INDEX, board.GetLength(1)).All(i => board[i, UIMethod.THIRD_INDEX - i] == symbol);
            };
            return Enumerable.Range(UIMethod.ZERO_BASED_INDEX, board.GetLength(0)).Any(i => checkLine(i, i)) || checkDiagonals();
        }

        public static char CheckForTheWinningSymbol(char[,] board)
        {
            // Check horizontal lines
            for (int i = UIMethod.ZERO_BASED_INDEX; i < board.GetLength(0); i++)
            {
                char winningChar = CheckLine(board, Enumerable.Range(UIMethod.ZERO_BASED_INDEX, board.GetLength(0)).Select(j => (i, j)));
                if (winningChar != UIMethod.BOARD_EMPTY_SPACE)
                {
                    return winningChar;
                }
            }
            // Check vertical lines
            for (int j = UIMethod.ZERO_BASED_INDEX; j < board.GetLength(0); j++)
            {
                char winningChar = CheckLine(board, Enumerable.Range(UIMethod.ZERO_BASED_INDEX, board.GetLength(0)).Select(i => (i, j)));
                if (winningChar != UIMethod.BOARD_EMPTY_SPACE)
                {
                    return winningChar;
                }
            }
            char diagonal1WinningChar = CheckLine(board, Enumerable.Range(UIMethod.ZERO_BASED_INDEX, board.GetLength(0)).Select(i => (i, i)));
            if (diagonal1WinningChar != UIMethod.BOARD_EMPTY_SPACE)
            {
                return diagonal1WinningChar;
            }
            char diagonal2WinningChar = CheckLine(board, Enumerable.Range(UIMethod.ZERO_BASED_INDEX, board.GetLength(0)).Select(i => (i, board.GetLength(0) - 1 - i)));
            if (diagonal2WinningChar != UIMethod.BOARD_EMPTY_SPACE)
            {
                return diagonal2WinningChar;
            }
            // No winner found
            return UIMethod.BOARD_EMPTY_SPACE;
        }

        static char CheckLine(char[,] board, IEnumerable<(int, int)> indices)
        {
            char firstChar = UIMethod.BOARD_EMPTY_SPACE;
            foreach (var (i, j) in indices)
            {
                if (firstChar == UIMethod.BOARD_EMPTY_SPACE)
                {
                    firstChar = board[i, j];
                }
                else if (board[i, j] != firstChar)
                {
                    return UIMethod.BOARD_EMPTY_SPACE; // Not all characters in the line are the same
                }
            }
            return firstChar; // All characters in the line are the same
        }

        public static bool IsBoardFull(char[,] board)
        {
            foreach (char cell in board)
            {
                if (cell == UIMethod.BOARD_EMPTY_SPACE)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
