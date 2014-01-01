using CEngineSharp_Client.World.Entity;
using System;
using System.Collections.Generic;

namespace CEngineSharp_Client.World.Content_Managers
{
    public static class PlayerManager
    {
        public static int MyIndex { get; set; }

        private static Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public static int PlayerCount { get { return _players.Count; } }

        public static Player GetPlayer(int index)
        {
            try
            {

                return _players[index];
            }
            catch (Exception)
            {
                return null;
            }
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
    }
}