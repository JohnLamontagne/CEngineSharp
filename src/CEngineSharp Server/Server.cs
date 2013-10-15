using CEngineSharp_Server.GameLogic;
using CEngineSharp_Server.Net;
using CEngineSharp_Server.Net.Packets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
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

                        for (int i = PlayerManager.PlayerCount - 1; i >= 0; i++)
                        {
                            if (PlayerManager.GetPlayer(i).LoggedIn)
                                Networking.KickPlayer(i);
                        }

                        return;
                    }

                    for (int i = 0; i < PlayerManager.PlayerCount; i++)
                    {
                        var player = PlayerManager.GetPlayer(i);

                        if (player.Name.ToLower() == command[1] && player.LoggedIn)
                        {
                            Console.WriteLine("Kicking player: " + command[1]);
                            Networking.KickPlayer(i);
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
            if (!Directory.Exists(Constants.FILEPATH_ACCOUNTS)) Directory.CreateDirectory(Constants.FILEPATH_ACCOUNTS);
            if (!Directory.Exists(Constants.FILEPATH_NPCS)) Directory.CreateDirectory(Constants.FILEPATH_NPCS.TrimEnd('/'));
            if (!Directory.Exists(Constants.FILEPATH_MAPS)) Directory.CreateDirectory(Constants.FILEPATH_MAPS.TrimEnd('/'));
            if (!Directory.Exists(Constants.FILEPATH_ITEMS)) Directory.CreateDirectory(Constants.FILEPATH_ITEMS.TrimEnd('/'));

            // Check to make sure the file for storing players name is there.
            if (!File.Exists(Constants.FILEPATH_DATA + "names.txt")) File.Create(Constants.FILEPATH_DATA + "names.txt");

            MapManager.LoadMaps();
        }

        public static void SaveWorld()
        {
            Console.WriteLine("Saving players...");
            PlayerManager.SavePlayers();
        }
    }
}