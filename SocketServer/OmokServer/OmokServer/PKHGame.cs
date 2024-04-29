using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryPack;

namespace OmokServer
{
    internal class PKHGame : PKHandler
    {
        Dictionary<int, Game> games = new Dictionary<int, Game>();

        public void HandleGameStart(int roomId)
        {
            if (!games.ContainsKey(roomId))
            {
                var newGame = new Game();
                games[roomId] = newGame;
                newGame.StartGame();
                // Send game start packet to all players in the room
            }
        }

        public void HandlePlaceStone(int roomId, int x, int y, int player)
        {
            if (games.TryGetValue(roomId, out var game) && game.GameInProgress)
            {
                if (game.PlaceStone(x, y, player))
                {
                    // Send game end packet to all players in the room
                    game.EndGame();
                }
            }
        }
    }
}