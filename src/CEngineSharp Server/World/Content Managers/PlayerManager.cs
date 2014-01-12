using CEngineSharp_Server;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Entities;
using SharpNetty;
using System;
using System.Collections.Generic;
using System.IO;

namespace CEngineSharp_Server.World.Content_Managers
{
    public static class PlayerManager
    {
        private static Dictionary<int, Player> _players = new Dictionary<int, Player>();

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

        public static Player[] GetPlayers()
        {
            Player[] players = new Player[_players.Count];
            _players.Values.CopyTo(players, 0);
            return players;
        }

        public static void AddPlayer(int socketIndex, Player player)
        {
            _players.Add(socketIndex, player);
        }

        public static void RemovePlayer(int socketIndex)
        {
            _players.Remove(socketIndex);
        }

        public static int PlayerCount
        {
            get { return _players.Count; }
        }

        private static bool CheckName(string name)
        {
            try
            {
                string[] names = File.ReadAllLines((Constants.FILEPATH_DATA + "names.txt"));

                foreach (var playernames in names)
                {
                    if (playernames == name)
                        return true;
                }

                return false;
            }

            catch (Exception ex)
            {
                // Let the error handler take care of this problem; since it's an error affecting any registration or login attempts, flag it as a high level error.
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.High);

                // Just here for compiler-satisfaction - we'll never actually return anything as the server will terminate due to the exception, anyway,
                return false;
            }
        }

        private static void AddPlayerName(string name)
        {
            try
            {
                using (StreamWriter streamWriter = File.AppendText(Constants.FILEPATH_DATA + "names.txt"))
                {
                    streamWriter.WriteLine(name);
                }
            }
            catch (Exception ex)
            {
                // Let the error handler take care of this problem; since it's an error affecting any registration or login attempts, flag it as a high level error.
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.High);
            }
        }

        public static bool RegisterPlayer(Player player, string name, string password)
        {
            if (PlayerManager.CheckName(name)) return false;

            player.Name = name;
            player.Password = password;
            player.Level = 1;
            player.TextureNumber = 0;

            foreach (Vitals vital in Enum.GetValues(typeof(Vitals)))
            {
                player.SetVital(vital, 10);
            }

            PlayerManager.SavePlayer(player);

            AddPlayerName(name);

            player.LoggedIn = true;

            return true;
        }

        public static void LoadPlayer(string fileName, int index)
        {
            try
            {
                Player player = _players[index];
                string filePath = Constants.FILEPATH_ACCOUNTS + fileName + ".dat";

                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        player.Name = br.ReadString();
                        player.Password = br.ReadString();
                        player.Level = br.ReadInt32();
                        player.TextureNumber = br.ReadInt32();

                        foreach (Vitals vital in Enum.GetValues(typeof(Vitals)))
                        {
                            player.SetVital(vital, br.ReadInt32());
                        }

                        player.Map = MapManager.GetMap(br.ReadInt32());

                        player.Position = new Vector2i(br.ReadInt32(), br.ReadInt32());
                    }
                }

                player.LoggedIn = true;
            }
            catch (Exception ex)
            {
                // Let the error handler take care of this problem; since it's an error only effecting a particular player, flag it as a low level error.
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public static void SavePlayer(Player player)
        {
            try
            {
                string filePath = Constants.FILEPATH_ACCOUNTS + player.Name + ".dat";

                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(player.Name);
                        bw.Write(player.Password);
                        bw.Write(player.Level);
                        bw.Write(player.TextureNumber);

                        foreach (Vitals vital in Enum.GetValues(typeof(Vitals)))
                        {
                            bw.Write(player.GetVital(vital));
                        }

                        bw.Write(player.MapNum);

                        bw.Write(player.Position.X);
                        bw.Write(player.Position.Y);
                    }
                }
            }
            catch (Exception ex)
            {
                // Let the error handler take care of this problem; since it's an error only effecting a particular player, flag it as a low level error.
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public static void SavePlayers()
        {
            foreach (var player in PlayerManager._players.Values)
            {
                PlayerManager.SavePlayer(player);
            }
        }

        public static bool Authenticate(string name, string password)
        {
            string playerName;
            string playerPassword;

            if (!File.Exists(Constants.FILEPATH_ACCOUNTS + name + ".dat")) return false;

            foreach (var player in _players.Values)
            {
                if (player.LoggedIn)
                    if (player.Name.ToLower() == name.ToLower()) return false;
            }

            using (FileStream fs = new FileStream(Constants.FILEPATH_ACCOUNTS + name + ".dat", FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    playerName = br.ReadString();
                    playerPassword = br.ReadString();
                }
            }

            return (name == playerName && password == playerPassword);
        }

        public static void BroadcastPacket(Packet packet)
        {
            foreach (var player in PlayerManager._players.Values)
            {
                if (player.LoggedIn)
                    player.SendPacket(packet);
            }
        }
    }
}