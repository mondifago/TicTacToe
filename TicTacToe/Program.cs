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

        HumanPlayer player1 = new HumanPlayer('X');
        ComputerPlayer player2 = new ComputerPlayer('O');

        bool isPlayer1Turn = true;

        while (true)
        {
            Player currentPlayer = isPlayer1Turn ? (Player)player1 : (Player)player2;
            Console.WriteLine("Player " + (isPlayer1Turn ? "1" : "2") + "'s turn:");

            if (currentPlayer is HumanPlayer)
            {
                Console.WriteLine("Enter row (1-3):");
                int row = int.Parse(Console.ReadLine()) - 1;

                Console.WriteLine("Enter column (1-3):");
                int col = int.Parse(Console.ReadLine()) - 1;

                try
                {
                    currentPlayer.MakeMove(board, row, col);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
            else if (currentPlayer is ComputerPlayer)
            {
                (int row, int col) = player2.ChooseMove(board);
                currentPlayer.MakeMove(board, row, col);
            }

            PrintBoard(board);

            if (CheckWinner(board, currentPlayer.Symbol))
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

public abstract class Player
{
    public char Symbol { get; private set; }

    public Player(char symbol)
    {
        Symbol = symbol;
    }

    public abstract void MakeMove(char[,] board, int row, int col);
}

public class HumanPlayer : Player
{
    public HumanPlayer(char symbol) : base(symbol) { }

    public override void MakeMove(char[,] board, int row, int col)
    {
        if (row < 0 || row >= board.GetLength(0) || col < 0 || col >= board.GetLength(1) || board[row, col] != ' ')
        {
            throw new InvalidOperationException("Invalid move! Please choose an empty cell.");
        }

        board[row, col] = Symbol;
    }
}

public class ComputerPlayer : Player
{
    public ComputerPlayer(char symbol) : base(symbol) { }

    public override void MakeMove(char[,] board, int row, int col)
    {
        // Computer player should not use this method
        throw new InvalidOperationException("Computer player cannot make move with explicit row and column.");
    }

    public (int, int) ChooseMove(char[,] board)
    {
        int bestScore = int.MinValue;
        int bestRow = -1;
        int bestCol = -1;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (board[row, col] == ' ')
                {
                    board[row, col] = Symbol;
                    int score = Minimax(board, 0, false);
                    board[row, col] = ' ';

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestRow = row;
                        bestCol = col;
                    }
                }
            }
        }

        return (bestRow, bestCol);
    }

    private int Minimax(char[,] board, int depth, bool isMaximizing)
    {
        char winner = CheckWinnerOfCurrentBoard(board);
        if (winner == Symbol)
        {
            return 10 - depth;
        }
        else if (winner != ' ')
        {
            return depth - 10;
        }
        else if (IsBoardFull(board))
        {
            return 0;
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == ' ')
                    {
                        board[row, col] = Symbol;
                        int score = Minimax(board, depth + 1, false);
                        board[row, col] = ' ';
                        bestScore = Math.Max(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == ' ')
                    {
                        board[row, col] = GetOpponentSymbol();
                        int score = Minimax(board, depth + 1, true);
                        board[row, col] = ' ';
                        bestScore = Math.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }

    private char GetOpponentSymbol()
    {
        return Symbol == 'X' ? 'O' : 'X';
    }

    private char CheckWinnerOfCurrentBoard(char[,] board)
    {
        // Check rows, columns, and diagonals for a winner
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != ' ')
                return board[i, 0];
            if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[0, i] != ' ')
                return board[0, i];
        }
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != ' ')
            return board[0, 0];
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != ' ')
            return board[0, 2];
        return ' ';
    }

    private bool IsBoardFull(char[,] board)
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
