﻿using System;
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
        public const string PLAYER1 = "Human";
        public const string PLAYER2 = "Computer";
        public const char BOARD_EMPTY_SPACE = ' ';

        public static void DisplayWelcomeMessage()
		{
            Console.WriteLine("Welcome to Tic Tac Toe. Please select your game mode:");
            Console.Write($"EASY MODE = {EASY_MODE} and DIFFICULT MODE = {DIFFICULT_MODE} ({EASY_MODE}/{DIFFICULT_MODE})?:\t");
        }

        public static int GameModeChooser()
        {
            int gameMode = int.Parse(Console.ReadLine());
            Console.WriteLine();
            return gameMode;
        }

        public static (int, int) HumanMoveCoodinates(char[,] board)
        {
            int row, col;
            while (true)
            {
                Console.Write($"Enter row ({ROW_1}-{ROW_3}):\t");
                row = int.Parse(Console.ReadLine()) - 1;

                Console.Write($"Enter column ({COL_1}-{COL_3}):\t");
                col = int.Parse(Console.ReadLine()) - 1;

                try
                { 
                    HumanMove(board, row, col, HUMAN_SYMBOL);
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

        public static void HumanMove(char[,] board, int row, int col, char symbol)
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

        public static void DisplayHumanWinner()
        {
            Console.WriteLine(PLAYER1 + " wins!");
        }

        public static void DisplayComputerWinner()
        {
            Console.WriteLine(PLAYER2 + " wins!");
        }

        public static void DisplayDraw()
        {
            Console.WriteLine("It's a draw!");
        }
    }
}

