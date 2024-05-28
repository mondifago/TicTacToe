using System;
namespace TicTacToe
{
    public static class RandomClass
    {
        private static readonly Random RANDOM = new Random();

        public static Random RandomInstance
        {
            get { return RANDOM; }
        }
    }

    public static class LogicMethod
	{
        public static void InitializeBoard(char[,] board)
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    board[row, col] = TTTConstants.BOARD_EMPTY_SPACE;
                }
            }
        }

        public static void MakeHumanMove(char[,] board, int row, int col, char symbol)
        {
            if (row < 0 || row >= board.GetLength(0) || col < 0|| col >= board.GetLength(1) || board[row, col] != TTTConstants.BOARD_EMPTY_SPACE)
            {
                throw new InvalidOperationException("Invalid move! Please choose an empty cell.");
                
            }

            board[row, col] = symbol;
        }

        public static void DecideComputerMoveBasedOnGameMode(char[,] board, int gameMode)
        {
            if (gameMode == TTTConstants.EASY_MODE)
            {
                (int row, int col) = MakeAIMoveEasyMode(board);
                board[row, col] = TTTConstants.COMPUTER_SYMBOL;
            }
            if (gameMode == TTTConstants.DIFFICULT_MODE)
            {
                (int row, int col) = MakeAIMoveDifficultMode(board);
                board[row, col] = TTTConstants.COMPUTER_SYMBOL;
            }
        }

        public static (int, int) MakeAIMoveEasyMode(char[,] board)
        {
            int row, col;
            do
            {
                row = RandomClass.RandomInstance.Next(0, TTTConstants.BOARD_DIMENSION);
                col = RandomClass.RandomInstance.Next(0, TTTConstants.BOARD_DIMENSION);
            } while (board[row, col] != TTTConstants.BOARD_EMPTY_SPACE);

            return (row, col);
        }

        public static (int, int) MakeAIMoveDifficultMode(char[,] board)
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

                int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
                char playerSymbol = isMaximizing ? currentPlayerSymbol : (currentPlayerSymbol == TTTConstants.HUMAN_SYMBOL ? TTTConstants.COMPUTER_SYMBOL : TTTConstants.HUMAN_SYMBOL);

                for (int row = 0; row < currentBoard.GetLength(0); row++)
                {
                    for (int col = 0; col < currentBoard.GetLength(1); col++)
                    {
                        if (currentBoard[row, col] == TTTConstants.BOARD_EMPTY_SPACE)
                        {
                            currentBoard[row, col] = playerSymbol;
                            int currentScore = minimax(currentBoard, depth + 1, !isMaximizing);
                            currentBoard[row, col] = TTTConstants.BOARD_EMPTY_SPACE;

                            if (isMaximizing)
                            {
                                bestScore = Math.Max(bestScore, currentScore);
                            }
                            else
                            {
                                bestScore = Math.Min(bestScore, currentScore);
                            }
                        }
                    }
                }

                return bestScore;
            };

            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == TTTConstants.BOARD_EMPTY_SPACE)
                    {
                        board[row, col] = currentPlayerSymbol;
                        int moveScore = minimax(board, 0, false);
                        board[row, col] = TTTConstants.BOARD_EMPTY_SPACE;

                        if (moveScore > bestScore)
                        {
                            bestScore = moveScore;
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
                return Enumerable.Range(0, board.GetLength(0)).All(i => board[x, i] == symbol) ||
                       Enumerable.Range(0, board.GetLength(1)).All(i => board[i, y] == symbol);
            };

            Func<bool> checkDiagonals = () =>
            {
                return Enumerable.Range(0, board.GetLength(0)).All(i => board[i, i] == symbol) ||
                       Enumerable.Range(0, board.GetLength(1)).All(i => board[i, 2 - i] == symbol);
            };
            return Enumerable.Range(0, board.GetLength(0)).Any(i => checkLine(i, i)) || checkDiagonals();
        }

        public static char CheckForTheWinningSymbol(char[,] board)
        {
            // Check horizontal lines
            for (int i = 0; i < board.GetLength(0); i++)
            {
                char winningChar = CheckLine(board, Enumerable.Range(0, board.GetLength(0)).Select(j => (i, j)));
                if (winningChar != TTTConstants.BOARD_EMPTY_SPACE)
                {
                    return winningChar;
                }
            }
            // Check vertical lines
            for (int j = 0; j < board.GetLength(0); j++)
            {
                char winningChar = CheckLine(board, Enumerable.Range(0, board.GetLength(0)).Select(i => (i, j)));
                if (winningChar != TTTConstants.BOARD_EMPTY_SPACE)
                {
                    return winningChar;
                }
            }
            char diagonal1WinningChar = CheckLine(board, Enumerable.Range(0, board.GetLength(0)).Select(i => (i, i)));
            if (diagonal1WinningChar != TTTConstants.BOARD_EMPTY_SPACE)
            {
                return diagonal1WinningChar;
            }
            char diagonal2WinningChar = CheckLine(board, Enumerable.Range(0, board.GetLength(0)).Select(i => (i, board.GetLength(0) - 1 - i)));
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

        public static bool CheckGameStatus(char[,] board)
        {
            if (CheckForTheWinningPlayer(board, TTTConstants.HUMAN_SYMBOL))
            {
                UIMethods.DisplayThatHumanHasWon();
                return true;
            }
            if (CheckForTheWinningPlayer(board, TTTConstants.COMPUTER_SYMBOL))
            {
                UIMethods.DisplayThatComputerHasWon();
                return true;
            }
            if (IsBoardFull(board))
            {
                UIMethods.DisplayDraw();
                return true;
            }
            return false;
        }
    }
}

