using System;
namespace TicTacToe
{
    public static class TTTConstants
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
        public const char BOARD_EMPTY_SPACE = ' ';
        public const string HUMAN_PLAYER = "Human";
        public const string COMPUTER_PLAYER = "Computer";
        public const string WELCOME_MESSAGE = "Welcome to Tic Tac Toe. Please select your game mode:";
        public const string CHOOSE_VALID_CELL = "Invalid move! Please choose an empty cell.";
        public const string WRONG_FORMAT_MESSAGE = "Invalid input! Please enter a valid integer.";
        public const string OUT_OF_RANGE_MESSAGE = "Please enter either 1 or 2:\t";
        public const string OUT_OF_FORMAT_MESSAGE = "Please enter a valid number (1 / 2):\t";
    }
}

