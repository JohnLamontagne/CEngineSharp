using System;
using System.IO;

namespace CEngineSharp_Server.Utilities
{
    public static class ErrorHandler
    {
        public enum ErrorLevels
        {
            Low,
            Medium,
            High
        }

        public static void HandleException(Exception ex, ErrorLevels errorLevel)
        {
            LogError(ex, errorLevel);

            if ((int)errorLevel >= (int)ServerConfiguration.SupressionLevel)
            {
                Console.WriteLine("An unrecoverable error has occured; please check the error log files for additional details.");
                Console.WriteLine("Press any key to continue");

                Globals.ShuttingDown = true;

                Console.ReadKey();

                Environment.Exit(0);
            }
        }

        private static void LogError(Exception ex, ErrorLevels errorLevel)
        {
            using (StreamWriter streamWriter = File.AppendText(Constants.FilePath_Data + "Errors.log"))
            {
                streamWriter.WriteLine("[{0}] - Error Level: {1}, Error Message: {2}", DateTime.Now.ToString("M/d/yyyy"), errorLevel.ToString(), ex.Message);
            }
        }
    }
}