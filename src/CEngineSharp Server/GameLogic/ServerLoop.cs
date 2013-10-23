using CEngineSharp_Server.Net.Packets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
using System;
using System.Diagnostics;
using System.Threading;

namespace CEngineSharp_Server.GameLogic
{
    public class ServerLoop
    {
        public class GameTimer
        {
            private Stopwatch stopWatch;

            public GameTimer()
            {
                stopWatch = new Stopwatch();
                stopWatch.Start();
            }

            public long GetTotalTimeElapsed()
            {
                return stopWatch.ElapsedMilliseconds;
            }
        }

        public static GameTimer GameTime;

        public ServerLoop()
        {
            GameTime = new GameTimer();
        }

        public void Start()
        {
            long lastConsoleTitleUpdateTime = 0;
            long lastCpsCheck = 0;
            int cps = 0;
            int cpsCount = 0;

            // Continue processing the server-logic until it's time to shut things down.
            while (!Globals.ShuttingDown)
            {
                if (lastConsoleTitleUpdateTime <= GameTime.GetTotalTimeElapsed())
                {
                    Server.ServerWindow.SetTitle(ServerConfiguration.GameName + " - " + ServerConfiguration.ServerIP + ":" + ServerConfiguration.ServerPort + " - Player Count: " + PlayerManager.PlayerCount +
                        " - Debug Mode: " + (ServerConfiguration.SupressionLevel == ErrorHandler.ErrorLevels.Low ? "On" : "Off") + " - Cps: " + cps + "/sec");

                    lastConsoleTitleUpdateTime = GameTime.GetTotalTimeElapsed() + 500;
                }

                if (lastCpsCheck <= GameTime.GetTotalTimeElapsed())
                {
                    cps = cpsCount;
                    cpsCount = 0;
                    lastCpsCheck = GameTime.GetTotalTimeElapsed() + 1000;
                }
                else
                    cpsCount++;
            }

            // The server loop has stopped executing due to the Globals.ShuttingDown variable being set to true.

            // Save the game world.
            PlayerManager.SavePlayers();
            // Terminate the server.
            Environment.Exit(0);
        }

        public void ServerMessage()
        {
            Random random = new Random();
            string message = Constants.SERVER_MESSAGE_HELLO;
            var chatMessagePacket = new ChatMessagePacket();

            chatMessagePacket.WriteData(message);

            PlayerManager.BroadcastPacket(chatMessagePacket);
        }
    }
}