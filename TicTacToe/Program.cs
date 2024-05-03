using System;
using System.Linq;

namespace TicTacToe
{
    public class Program
    {
        const int BOARD_ROW_DIM = 3;
        const int BOARD_COLUMN_DIM = 3;
        const int ZERO_BASED_INDEX = 0;
        const int THIRD_INDEX = 2;
        const int COL_1 = 1;
        const int COL_2 = 2;
        const int COL_3 = 3;
        const int ROW_1 = 1;
        const int ROW_2 = 2;
        const int ROW_3 = 3;
        const int EASY_MODE = 1;
        const int DIFFICULT_MODE = 2;
        const char HUMAN_SYMBOL = 'X';
        const char COMPUTER_SYMBOL = 'O';


        public static void Main(string[] args)
        {
            string player1 = "Human";
            string player2 = "Computer";
            char boardEmptySpace = ' ';
            char[,] board = new char[BOARD_ROW_DIM, BOARD_COLUMN_DIM];
            bool isPlayer1Turn = true;
            

            Console.WriteLine("Welcome to Tic Tac Toe. Please select your game mode:");
            Console.Write($"EASY MODE = {EASY_MODE} and DIFFICULT MODE = {DIFFICULT_MODE} ({EASY_MODE}/{DIFFICULT_MODE})?:\t");
            int gameMode = int.Parse(Console.ReadLine());
            Console.WriteLine();

            for (int row = ZERO_BASED_INDEX; row < board.GetLength(0); row++)
            {
                for (int col = ZERO_BASED_INDEX; col < board.GetLength(1); col++)
                {
                    board[row, col] = boardEmptySpace;
                }
            }

            PrintBoard(board);

            while (true)
            {
                string currentPlayer = isPlayer1Turn ? player1 : player2;
                Console.WriteLine($"It is {currentPlayer}'s turn:");

                if (currentPlayer == player1)
                {
                    Console.Write($"Enter row ({ROW_1}-{ROW_3}):\t");
                    int row = int.Parse(Console.ReadLine()) - 1;

                    Console.Write($"Enter column ({COL_1}-{COL_3}):\t");
                    int col = int.Parse(Console.ReadLine()) - 1;

                    try
                    {
                        HumanMove(board, row, col, HUMAN_SYMBOL);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }
                else if (currentPlayer == player2)
                {
                    if (gameMode == EASY_MODE)
                    {
                        (int row, int col) = EasyComputerMove(board);
                        board[row, col] = COMPUTER_SYMBOL;
                    }
                    else if (gameMode == DIFFICULT_MODE)
                    {
                        (int row, int col) = DifficultComputerMove(board);
                        board[row, col] = COMPUTER_SYMBOL;
                    }
                }

                PrintBoard(board);
                if (EasyModeCheckWinner(board, HUMAN_SYMBOL))
                {
                    Console.WriteLine(player1 + " wins!");
                    break;
                }
                else if (EasyModeCheckWinner(board, COMPUTER_SYMBOL))
                {
                    Console.WriteLine(player2 + " wins!");
                    break;
                }
                else if (IsBoardFull(board))
                {
                    Console.WriteLine("It's a draw!");
                    break;
                }
                isPlayer1Turn = !isPlayer1Turn;
            }
        }

        public static void PrintBoard(char[,] board)
        {
            Console.WriteLine($"   {COL_1}   {COL_2}   {COL_3}");
            Console.WriteLine(" -------------");
            for (int row = ZERO_BASED_INDEX; row < board.GetLength(0); row++)
            {
                Console.Write((row + 1) + "|");
                for (int col = ZERO_BASED_INDEX; col < board.GetLength(1); col++)
                {
                    Console.Write($" {board[row, col]} ");
                    if (col < THIRD_INDEX)
                    {
                        Console.Write("|");
                    }
                }
                Console.WriteLine();
                if (row < THIRD_INDEX)
                {
                    Console.WriteLine(" -------------");
                }
            }
        }

        public static void HumanMove(char[,] board, int row, int col, char symbol, char boardEmptySpace = ' ')
        {
            if (row < ZERO_BASED_INDEX || row >= board.GetLength(0) || col < ZERO_BASED_INDEX || col >= board.GetLength(1) || board[row, col] != boardEmptySpace)
            {
                throw new InvalidOperationException("Invalid move! Please choose an empty cell.");
            }

            board[row, col] = symbol;
        }

        public static (int, int) EasyComputerMove(char[,] board, char boardEmptySpace = ' ')
        {
            Random random = new Random();
            int row, col;
            do
            {
                row = random.Next(ZERO_BASED_INDEX, BOARD_ROW_DIM);
                col = random.Next(ZERO_BASED_INDEX, BOARD_COLUMN_DIM);
            } while (board[row, col] != boardEmptySpace);

            return (row, col);
        }

        public static (int, int) DifficultComputerMove(char[,] board)
        {
            int bestScore = int.MinValue;
            int bestRow = -1;
            int bestCol = -1;
            char currentPlayerSymbol = COMPUTER_SYMBOL;

            Func<char[,], char, int> evaluateScore = (currentBoard, currentPlayer) =>
            {
                char winner = DifficultModeCheckWinner(currentBoard, currentPlayer);
                if (winner == currentPlayerSymbol) return 10;
                if (winner != ' ') return -10;
                if (IsBoardFull(currentBoard)) return 0;
                return 0;
            };

            Func<char[,], int, bool, int> minimax = null;
            minimax = (currentBoard, depth, isMaximizing) =>
            {
                int score = evaluateScore(currentBoard, currentPlayerSymbol);
                if (score != 0) return score - depth;

                if (isMaximizing)
                {
                    int maxScore = int.MinValue;
                    for (int row = 0; row < currentBoard.GetLength(0); row++)
                    {
                        for (int col = 0; col < currentBoard.GetLength(1); col++)
                        {
                            if (currentBoard[row, col] == ' ')
                            {
                                currentBoard[row, col] = currentPlayerSymbol;
                                int currentScore = minimax(currentBoard, depth + 1, false);
                                currentBoard[row, col] = ' ';
                                maxScore = Math.Max(maxScore, currentScore);
                            }
                        }
                    }
                    return maxScore;
                }
                else
                {
                    int minScore = int.MaxValue;
                    char opponentSymbol = (currentPlayerSymbol == 'X') ? 'O' : 'X';
                    for (int row = 0; row < currentBoard.GetLength(0); row++)
                    {
                        for (int col = 0; col < currentBoard.GetLength(1); col++)
                        {
                            if (currentBoard[row, col] == ' ')
                            {
                                currentBoard[row, col] = opponentSymbol;
                                int currentScore = minimax(currentBoard, depth + 1, true);
                                currentBoard[row, col] = ' ';
                                minScore = Math.Min(minScore, currentScore);
                            }
                        }
                    }
                    return minScore;
                }
            };

            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == ' ')
                    {
                        board[row, col] = currentPlayerSymbol;
                        int score = minimax(board, 0, false);
                        board[row, col] = ' ';
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

        public static bool EasyModeCheckWinner(char[,] board, char symbol)
        {
            Func<int, int, bool> checkLine = (x, y) =>
            {
                return Enumerable.Range(ZERO_BASED_INDEX, board.GetLength(0)).All(i => board[x, i] == symbol) ||
                       Enumerable.Range(ZERO_BASED_INDEX, board.GetLength(1)).All(i => board[i, y] == symbol);
            };

            Func<bool> checkDiagonals = () =>
            {
                return Enumerable.Range(ZERO_BASED_INDEX, board.GetLength(0)).All(i => board[i, i] == symbol) ||
                       Enumerable.Range(ZERO_BASED_INDEX, board.GetLength(1)).All(i => board[i, THIRD_INDEX - i] == symbol);
            };

            return Enumerable.Range(ZERO_BASED_INDEX, board.GetLength(0)).Any(i => checkLine(i, i)) || checkDiagonals();
        }

        public static char DifficultModeCheckWinner(char[,] board, char currentPlayer)
        {
            // Check for three in a row horizontally and vertically
            for (int i = 0; i < BOARD_ROW_DIM; i++)
            {
                // Check horizontal lines
                if (board[i, 0] != ' ' && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                {
                    return board[i, 0];
                }

                // Check vertical lines
                if (board[0, i] != ' ' && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                {
                    return board[0, i];
                }
            }

            // Check for three in a row diagonally
            if (board[0, 0] != ' ' && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            {
                return board[0, 0];
            }
            if (board[0, 2] != ' ' && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            {
                return board[0, 2];
            }

            // No winner found
            return ' ';
        }

        public static bool IsBoardFull(char[,] board, char boardEmptySpace = ' ')
        {
            foreach (char cell in board)
            {
                if (cell == boardEmptySpace)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
