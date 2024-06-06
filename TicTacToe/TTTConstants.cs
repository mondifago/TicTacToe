using System;
namespace TicTacToe
{
    public enum GameMode
    {
        Easy,
        Difficult
    }

    public static class TTTConstants
	{
        public const int INITIAL_INVALID_VALUE = -1;
        public const int MINIMAX_WIN_SCORE = 10;
        public const int MINIMAX_LOOSE_SCORE = -10;
        public const int MINIMAX_DRAW_SCORE = 0;
        public const int BOARD_DIMENSION = 3;
        public const char HUMAN_SYMBOL = 'X';
        public const char COMPUTER_SYMBOL = 'O';
        public const char BOARD_EMPTY_SPACE = ' ';
        public const string HUMAN_PLAYER = "Human";
        public const string COMPUTER_PLAYER = "Computer";
        public const string VERTICAL_LINE = "|";
    }
}

