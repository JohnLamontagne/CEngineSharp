using System;
using System.Collections.Generic;
using System.Threading;

namespace CEngineSharp_Server.Utilities
{
    public static class ThreadManager
    {
        private static Dictionary<string, Thread> threads = new Dictionary<string, Thread>();

        public static void AddThread(Thread thread, string name)
        {
            ThreadManager.threads.Add(name, thread);
        }

        public static void RemoveThread(string name)
        {
            ThreadManager.threads.Remove(name);
        }

        public static Thread GetThread(string name)
        {
            return ThreadManager.threads[name];
        }

    }
}