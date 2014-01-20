using CEngineSharp_Server.World.Content_Managers;
using System;
using System.IO;

namespace CEngineSharp_Server.World
{
    public sealed class ContentManager
    {
        #region Singleton

        private static ContentManager _contentManager;

        public static ContentManager Instance
        {
            get { return _contentManager ?? (_contentManager = new ContentManager()); }
        }

        #endregion Singleton

        private readonly PlayerManager _playerManager;
        private readonly NpcManager _npcManager;
        private readonly MapManager _mapManager;
        private readonly ItemManager _itemManager;

        public PlayerManager PlayerManager { get { return _playerManager; } }
        public NpcManager NpcManager { get { return _npcManager; } }
        public MapManager MapManager { get { return _mapManager; } }
        public ItemManager ItemManager { get { return _itemManager; } }

        private ContentManager()
        {
            _playerManager = new PlayerManager();
            _npcManager = new NpcManager();
            _mapManager = new MapManager();
            _itemManager = new ItemManager();
        }

        public void LoadContent()
        {
            Console.WriteLine("Checking world integrity...");
            // Check to make sure our files containing the world exist.
            if (!Directory.Exists(Constants.FILEPATH_PLAYERS)) Directory.CreateDirectory(Constants.FILEPATH_PLAYERS.TrimEnd('/'));
            if (!Directory.Exists(Constants.FILEPATH_NPCS)) Directory.CreateDirectory(Constants.FILEPATH_NPCS.TrimEnd('/'));
            if (!Directory.Exists(Constants.FILEPATH_MAPS)) Directory.CreateDirectory(Constants.FILEPATH_MAPS.TrimEnd('/'));
            if (!Directory.Exists(Constants.FILEPATH_ITEMS)) Directory.CreateDirectory(Constants.FILEPATH_ITEMS.TrimEnd('/'));

            // Check to make sure the file for storing players name is there.
            if (!File.Exists(Constants.FILEPATH_DATA + "names.txt")) File.Create(Constants.FILEPATH_DATA + "names.txt").Close();

            _mapManager.LoadMaps();
            _npcManager.LoadNpcs();
            _itemManager.LoadItems();
        }
    }
}
