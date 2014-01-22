using CEngineSharp_Server;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Entities;
using CEngineSharp_Utilities;
using SharpNetty;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CEngineSharp_Server.World.Content_Managers
{
    public sealed class PlayerManager
    {
        private readonly Dictionary<int, Player> _players;

        public int PlayerCount { get { return _players.Count; } }

        public PlayerManager()
        {
            _players = new Dictionary<int, Player>();
        }

        public Player[] GetPlayers()
        {
            return _players.Values.ToArray();
        }

        public Player GetPlayer(string playerName)
        {
            return _players.Values.FirstOrDefault(player => playerName == player.Name);
        }

        public Player GetPlayer(int playerIndex)
        {
            return _players[playerIndex];
        }

        public void AddPlayer(Player player, int playerIndex)
        {
            _players.Add(playerIndex, player);
        }

        public void RemovePlayer(int playerIndex)
        {
            _players.Remove(playerIndex);
        }

        public void RemovePlayer(Player player)
        {
            _players.Remove(player.PlayerIndex);
        }

        public void SavePlayers()
        {
            foreach (var player in this.GetPlayers())
            {
                player.Save(Constants.FILEPATH_PLAYERS);
            }
        }

        public Player LoadPlayer(string filePath)
        {
            var player = new Player { Position = new Vector() };

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    player.Name = binaryReader.ReadString();
                    player.Password = binaryReader.ReadString();
                    player.TextureNumber = binaryReader.ReadInt32();
                    player.Position.X = binaryReader.ReadInt32();
                    player.Position.Y = binaryReader.ReadInt32();

                    int statCount = binaryReader.ReadInt32();
                    for (int i = 0; i < statCount; i++)
                        player.SetStat((Stats)i, binaryReader.ReadInt32());
                }
            }

            return player;
        }

        private bool CheckName(string name)
        {
            var names = File.ReadAllLines((Constants.FILEPATH_DATA + "names.txt"));

            return names.Any(playernames => playernames == name);
        }

        private void AppendPlayerName(string name)
        {
            using (var streamWriter = File.AppendText(Constants.FILEPATH_DATA + "names.txt"))
            {
                streamWriter.WriteLine(name);
            }
        }

        private bool AuthenticateRegistration(string playerName, string playerPass)
        {
            // Check to make sure there aren't any players currently logged in with their selected name.
            // Also check to make sure that their selected name hasn't already been selected.
            return !(this.GetPlayers().Any(player => player.Name == playerName && player.LoggedIn) || this.CheckName(playerName));
        }

        public bool RegisterPlayer(Player player)
        {
            if (!this.AuthenticateRegistration(player.Name, player.Password)) return false;

            this.AppendPlayerName(player.Name);


            player.AccessLevel = Player.AccessLevels.Player;

            player.Save(Constants.FILEPATH_PLAYERS);

            return true;
        }

        private bool AuthenticateLogin(Player player)
        {
            var actualPlayer = this.LoadPlayer(player.Name);

            if (actualPlayer.Password == player.Name && actualPlayer.Name == player.Password)
            {
                // Alright, the player is a-okay; let's really load him in now, and pass on the connection from the 'temporary' player.
                var authenticatedPlayer = actualPlayer as Player;
                authenticatedPlayer.Connection = player.Connection;
                player = authenticatedPlayer;

                return true;
            }

            return false;
        }

        public bool LoginPlayer(int playerIndex)
        {
            var player = this.GetPlayer(playerIndex);

            if (player != null && this.AuthenticateLogin(player))
            {
                player.EnterGame();
                return true;
            }

            return false;
        }
    }
}