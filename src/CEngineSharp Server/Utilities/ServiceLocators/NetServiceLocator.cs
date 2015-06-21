using CEngineSharp_Server.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Server.Utilities.ServiceLocators
{
    internal class NetServiceLocator : IServiceLocator<NetManager>
    {
        #region singleton

        private static NetServiceLocator _singleton;

        public static NetServiceLocator Singleton { get { return _singleton ?? (_singleton = new NetServiceLocator()); } }

        #endregion singleton

        private NetManager _netManager;

        public NetManager GetService()
        {
            return _netManager;
        }

        public void SetService(NetManager service)
        {
            _netManager = service;
        }
    }
}