using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.Utilities;
using CEngineSharp_Client.World.Entity;
using CEngineSharp_Utilities;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CEngineSharp_Client.World.Content_Managers
{
    public class PlayerManager
    {
        public long ClientID { get; private set; }

        private readonly Dictionary<long, Player> _players;

        public PlayerManager()
        {
            _players = new Dictionary<long, Player>();

            ServiceLocator.NetManager.AddPacketHandler(PacketType.LoginPacket, this.HandleLogin);
            ServiceLocator.NetManager.AddPacketHandler(PacketType.LogoutPacket, this.HandleLogout);
            ServiceLocator.NetManager.AddPacketHandler(PacketType.RegistrationPacket, this.HandleRegistration);
            ServiceLocator.NetManager.AddPacketHandler(PacketType.PlayerDataPacket, this.HandlePlayerData);
            ServiceLocator.NetManager.AddPacketHandler(PacketType.PlayerMovementPacket, this.HandlePlayerMovement);
            ServiceLocator.NetManager.AddPacketHandler(PacketType.UpdateInventoryPacket, this.HandleInventoryUpdate);
            ServiceLocator.NetManager.AddPacketHandler(PacketType.UpdatePlayerStatsPacket, this.HandleUpdateStats);
        }

        private void HandleUpdateStats(PacketReceivedEventArgs args)
        {
            var playerHp = args.Message.ReadInt32();
            var playerMp = args.Message.ReadInt32();

            this.GetPlayer(this.ClientID).Hp = playerHp;
        }

        private void HandleInventoryUpdate(PacketReceivedEventArgs args)
        {
            var inventoryItemCount = args.Message.ReadInt32();

            var player = this.GetPlayer(this.ClientID);

            player.ClearInventory();

            for (int i = 0; i < inventoryItemCount; i++)
            {
                var itemName = args.Message.ReadString();
                var itemTextureNum = args.Message.ReadInt32();

                player.AddInventoryItem(new Item(itemName, itemTextureNum));
            }
        }

        private void HandlePlayerMovement(PacketReceivedEventArgs args)
        {
            var id = args.Message.ReadInt64();
            var x = args.Message.ReadInt32();
            var y = args.Message.ReadInt32();
            var direction = (Directions)args.Message.ReadByte();

            this.GetPlayer(id).Move(x, y, direction);
        }

        private void HandlePlayerData(PacketReceivedEventArgs args)
        {
            var id = args.Message.ReadInt64();
            var playerName = args.Message.ReadString();
            var level = args.Message.ReadInt32();
            var positionX = args.Message.ReadInt32();
            var positionY = args.Message.ReadInt32();
            var direction = args.Message.ReadByte();
            var textureNumber = args.Message.ReadInt32();

            var player = new Player(ServiceLocator.ScreenManager.ActiveScreen.TextureManager.GetTexture("character" + textureNumber), new Vector2i(positionX, positionY));
            this.AddPlayer(id, player);

            if (id == this.ClientID)
            {
                Client.InGame = true;
            }
        }

        private void HandleRegistration(PacketReceivedEventArgs args)
        {
            // Alright, so when the player rec. the go ahead to get into the game, this code will go through
            if (args.Message.ReadBoolean())
            {
                (ServiceLocator.ScreenManager.ActiveScreen as MenuScreen).SetMenuStatus(args.Message.ReadString());

                this.ClientID = args.Message.ReadInt64();

                ServiceLocator.ScreenManager.SetActiveScreen("gameScreen");
            }
            else
            {
                // This should NEVER be true if all is well.
                Debug.Assert(ServiceLocator.ScreenManager.ActiveScreen is MenuScreen, "Graphical transition error!");
                (ServiceLocator.ScreenManager.ActiveScreen as MenuScreen).SetMenuStatus(args.Message.ReadString());
            }
        }

        private void HandleLogout(PacketReceivedEventArgs args)
        {
            long clientID = args.Message.ReadInt64();

            if (this.ClientID == clientID)
            {
                ServiceLocator.ScreenManager.SetActiveScreen("mainMenu");

                this.ClearPlayers();

                return;
            }

            this.RemovePlayer(clientID);
        }

        private void HandleLogin(PacketReceivedEventArgs args)
        {
            if (args.Message.ReadBoolean())
            {
                (ServiceLocator.ScreenManager.ActiveScreen as MenuScreen).SetMenuStatus(args.Message.ReadString());

                this.ClientID = args.Message.ReadInt64();

                ServiceLocator.ScreenManager.SetActiveScreen("gameScreen");
            }
            else
            {
                // This should NEVER be true if all is well.
                Debug.Assert(ServiceLocator.ScreenManager.ActiveScreen is MenuScreen, "Graphical transition error!");
                (ServiceLocator.ScreenManager.ActiveScreen as MenuScreen).SetMenuStatus(args.Message.ReadString());
            }
        }

        public int PlayerCount { get { return _players.Count; } }

        public Player GetPlayer(long id)
        {
            try
            {
                return _players[id];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void AddPlayer(long id, Player player)
        {
            _players.Add(id, player);
        }

        public void RemovePlayer(long id)
        {
            _players.Remove(id);
        }

        public void ClearPlayers()
        {
            _players.Clear();
        }

        public Player[] GetPlayers()
        {
            var players = new Player[_players.Count];

            _players.Values.CopyTo(players, 0);

            return players;
        }
    }
}