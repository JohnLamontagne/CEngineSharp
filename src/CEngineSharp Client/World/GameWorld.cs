using CEngineSharp_Client.World.Entity;
using System.Collections.Generic;

namespace CEngineSharp_Client.World
{
    public static class GameWorld
    {
        private static Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public static int PlayerCount { get { return _players.Count; } }

        public static Player GetPlayer(int index)
        {
            return _players[index];
        }

        public static void AddPlayer(int index, Player player)
        {
            _players.Add(index, player);
        }

        public static void RemovePlayer(int index)
        {
            _players.Remove(index);
        }

        public static void ClearPlayers()
        {
            _players.Clear();
        }

        public static Player[] GetPlayers()
        {
            Player[] players = new Player[_players.Count];

            _players.Values.CopyTo(players, 0);

            return players;
        }

        public static Map CurrentMap { get; set; }
    }
}