using CEngineSharp_Server.GameLogic;
using CEngineSharp_Server.Net;
using CEngineSharp_Server.Net.Packets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEngineSharp_Server
{
    public static class Server
    {
        public static MainWindow ServerWindow;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ServerWindow = new MainWindow();

            Application.Run(ServerWindow);
        }

        public static void HandleCommand(string value)
        {
            string[] command = value.Split(' ');

            if (command[0] == "") return;

            switch (command[0].ToLower())
            {
                case "shutdown":
                    Console.WriteLine("Shutting down the server...");
                    Globals.ShuttingDown = true;
                    break;

                case "kick":

                    if (command[1].ToLower() == "all")
                    {
                        Console.WriteLine("Kicking all players...");

                        for (int i = PlayerManager.Players.Count - 1; i >= 0; i++)
                        {
                            if (PlayerManager.Players[i].LoggedIn)
                                Networking.KickPlayer(i);
                        }

                        return;
                    }

                    foreach (var player in PlayerManager.Players)
                    {
                        if (player.Value.Name.ToLower() == command[1] && player.Value.LoggedIn)
                        {
                            Console.WriteLine("Kicking player: " + command[1]);
                            Networking.KickPlayer(player.Key);
                            return;
                        }
                    }

                    Console.WriteLine(command[1] + " is not logged in!");

                    break;

                case "say":
                    var chatMessagePacket = new ChatMessagePacket();
                    string message = "";

                    for (int i = 1; i < command.Length; i++)
                    {
                        message += command[i];
                    }

                    chatMessagePacket.WriteData("Server: " + message);

                    PlayerManager.BroadcastPacket(chatMessagePacket);

                    Console.WriteLine("Server: " + message);

                    break;

                default:
                    Console.WriteLine("Unknown command:" + command[0]);
                    break;
            }
        }

        public static void LoadWorld()
        {
            Console.WriteLine("Checking world integrity...");
            // Check to make sure our files containing the world exist.
            if (!Directory.Exists(Constants.FilePath_Accounts)) Directory.CreateDirectory(Constants.FilePath_Accounts);
            if (!Directory.Exists(Constants.FilePath_Npcs)) Directory.CreateDirectory(Constants.FilePath_Npcs.TrimEnd('/'));
            if (!Directory.Exists(Constants.FilePath_Maps)) Directory.CreateDirectory(Constants.FilePath_Maps.TrimEnd('/'));
            if (!Directory.Exists(Constants.FilePath_Items)) Directory.CreateDirectory(Constants.FilePath_Items.TrimEnd('/'));

            // Check to make sure the file for storing players name is there.
            if (!File.Exists(Constants.FilePath_Data + "names.txt")) File.Create(Constants.FilePath_Data + "names.txt");
        }

        public static void SaveWorld()
        {
            Console.WriteLine("Saving players...");
            PlayerManager.SavePlayers();
        }
    }
}