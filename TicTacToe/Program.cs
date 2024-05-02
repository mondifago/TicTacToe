using System;

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

        public static void Main(string[] args)
        {
            string player1 = "Human";
            string player2 = "Computer";
            char boardEmptySpace = ' ';
            char humanSymbol = 'X';
            char computerSymbol = 'O';
            
            char[,] board = new char[BOARD_ROW_DIM,BOARD_COLUMN_DIM];
            bool isPlayer1Turn = true;

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
                Console.WriteLine("It is " + (isPlayer1Turn ? player1 : player2) + "'s turn:");

                if (currentPlayer == player1)
                {
                    Console.Write($"Enter row ({ROW_1}-{ROW_3}):\t");
                    int row = int.Parse(Console.ReadLine()) - 1;

                    Console.Write($"Enter column ({COL_1}-{COL_3}):\t");
                    int col = int.Parse(Console.ReadLine()) - 1;

                    try
                    {
                        HumanMove(board, row, col, humanSymbol);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }
                else if (currentPlayer == player2)
                {
                    (int row, int col) = ComputerMove(board);
                    board[row, col] = computerSymbol;
                }

                PrintBoard(board);

                if (CheckWinner(board, humanSymbol))
                {
                    Console.WriteLine(player1 +" wins!");
                    break;
                }
                else if (CheckWinner(board, computerSymbol))
                {
                    Console.WriteLine(player2 +" wins!");
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
                    Console.Write(" " + board[row, col] + " ");
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
            if (row < 0 || row >= board.GetLength(0) || col < 0 || col >= board.GetLength(1) || board[row, col] != boardEmptySpace)
            {
                throw new InvalidOperationException("Invalid move! Please choose an empty cell.");
            }

            board[row, col] = symbol;
        }

        public static (int, int) ComputerMove(char[,] board, char boardEmptySpace = ' ')
        {
            // Choose a random empty cell
            Random random = new Random();
            int row, col;
            do
            {
                row = random.Next(ZERO_BASED_INDEX, BOARD_ROW_DIM);
                col = random.Next(ZERO_BASED_INDEX, BOARD_COLUMN_DIM);
            } while (board[row, col] != boardEmptySpace);

            return (row, col);
        }

        public static bool CheckWinner(char[,] board, char symbol)
        {
            // Check rows, columns, and diagonals for a winner
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == symbol && board[i, 1] == symbol && board[i, 2] == symbol)
                    return true;
                if (board[0, i] == symbol && board[1, i] == symbol && board[2, i] == symbol)
                    return true;
            }
            if (board[0, 0] == symbol && board[1, 1] == symbol && board[2, 2] == symbol)
                return true;
            if (board[0, 2] == symbol && board[1, 1] == symbol && board[2, 0] == symbol)
                return true;
            return false;
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
