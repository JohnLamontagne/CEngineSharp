using System;
using System.IO;

namespace CEngineSharp_Server.Utilities
{
    public static class ServerConfiguration
    {
        private static string _serverIP;
        private static int _serverPort;
        private static int _socketBacklog;
        private static int _maximumConnections;
        private static string _gameName;
        private static ErrorHandler.ErrorLevels _supressionLevel;

        public static string ServerIP
        {
            get { return _serverIP; }
        }

        public static int ServerPort
        {
            get { return _serverPort; }
        }

        public static int SocketBacklog
        {
            get { return _socketBacklog; }
        }

        public static int MaximumConnections
        {
            get { return _maximumConnections; }
        }

        public static string GameName
        {
            get { return _gameName; }
        }

        public static ErrorHandler.ErrorLevels SupressionLevel
        {
            get { return _supressionLevel; }
        }

        private static void CreateConfig()
        {
            string filePath = Constants.FILEPATH_DATA + "Config.configuration";

            using (StreamWriter streamWriter = File.AppendText(filePath))
            {
                streamWriter.WriteLine("ServerIP: 127.0.0.1");
                streamWriter.WriteLine("ServerPort: 4500");
                streamWriter.WriteLine("SocketBacklog: 100");
                streamWriter.WriteLine("MaximumConnections: 100");
                streamWriter.WriteLine("GameName: CEngine#");
                streamWriter.WriteLine("ErrorSuppressionLevel: Medium");
            }
        }

        public static void LoadConfig()
        {
            try
            {
                string filePath = Constants.FILEPATH_DATA + "Config.configuration";

                if (!File.Exists(filePath))
                    CreateConfig();

                string[] config = File.ReadAllLines(filePath);

                _serverIP = config[0].Remove(0, 10);
                _serverPort = int.Parse(config[1].Remove(0, 12));
                _socketBacklog = int.Parse(config[2].Remove(0, 15));
                _maximumConnections = int.Parse(config[3].Remove(0, 20));
                _gameName = config[4].Remove(0, 10);
                _supressionLevel = (ErrorHandler.ErrorLevels)Enum.Parse(typeof(ErrorHandler.ErrorLevels), config[5].Remove(0, 22));
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.High);
            }
        }
    }
}