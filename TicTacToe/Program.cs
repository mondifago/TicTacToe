using System;
using System.Xml.Linq;

namespace TicTacToe;

public class Program
{
    public static void Main(string[] args)
    {
        char[,] board = {
            {' ', ' ', ' '},
            {' ', ' ', ' '},
            {' ', ' ', ' '}
        };

        PrintBoard(board);

        Player1 HumanPlayer = new Player1("Human",'X');
        Player2 ComputerPlayer = new Player2("Computer",'O');
        string currentPlayer;

        bool isPlayer1Turn = true;

        while (true)
        {
            currentPlayer = isPlayer1Turn ? HumanPlayer.ToString() : ComputerPlayer.ToString();
            Console.WriteLine("Player " + (isPlayer1Turn ? "1" : "2") + "'s turn:");

            if (currentPlayer == HumanPlayer.ToString())
            {
                Console.WriteLine("Enter row (1-3):");
                int row = int.Parse(Console.ReadLine()) - 1;

                Console.WriteLine("Enter column (1-3):");
                int col = int.Parse(Console.ReadLine()) - 1;

                try
                {
                    HumanPlayer.HumanMove(board, row, col);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
            if (currentPlayer == ComputerPlayer.ToString())
            {
                (int row, int col) = ComputerPlayer.ComputerMove(board);
                
            }

            PrintBoard(board);

            if (CheckWinner(board, currentPlayer[currentPlayer.Length - 1]))
            {
                Console.WriteLine("Player " + (isPlayer1Turn ? "1" : "2") + " wins!");
                break;
            }
            else if (IsBoardFull(board))
            {
                Console.WriteLine("It's a draw!");
                break;
            }

            isPlayer1Turn = !isPlayer1Turn;
        }
    }

    public static void PrintBoard(char[,] board)
    {
        Console.WriteLine("  1 2 3");
        Console.WriteLine(" -------");
        for (int row = 0; row < 3; row++)
        {
            Console.Write(row + 1 + "|");
            for (int col = 0; col < 3; col++)
            {
                Console.Write(" " + board[row, col] + " ");
                if (col < 2)
                {
                    Console.Write("|");
                }
            }
            Console.WriteLine();
            if (row < 2)
            {
                Console.WriteLine(" -------");
            }
        }
    }

    public static bool CheckWinner(char[,] board, char symbol)
    {
        // Check rows, columns, and diagonals for a winner
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == symbol && board[i, 1] == symbol && board[i, 2] == symbol)
                return true;
            if (board[0, i] == symbol && board[1, i] == symbol && board[2, i] == symbol)
                return true;
        }
        if (board[0, 0] == symbol && board[1, 1] == symbol && board[2, 2] == symbol)
            return true;
        if (board[0, 2] == symbol && board[1, 1] == symbol && board[2, 0] == symbol)
            return true;
        return false;
    }

    public static bool IsBoardFull(char[,] board)
    {
        foreach (char cell in board)
        {
            if (cell == ' ')
            {
                return false;
            }
        }
        return true;
    }
}



public class Player1 
{
    private string NameOfPlayer1;
    private char SymbolOfPlayer1;

    public string HumanPlayer { get; set; }
    public char HumanSymbol { get; set; }

    public Player1(string name, char symbol)
    {
        NameOfPlayer1 = name;
        SymbolOfPlayer1 = symbol;
    }

    public char HumanMove(char[,] board, int row, int col)
    {
        if (row < 0 || row >= board.GetLength(0) || col < 0 || col >= board.GetLength(1) || board[row, col] != ' ')
        {
            throw new InvalidOperationException("Invalid move! Please choose an empty cell.");
        }

        board[row, col] = HumanSymbol;
        return HumanSymbol;
    }

    public override string ToString()
    {
        return $"HumanPlayer: {HumanPlayer}, HumanSymbol: {HumanSymbol}";
    }
}

public class Player2
{
    private string NameOfPlayer2;
    private char SymbolOfPlayer2;

    public string ComputerPlayer { get; set; }
    public char ComputerSymbol { get; set; }

    public Player2(string name, char symbol)
    {
        NameOfPlayer2 = name;
        SymbolOfPlayer2 = symbol;
    }

    public (int, int) ComputerMove(char[,] board)
    {
        // Choose a random empty cell
        Random random = new Random();
        int row, col;
        do
        {
            row = random.Next(0, 3);
            col = random.Next(0, 3);
        } while (board[row, col] != ' ');

        return (row, col);
    }
        
    public override string ToString()
    {
        return $"ComputerPlayer: {ComputerPlayer}, ComputerSymbol: {ComputerSymbol}";
    }
}


