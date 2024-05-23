using System;
using System.Linq;
using System.Collections.Generic;

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
                    UIMethods.GetCoordinatesForHumanMove(board);
                }
                if (currentPlayer == TTTConstants.COMPUTER_PLAYER)
                {
                    LogicMethod.DecideComputerMoveBasedOnGameMode(board, gameMode);
                }

                UIMethods.PrintBoard(board);

                if (LogicMethod.CheckForTheWinningPlayer(board, TTTConstants.HUMAN_SYMBOL))
                {
                    UIMethods.DisplayThatHumanHasWon();
                    break;
                }
                if (LogicMethod.CheckForTheWinningPlayer(board, TTTConstants.COMPUTER_SYMBOL))
                {
                    UIMethods.DisplayThatComputerHasWon();
                    break;
                }
                if (LogicMethod.IsBoardFull(board))
                {
                    UIMethods.DisplayDraw();
                    break;
                }
                isPlayer1Turn = !isPlayer1Turn;
            }
        }
    }
}
