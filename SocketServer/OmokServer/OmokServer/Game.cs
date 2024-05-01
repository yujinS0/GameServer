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
    private const int BoardSize = 19;
    private int[,] board = new int[BoardSize, BoardSize];
    private List<RoomUser> players;
    private bool isGameStarted = false;
    public static Func<string, byte[], bool> NetSendFunc;

    //public Game(List<RoomUser> players)
    //{
    //    this.players = players;
    //    InitializeBoard();
    //}
    public Game(List<RoomUser> players, Func<string, byte[], bool> netSendFunction)
    {
        this.players = players ?? throw new ArgumentNullException(nameof(players));
        NetSendFunc = netSendFunction ?? throw new ArgumentNullException(nameof(netSendFunction));
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                board[i, j] = (int)StoneType.None;
            }
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
        NotifyGameStart();
        MainServer.MainLogger.Debug("Game has started.");
    }

    private void NotifyGameStart()
    {
        if (players == null || !players.Any())
            throw new InvalidOperationException("Players list is empty or not initialized.");

        if (NetSendFunc == null)
            throw new InvalidOperationException("Network send function is not set.");


        var random = new Random();
        int firstPlayerIndex = random.Next(players.Count);
        var firstPlayer = players[firstPlayerIndex];

        var startPacket = new PKTNtfStartOmok
        {
            FirstUserID = firstPlayer.UserID
        };

        var sendPacket = MemoryPackSerializer.Serialize(startPacket);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NtfStartOmok);

        // Send this packet to all players
        foreach (var player in players)
        {
            NetSendFunc(player.NetSessionID, sendPacket);
            MainServer.MainLogger.Debug($"Notified {player.UserID} about game start.");
        }
    }

    public void PlaceStone(int x, int y, RoomUser player)
    {
        if (!isGameStarted || board[x, y] != (int)StoneType.None)
        {
            return; // Game not started or position already taken
        }

        board[x, y] = (player == players[0]) ? (int)StoneType.Black : (int)StoneType.White;

        // Check for a win after placing the stone
        if (CheckWin(x, y))
        {
            EndGame(player);
        }
        else
        {
            // Switch turn to the next player
            SwitchTurn();
        }
    }

    private bool CheckWin(int x, int y)
    {
        // Implement win checking logic here
        return false;
    }

    private void EndGame(RoomUser winner)
    {
        isGameStarted = false;
        Console.WriteLine($"Player {winner.UserID} wins the game!");
        // Send game end notification to all players
    }

    private void SwitchTurn()
    {
        // Logic to switch the turn to the next player
    }
}

public enum StoneType
{
    None = 0,
    Black = 1,
    White = 2
}
//public class Game
//{
//    private List<RoomUser> players = new List<RoomUser>();
//    private bool isGameStarted = false;
//    public static Func<string, byte[], bool> NetSendFunc;

//    //public void AddPlayer(RoomUser player)
//    //{
//    //    if (players.Count < 2 && !players.Any(p => p.NetSessionID == player.NetSessionID))
//    //    {
//    //        players.Add(player);
//    //        Console.WriteLine($"Player {player.UserID} added to the game.");
//    //    }
//    //}

//    public bool AreAllPlayersReady()
//    {
//        return players.Count == 2 && players.All(p => p.GetIsReady());
//    }

//    public void StartGame()
//    {
//        if (AreAllPlayersReady() && !isGameStarted)
//        {
//            isGameStarted = true;
//            NotifyGameStart();
//            Console.WriteLine("Game has started.");
//        }
//    }

//    private void NotifyGameStart()
//    {
//        var random = new Random();
//        int firstPlayerIndex = random.Next(players.Count);
//        var firstPlayer = players[firstPlayerIndex];

//        var startPacket = new PKTNtfStartOmok
//        {
//            FirstUserID = firstPlayer.UserID
//        };

//        var sendPacket = MemoryPackSerializer.Serialize(startPacket);
//        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NtfStartOmok);

//        // Send this packet to all players
//        foreach (var player in players)
//        {
//            NetSendFunc(player.NetSessionID, sendPacket);
//            Console.WriteLine($"Notified {player.UserID} about game start.");
//        }
//    }
//}
//////////////////////////////////////////////
//public class Game
//{
//    private const int BoardSize = 19;
//    private int[,] board = new int[BoardSize, BoardSize];
//    public bool IsGameOver { get; private set; }
//    private List<User> players = new List<User>();
//    private int currentPlayerIndex = 0;
//    private int currentTurnCount = 0;

//    public Game()
//    {
//        IsGameOver = false;
//        ClearBoard();
//    }

//    private void ClearBoard()
//    {
//        for (int i = 0; i < BoardSize; i++)
//            for (int j = 0; j < BoardSize; j++)
//                board[i, j] = (int)StoneType.None;
//    }

//    public void AddUser(User user)
//    {
//        if (players.Count < 2 && !players.Contains(user))
//        {
//            players.Add(user);
//        }
//    }

//    public bool AllPlayersReady()
//    {
//        return players.Count == 2 && players.All(p => p.GetIsReady());
//    }

//    //public bool StartGame()
//    //{
//    //    if (AllPlayersReady())
//    //    {
//    //        IsGameOver = false;
//    //        ClearBoard();
//    //        currentPlayerIndex = 0; // Decide who starts the game, could be random or fixed
//    //        currentTurnCount = 1;
//    //        NotifyAllPlayers("Game has started!");
//    //        return true;
//    //    }
//    //    return false;
//    //}
//    public bool StartGame()
//    {
//        if (AllPlayersReady())
//        {
//            IsGameOver = false;
//            ClearBoard();

//            // Randomly select the first player
//            Random rand = new Random();
//            currentPlayerIndex = rand.Next(players.Count);  // Randomly choose between 0 and 1
//            currentTurnCount = 1;

//            NotifyAllPlayers("Game has started!");

//            // Send the start game notification with the first player's ID
//            NotifyStartGame(players[currentPlayerIndex]);

//            return true;
//        }
//        return false;
//    }


//    public bool PlaceStone(int x, int y, User user)
//    {
//        if (IsGameOver || x < 0 || y < 0 || x >= BoardSize || y >= BoardSize || board[x, y] != (int)StoneType.None)
//            return false;

//        if (players[currentPlayerIndex] != user)
//        {
//            NotifyUser(user, "Not your turn!");
//            return false;
//        }

//        board[x, y] = (currentTurnCount % 2 == 1) ? (int)StoneType.Black : (int)StoneType.White;
//        if (CheckWin(x, y))
//        {
//            IsGameOver = true;
//            NotifyAllPlayers($"Player {user.ID()} wins!");
//            return true;
//        }

//        currentTurnCount++;
//        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
//        NotifyAllPlayers($"Player {user.ID()} made a move.");
//        return true;
//    }

//    private bool CheckWin(int x, int y)
//    {
//        return CheckDirection(x, y, 1, 0) || CheckDirection(x, y, 0, 1) ||
//               CheckDirection(x, y, 1, 1) || CheckDirection(x, y, 1, -1);
//    }

//    private bool CheckDirection(int x, int y, int dx, int dy)
//    {
//        int count = 1;
//        int stoneType = board[x, y];

//        count += CountStones(x, y, dx, dy);
//        count += CountStones(x, y, -dx, -dy);

//        return count >= 5;
//    }

//    private int CountStones(int x, int y, int dx, int dy)
//    {
//        int count = 0;
//        int nx = x + dx;
//        int ny = y + dy;

//        while (nx >= 0 && nx < BoardSize && ny >= 0 && ny < BoardSize && board[nx, ny] == board[x, y])
//        {
//            count++;
//            nx += dx;
//            ny += dy;
//        }

//        return count;
//    }

//    private void NotifyAllPlayers(string message)
//    {
//        foreach (var player in players)
//        {
//            // Implement real notification logic here
//            Console.WriteLine($"Notify {player.ID()}: {message}");
//        }
//    }

//    private void NotifyUser(User user, string message)
//    {
//        // Implement real notification logic here
//        Console.WriteLine($"Notify {user.ID()}: {message}");
//    }

//    public void EndGame()
//    {
//        IsGameOver = true;
//        foreach (var player in players)
//            player.SetReady(false);
//        NotifyAllPlayers("Game has ended.");
//    }
//}

//////////////////////////////////////
/*

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
 */