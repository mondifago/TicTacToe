using System;
using System.Drawing;

namespace TicTacToe
{
    

    public static class UIMethod
	{
        public const int INITIAL_INVALID_VALUE = -1;
        public const int MINIMAX_WIN_SCORE = 10;
        public const int MINIMAX_LOOSE_SCORE = -10;
        public const int MINIMAX_DRAW_SCORE = 0;
        public const int BOARD_ROW_DIM = 3;
        public const int BOARD_COLUMN_DIM = 3;
        public const int ZERO_BASED_INDEX = 0;
        public const int THIRD_INDEX = 2;
        public const int COL_1 = 1;
        public const int COL_2 = 2;
        public const int COL_3 = 3;
        public const int ROW_1 = 1;
        public const int ROW_2 = 2;
        public const int ROW_3 = 3;
        public const int EASY_MODE = 1;
        public const int DIFFICULT_MODE = 2;
        public const char HUMAN_SYMBOL = 'X';
        public const char COMPUTER_SYMBOL = 'O';
        public const string HUMAN_PLAYER = "Human";
        public const string COMPUTER_PLAYER = "Computer";
        public const char BOARD_EMPTY_SPACE = ' ';

        public static void DisplayWelcomeMessage()
		{
            Console.WriteLine("Welcome to Tic Tac Toe. Please select your game mode:");
            Console.Write($"EASY MODE = {EASY_MODE} and DIFFICULT MODE = {DIFFICULT_MODE} ({EASY_MODE}/{DIFFICULT_MODE})?:\t");
        }

        public static int ChooseGameMode()
        {
            int gameMode = 0;
            bool validInput = false;

            do
            {
                try
                {
                    gameMode = int.Parse(Console.ReadLine());

                    if (gameMode == 1 || gameMode == 2)
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.Write("Please enter either 1 or 2:\t");
                    }
                }
                catch (FormatException)
                {
                    Console.Write("Please enter a valid number (1 / 2):\t");
                }
                catch (OverflowException)
                {
                    Console.Write("Please enter a valid number within the range of integers:\t");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

            } while (!validInput);

            Console.WriteLine();
            return gameMode;
        }


        public static (int, int) GetCoordinatesForHumanMove(char[,] board)
        {
            int row, col;
            while (true)
            {
                Console.Write($"Enter row ({ROW_1}-{ROW_3}):\t");
                if (!int.TryParse(Console.ReadLine(), out row))
                {
                    Console.WriteLine("Invalid input! Please enter a valid integer.");
                    continue;
                }
                row--; 

                Console.Write($"Enter column ({COL_1}-{COL_3}):\t");
                if (!int.TryParse(Console.ReadLine(), out col))
                {
                    Console.WriteLine("Invalid input! Please enter a valid integer.");
                    continue;
                }
                col--; 

                try
                {
                    HumanMakeMove(board, row, col, HUMAN_SYMBOL);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
            return (row, col);
        }

        public static void HumanMakeMove(char[,] board, int row, int col, char symbol)
        {
            if (row < ZERO_BASED_INDEX || row >= board.GetLength(0) || col < ZERO_BASED_INDEX || col >= board.GetLength(1) || board[row, col] != BOARD_EMPTY_SPACE)
            {
                throw new InvalidOperationException("Invalid move! Please choose an empty cell.");
            }

            board[row, col] = symbol;
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

        public static void DisplayCurrentPlayer(string currentPlayer)
        {
            
            Console.WriteLine($"It is {currentPlayer}'s turn:");
        }

        public static void DisplayThatHumanHasWon()
        {
            Console.WriteLine(HUMAN_PLAYER + " wins!");
        }

        public static void DisplayThatComputerHasWon()
        {
            Console.WriteLine(COMPUTER_PLAYER + " wins!");
        }

        public static void DisplayDraw()
        {
            Console.WriteLine("It's a draw!");
        }
    }
}

