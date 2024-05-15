using System;
using System.Drawing;

namespace TicTacToe
{
    public static class UIMethods
	{
        public static void DisplayWelcomeMessage()
		{
            Console.WriteLine("Welcome to Tic Tac Toe. Please select your game mode:");
            Console.Write($"EASY MODE = {TTTConstants.EASY_MODE} and DIFFICULT MODE = {TTTConstants.DIFFICULT_MODE} ({TTTConstants.EASY_MODE}/{TTTConstants.DIFFICULT_MODE})?:\t");
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
                    Console.Write("Please enter either 1 or 2:\t");
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
                Console.Write($"Enter row ({TTTConstants.ROW_1}-{TTTConstants.ROW_3}):\t");
                if (!int.TryParse(Console.ReadLine(), out row))
                {
                    Console.WriteLine("Invalid input! Please enter a valid integer.");
                    continue;
                }
                row--; 

                Console.Write($"Enter column ({TTTConstants.COL_1}-{TTTConstants.COL_3}):\t");
                if (!int.TryParse(Console.ReadLine(), out col))
                {
                    Console.WriteLine("Invalid input! Please enter a valid integer.");
                    continue;
                }
                col--; 

                try
                {
                    HumanMakeMove(board, row, col, TTTConstants.HUMAN_SYMBOL);
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
            if (row < TTTConstants.ZERO_BASED_INDEX || row >= board.GetLength(0) || col < TTTConstants.ZERO_BASED_INDEX || col >= board.GetLength(1) || board[row, col] != TTTConstants.BOARD_EMPTY_SPACE)
            {
                throw new InvalidOperationException("Invalid move! Please choose an empty cell.");
            }

            board[row, col] = symbol;
        }

        public static void PrintBoard(char[,] board)
        {
            Console.WriteLine($"   {TTTConstants.COL_1}   {TTTConstants.COL_2}   {TTTConstants.COL_3}");
            Console.WriteLine(" -------------");
            for (int row = TTTConstants.ZERO_BASED_INDEX; row < board.GetLength(0); row++)
            {
                Console.Write((row + 1) + "|");
                for (int col = TTTConstants.ZERO_BASED_INDEX; col < board.GetLength(1); col++)
                {
                    Console.Write($" {board[row, col]} ");
                    if (col < TTTConstants.THIRD_INDEX)
                    {
                        Console.Write("|");
                    }
                }
                Console.WriteLine();
                if (row < TTTConstants.THIRD_INDEX)
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
            Console.WriteLine(TTTConstants.HUMAN_PLAYER + " wins!");
        }

        public static void DisplayThatComputerHasWon()
        {
            Console.WriteLine(TTTConstants.COMPUTER_PLAYER + " wins!");
        }

        public static void DisplayDraw()
        {
            Console.WriteLine("It's a draw!");
        }
    }
}

