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

                Console.Write($"Enter row ({board.GetLength(0) - 2}-{board.GetLength(0)}):\t");
                row = GetValidInput(board);

                Console.Write($"Enter column ({board.GetLength(1) - 2}-{board.GetLength(1)}):\t");
                col = GetValidInput(board);

                if (board[row, col] == TTTConstants.BOARD_EMPTY_SPACE)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("The cell is already occupied! Please enter another set of coordinates.");
                }
            }
            return (row, col);
        }

        public static int GetValidInput(char[,] board)
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
                if (input < 0 || input > board.GetLength(0) - 1) 
                {
                    Console.Write($"Input out of range! Please enter a number between {board.GetLength(0) - 2} and {board.GetLength(0)}:\t");
                    continue;
                }
                break;

            } while (true);

            return input;
        }

        public static void PrintBoard(char[,] board)
        {
            Console.WriteLine($"   {board.GetLength(1)-2}   {board.GetLength(1) - 1}   {board.GetLength(1)}");
            Console.WriteLine(" -------------");
            for (int row = 0; row < board.GetLength(0); row++)
            {
                Console.Write((row + 1) + TTTConstants.VERTICAL_LINE);
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    Console.Write($" {board[row, col]} ");
                    if (col < board.GetLength(1) - 1)
                    {
                        Console.Write(TTTConstants.VERTICAL_LINE);
                    }
                }
                Console.WriteLine();
                if (row < board.GetLength(0) - 1)
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

