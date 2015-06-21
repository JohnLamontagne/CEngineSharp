using CEngineSharp_Client.Utilities;
using CEngineSharp_Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Client.World.Content_Managers
{
    public class WorldManager
    {
        private readonly PlayerManager _playerManager;
        private MapManager _mapManager;

        public PlayerManager PlayerManager { get { return _playerManager; } }

        public MapManager MapManager { get { return _mapManager; } }

        public WorldManager()
        {
            _playerManager = new PlayerManager();
            _mapManager = new MapManager();
        }
    }
}