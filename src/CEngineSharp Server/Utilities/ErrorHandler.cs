using System;
using System.IO;

namespace CEngineSharp_Server.Utilities
{
    public static class ErrorHandler
    {
        // Enum that will store the different ErrorLevels associated with exceptions.
        public enum ErrorLevels
        {
            // Signifies an error that will not jeopardize system stability.
            Low,

            // Signifies an error that should not jeopardize system stability.
            Medium,

            // Signifies an error that will (most likely) jeopardize system stability.
            High
        }

        /// <summary>
        ///  Handles a server exception by logging it and, if neccessary, shutting down the server.
        /// </summary>
        /// <param name="ex">The exception object that describes the error that has occured.</param>
        /// <param name="errorLevel">The projected severity of the error that has occured.</param>
        public static void HandleException(Exception ex, ErrorLevels errorLevel)
        {
            // Invoke the method that will log our error in a log-file.
            LogError(ex, errorLevel);

            // If the error that has occured has an error level higher than what we're supressing,
            // Notify the end user that the error is unrecoverable and shut everything down.
            if ((int)errorLevel >= (int)ServerConfiguration.SupressionLevel)
            {
                Console.WriteLine("An unrecoverable error has occured; please check the error log files for additional details.");

                // Set the ShuttingDown variable to true.
                // This will notify the GameLoop that it is time to clean things up.
                Globals.ShuttingDown = true;
            }
        }

        private static void LogError(Exception ex, ErrorLevels errorLevel)
        {
            // Append to the error-log file using StreamWriter.
            using (StreamWriter streamWriter = File.AppendText(Constants.FilePath_Data + "Errors.log"))
            {
                // Write our error details to the log file.
                streamWriter.WriteLine("[{0}] - Error Level: {1}, Error Message: {2} at {3}", DateTime.Now.ToString("M/d/yyyy"), errorLevel.ToString(), ex.Message, ex.StackTrace);
            } // -Using- automatically cleans up the IO for us.
        }
    }
}