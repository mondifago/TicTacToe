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
                string userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.Write("Please type in something:\t");
                }
                else
                {
                    if (int.TryParse(userInput, out gameMode))
                    {
                        if (gameMode == TTTConstants.EASY_MODE || gameMode == TTTConstants.DIFFICULT_MODE)
                        {
                            validInput = true;
                        }
                        else
                        {
                            Console.Write($"Please enter either {TTTConstants.EASY_MODE} or {TTTConstants.DIFFICULT_MODE}:\t");
                        }
                    }
                    else
                    {
                        Console.Write($"Please enter a valid number ({TTTConstants.EASY_MODE} / {TTTConstants.DIFFICULT_MODE}):\t");
                    }
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
                row = GetValidInput();

                Console.Write($"Enter column ({TTTConstants.COL_1}-{TTTConstants.COL_3}):\t");
                col = GetValidInput();

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

        public static int GetValidInput()
        {
            int input;
            do
            {
                if (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.Write("Invalid input! Please enter a valid integer:\t");
                    continue;
                }
                input--;
                if (input < TTTConstants.ZERO_BASED_INDEX || input > TTTConstants.THIRD_INDEX) 
                {
                    Console.Write($"Input out of range! Please enter a number between {TTTConstants.ROW_1} and {TTTConstants.ROW_3}:\t");
                    continue;
                }
                break;

            } while (true);

            return input;
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
                Console.Write((row + 1) + TTTConstants.VERTICAL_LINE);
                for (int col = TTTConstants.ZERO_BASED_INDEX; col < board.GetLength(1); col++)
                {
                    Console.Write($" {board[row, col]} ");
                    if (col < TTTConstants.THIRD_INDEX)
                    {
                        Console.Write(TTTConstants.VERTICAL_LINE);
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

