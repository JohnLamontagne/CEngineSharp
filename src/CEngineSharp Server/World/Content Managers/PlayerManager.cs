using CEngineSharp_Server.Networking;
using CEngineSharp_Server.Utilities.ServiceLocators;
using CEngineSharp_Server.World.Entities;
using CEngineSharp_Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CEngineSharp_Server.World.Content_Managers
{
    public sealed class PlayerManager
    {
        private readonly Dictionary<long, Player> _players;

        public int PlayerCount { get { return _players.Count; } }

        public PlayerManager()
        {
            _players = new Dictionary<long, Player>();

            NetServiceLocator.Singleton.GetService().Connection_Received += Connection_Received;
            NetServiceLocator.Singleton.GetService().AddPacketHandler(PacketType.LoginPacket, this.HandlePlayerLogin);
            NetServiceLocator.Singleton.GetService().AddPacketHandler(PacketType.RegistrationPacket, this.HandlePlayerRegister);
            NetServiceLocator.Singleton.GetService().AddPacketHandler(PacketType.MapCheckPacket, this.HandleMapCheck);
            NetServiceLocator.Singleton.GetService().AddPacketHandler(PacketType.DropItemPacket, this.HandleDropItem);
            NetServiceLocator.Singleton.GetService().AddPacketHandler(PacketType.PickupItemPacket, this.HandlePickupItem);
            NetServiceLocator.Singleton.GetService().AddPacketHandler(PacketType.PlayerMovementPacket, this.HandlePlayerMovement);
            NetServiceLocator.Singleton.GetService().AddPacketHandler(PacketType.ChatMessagePacket, this.HandleChatMessage);
        }

        private void HandleChatMessage(PacketReceivedEventArgs args)
        {
            var player = this.GetPlayer(args.Connection.RemoteUniqueIdentifier);

            var chatMessage = player.Name + " says: " + args.Message.ReadString();

            Packet packet = new Packet(PacketType.ChatMessagePacket);
            packet.Message.Write(chatMessage);
            player.Map.SendPacket(packet, NetDeliveryMethod.Unreliable, ChannelTypes.CHAT);
        }

        private void HandlePlayerMovement(PacketReceivedEventArgs args)
        {
            Vector vector = args.Message.ReadVector();
            byte direction = args.Message.ReadByte();

            var player = this.GetPlayer(args.Connection.RemoteUniqueIdentifier);
            player.MoveTo(vector, direction);
        }

        private void HandlePickupItem(PacketReceivedEventArgs args)
        {
            Vector vector = args.Message.ReadVector();

            var player = this.GetPlayer(args.Connection.RemoteUniqueIdentifier);
            player.TryPickupItem(vector);
        }

        private void HandleDropItem(PacketReceivedEventArgs args)
        {
            var slotNum = args.Message.ReadInt32();

            var player = this.GetPlayer(args.Connection.RemoteUniqueIdentifier);
            player.DropItem(slotNum);
        }

        private void HandleMapCheck(PacketReceivedEventArgs args)
        {
            var player = this.GetPlayer(args.Connection.RemoteUniqueIdentifier);

            var response = args.Message.ReadBoolean();

            if (response == false)
            {
                Packet packet = new Packet(PacketType.MapDataPacket);
                packet.Message.Write(player.Map.GetMapData());

                player.Connection.SendMessage(packet.Message, NetDeliveryMethod.ReliableOrdered, (int)ChannelTypes.WORLD);
                return;
            }

            // The player is now in the map.
            // Set their inMap variable to true.
            // This is to make sure they're able to actually see the map before any map updates occur.
            player.InMap = true;

            player.SendPlayerData();

            // Send all of the players...
            foreach (var mapPlayer in player.Map.GetPlayers())
            {
                if (mapPlayer == player) continue;

                Packet packet = new Packet(PacketType.PlayerDataPacket);
                packet.Message.Write(mapPlayer.GetPlayerData());
                player.Connection.SendMessage(packet.Message, NetDeliveryMethod.ReliableOrdered, (int)ChannelTypes.WORLD);
            }

            // Send all of the items currently spawned in the map.
            foreach (var mapItem in player.Map.GetMapItems())
            {
                Packet packet = new Packet(PacketType.SpawnMapItemPacket);
                packet.Message.Write(mapItem.Item.Name);
                packet.Message.Write(mapItem.Item.TextureNumber);
                packet.Message.Write(mapItem.Position);
                packet.Message.Write(mapItem.SpawnDuration);
                player.Connection.SendMessage(packet.Message, NetDeliveryMethod.ReliableOrdered, (int)ChannelTypes.WORLD);
            }
        }

        private void HandlePlayerRegister(PacketReceivedEventArgs args)
        {
            var username = args.Message.ReadString();
            var password = args.Message.ReadString();

            var player = this.GetPlayer(args.Connection.RemoteUniqueIdentifier);
            player.Name = username;
            player.Password = password;

            var registrationOkay = this.RegisterPlayer(player);

            Packet packet = new Packet(PacketType.RegistrationPacket);
            packet.Message.Write(registrationOkay);

            if (registrationOkay)
            {
                packet.Message.Write("Your account has been registered, logging in now...");
                packet.Message.Write(args.Connection.RemoteUniqueIdentifier);
                player.Connection.SendMessage(packet.Message, NetDeliveryMethod.ReliableOrdered, (int)ChannelTypes.WORLD);
                player.EnterGame();
            }
            else
            {
                packet.Message.Write("Your account has failed to register...");
            }
        }

        private void HandlePlayerLogin(PacketReceivedEventArgs args)
        {
            string username = args.Message.ReadString();
            string password = args.Message.ReadString();

            var player = this.GetPlayer(args.Connection.RemoteUniqueIdentifier);
            player.Name = username;
            player.Password = password;

            bool loginOkay = this.LoginPlayer(args.Connection.RemoteUniqueIdentifier);

            Packet packet = new Packet(PacketType.LoginPacket);
            packet.Message.Write(loginOkay);

            if (loginOkay)
            {
                packet.Message.Write("Login sucess!");
                packet.Message.Write(args.Connection.RemoteUniqueIdentifier);
                player.Connection.SendMessage(packet.Message, NetDeliveryMethod.ReliableOrdered, (int)ChannelTypes.WORLD);

                player.EnterGame();
            }
            else
            {
                // Login failure.
            }
        }

        private void Connection_Received(object sender, Networking.ConnectionEventArgs e)
        {
            var player = new Player(e.Connection.RemoteUniqueIdentifier);
            player.Connection = e.Connection;
            this.AddPlayer(player, e.Connection.RemoteUniqueIdentifier);
        }

        public Player[] GetPlayers()
        {
            return _players.Values.ToArray();
        }

        public Player GetPlayer(string playerName)
        {
            return _players.Values.FirstOrDefault(player => playerName == player.Name);
        }

        public Player GetPlayer(long playerIndex)
        {
            return _players[playerIndex];
        }

        public void AddPlayer(Player player, long playerIndex)
        {
            _players.Add(playerIndex, player);
        }

        public void RemovePlayer(long playerIndex)
        {
            _players.Remove(playerIndex);
        }

        public void RemovePlayer(Player player)
        {
            _players.Remove(player.PlayerIndex);
        }

        public void SavePlayers()
        {
            foreach (var player in this.GetPlayers())
            {
                player.Save(Constants.FILEPATH_PLAYERS);
            }
        }

        public Player LoadPlayer(string filePath)
        {
            var player = new Player { Position = new Vector() };

            using (var fileStream = new FileStream(filePath + ".dat", FileMode.Open))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    player.Name = binaryReader.ReadString();
                    player.Password = binaryReader.ReadString();
                    player.TextureNumber = binaryReader.ReadInt32();
                    player.Position.X = binaryReader.ReadInt32();
                    player.Position.Y = binaryReader.ReadInt32();

                    int statCount = binaryReader.ReadInt32();
                    for (int i = 0; i < statCount; i++)
                        player.SetStat((Stats)i, binaryReader.ReadInt32());
                }
            }

            return player;
        }

        private bool CheckName(string name)
        {
            var names = File.ReadAllLines((Constants.FILEPATH_DATA + "names.txt"));

            return names.Any(playernames => playernames == name);
        }

        private void AppendPlayerName(string name)
        {
            using (var streamWriter = File.AppendText(Constants.FILEPATH_DATA + "names.txt"))
            {
                streamWriter.WriteLine(name);
            }
        }

        private bool AuthenticateRegistration(string playerName, string playerPass)
        {
            // Check to make sure there aren't any players currently logged in with their selected name.
            // Also check to make sure that their selected name hasn't already been selected.
            return !(this.GetPlayers().Any(player => player.Name == playerName && player.LoggedIn) || this.CheckName(playerName));
        }

        public bool RegisterPlayer(Player player)
        {
            if (!this.AuthenticateRegistration(player.Name, player.Password)) return false;

            this.AppendPlayerName(player.Name);

            player.AccessLevel = Player.AccessLevels.Player;

            player.Save(Constants.FILEPATH_PLAYERS);

            return true;
        }

        private bool AuthenticateLogin(Player player)
        {
            var actualPlayer = this.LoadPlayer(Constants.FILEPATH_PLAYERS + player.Name);

            if (actualPlayer.Name == player.Name && actualPlayer.Password == player.Password)
            {
                // Alright, the player is a-okay; let's really load him in now, and pass on the connection from the 'temporary' player.
                actualPlayer.Connection = player.Connection;
                player = actualPlayer;

                return true;
            }

            return false;
        }

        public bool LoginPlayer(long playerIndex)
        {
            var player = this.GetPlayer(playerIndex);

            return player != null && this.AuthenticateLogin(player);
        }
    }
}