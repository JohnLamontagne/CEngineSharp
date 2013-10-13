using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using SharpNetty;
using System;
using System.Collections.Generic;
using System.IO;

namespace CEngineSharp_Server.World
{
    public static class PlayerManager
    {
        public static Dictionary<int, Player> Players = new Dictionary<int, Player>();

        private static bool CheckName(string name)
        {
            try
            {
                string[] names = File.ReadAllLines((Constants.FilePath_Data + "names.txt"));

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
                using (StreamWriter streamWriter = File.AppendText(Constants.FilePath_Data + "names.txt"))
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

        //public static void JoinMap(Map map, int playerIndex)
        //{
        //    map.Players.Add(playerIndex);
        //    // PlayerManager.Players[playerIndex].MapNum = GameWorld.Maps.IndexOf(map);
        //}

        //public static void LeaveMap(Map map, int playerIndex)
        //{
        //    map.Players.RemoveAt(playerIndex);
        //}

        public static bool RegisterPlayer(Player player, string name, string password)
        {
            if (PlayerManager.CheckName(name)) return false;

            player.Name = name;
            player.Password = password;
            player.Level = 1;

            foreach (Vitals vital in Enum.GetValues(typeof(Vitals)))
            {
                player.SetVital(vital, 10);
            }

            player.LoggedIn = true;

            PlayerManager.SavePlayer(player);

            AddPlayerName(name);

            return true;
        }

        public static Player LoadPlayer(string fileName, NettyServer.Connection connection)
        {
            try
            {
                Player player = new Player(connection);
                string filePath = Constants.FilePath_Accounts + fileName + ".dat";

                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        player.Name = br.ReadString();
                        player.Password = br.ReadString();
                        player.Level = br.ReadUInt16();

                        foreach (Vitals vital in Enum.GetValues(typeof(Vitals)))
                        {
                            player.SetVital(vital, br.ReadUInt16());
                        }
                    }
                }

                player.LoggedIn = true;

                return player;
            }
            catch (Exception ex)
            {
                // Let the error handler take care of this problem; since it's an error only effecting a particular player, flag it as a low level error.
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
                return null;
            }
        }

        public static void SavePlayer(Player player)
        {
            try
            {
                string filePath = Constants.FilePath_Accounts + player.Name + ".dat";

                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(player.Name);
                        bw.Write(player.Password);
                        bw.Write(player.Level);

                        foreach (Vitals vital in Enum.GetValues(typeof(Vitals)))
                        {
                            bw.Write(player.GetVital(vital));
                        }
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
            foreach (var player in PlayerManager.Players.Values)
            {
                PlayerManager.SavePlayer(player);
            }
        }

        public static bool Authenticate(string name, string password)
        {
            string playerName;
            string playerPassword;

            using (FileStream fs = new FileStream(Constants.FilePath_Accounts + name + ".data", FileMode.Open))
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
            foreach (var player in PlayerManager.Players.Values)
            {
                if (player.LoggedIn)
                    player.SendPacket(packet);
            }
        }
    }
}