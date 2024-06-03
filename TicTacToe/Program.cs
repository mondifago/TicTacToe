using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using TicTacToe;

namespace TicTacToe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            char[,] board = new char[TTTConstants.BOARD_DIMENSION, TTTConstants.BOARD_DIMENSION];
            bool isPlayer1Turn = true;
            
            UIMethods.DisplayWelcomeMessage();

            int gameMode = UIMethods.ChooseGameMode();

            LogicMethod.InitializeBoard(board);

            UIMethods.PrintBoard(board);

            while (true)
            {
                string currentPlayer = isPlayer1Turn ? TTTConstants.HUMAN_PLAYER : TTTConstants.COMPUTER_PLAYER;
                UIMethods.DisplayCurrentPlayer(currentPlayer);

                if (currentPlayer == TTTConstants.HUMAN_PLAYER)
                {
                    (int row, int col) = UIMethods.GetCoordinatesForHumanMove(board);
                    LogicMethod.MakeHumanMove(board, row, col);
                }

                if (currentPlayer == TTTConstants.COMPUTER_PLAYER)
                {
                    LogicMethod.DecideComputerMoveBasedOnGameMode(board, gameMode);
                }

                UIMethods.PrintBoard(board);

                if (LogicMethod.CheckGameStatus(board))
                {
                    break;
                }
                isPlayer1Turn = !isPlayer1Turn;
            }
        }
    }
}


