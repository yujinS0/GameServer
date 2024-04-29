using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryPack;
using System.Collections.Generic;

namespace OmokServer;

public class Game
{
    public const int BoardSize = 15;
    public int[,] Board { get; private set; }
    public bool GameInProgress { get; private set; }

    public Game()
    {
        Board = new int[BoardSize, BoardSize];
        InitializeBoard();
    }

    public void InitializeBoard()
    {
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                Board[i, j] = 0;
            }
        }
    }

    public bool PlaceStone(int x, int y, int player)
    {
        if (x < 0 || x >= BoardSize || y < 0 || y >= BoardSize || Board[x, y] != 0)
            return false;

        Board[x, y] = player;

        if (CheckForWin(x, y, player))
        {
            GameInProgress = false;
            return true;
        }

        return false;
    }

    private bool CheckForWin(int x, int y, int player)
    {
        // Horizontal, Vertical, and Diagonal win conditions check
        return false;  // Implement the win checking logic here
    }

    public void StartGame()
    {
        GameInProgress = true;
        InitializeBoard();
    }

    public void EndGame()
    {
        GameInProgress = false;
    }
}