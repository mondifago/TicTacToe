﻿using System;
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

            GameMode gameMode = UIMethods.ChooseGameMode();

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

                char status = LogicMethod.CheckGameStatus(board);
                if (status != TTTConstants.GAME_ONGOING)
                {
                    if (status == TTTConstants.HUMAN_SYMBOL)
                    {
                        UIMethods.DisplayThatHumanHasWon();
                    }
                    if (status == TTTConstants.COMPUTER_SYMBOL)
                    {
                        UIMethods.DisplayThatComputerHasWon();
                    }
                    if (status == TTTConstants.DRAW)
                    {
                        UIMethods.DisplayDraw();
                    }
                    break; 
                }
                isPlayer1Turn = !isPlayer1Turn;
            }
        }
    }
}


