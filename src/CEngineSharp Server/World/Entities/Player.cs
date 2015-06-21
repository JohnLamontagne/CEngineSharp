using CEngineSharp_Server.Networking;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Maps;
using CEngineSharp_Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using Map = CEngineSharp_Server.World.Maps.Map;

namespace CEngineSharp_Server.World.Entities
{
    public class Player : IEntity
    {
        public enum AccessLevels
        {
            Player = 0,
            PlayerModerator,
            Moderator,
            Admin
        }

        public string Name { get; set; }

        public string Password { get; set; }

        public Vector Position { get; set; }

        public int Level { get; set; }

        public int TextureNumber { get; set; }

        public int MapNum { get; protected set; }

        private readonly int[] _stats;

        private readonly List<Item> _inventory = new List<Item>();

        public bool LoggedIn { get; set; }

        public AccessLevels AccessLevel { get; set; }

        public Map Map { get; set; }

        public long PlayerIndex { get; private set; }

        public byte Direction { get; set; }

        public bool InMap { get; set; }

        public NetConnection Connection { get; set; }

        public int InventoryCount { get { return _inventory.Count; } }

        public Player(long index)
        {
            this.Position = new Vector(0, 0);
            this.PlayerIndex = index;
            _stats = new int[(int)Stats.STAT_COUNT];
        }

        public Player()
        {
            _stats = new int[(int)Stats.STAT_COUNT];
            this.Position = new Vector();
        }

        public int GetStat(Stats stat)
        {
            return _stats[(int)stat];
        }

        public void SetStat(Stats stat, int value)
        {
            _stats[(int)stat] = value;
        }

        public Item[] GetInventory()
        {
            return _inventory.ToArray();
        }

        public void Attack(IEntity attacker)
        {
            var damage = attacker.GetDamage();

            if (this.GetStat(Stats.Health) - damage <= 0)
                this.Die(attacker);
        }

        public void Die(IEntity murderer)
        {
            this.SendMessage("Oh dear, you are dead!");

            // Respawn the player.
            this.Respawn();
        }

        public void Interact(IEntity interactor)
        {
            throw new NotImplementedException();
        }

        public int GetDamage()
        {
            throw new NotImplementedException();
        }

        public void MoveTo(Vector vector, byte direction)
        {
            this.Direction = direction;

            //// If the tile is blocked, we obviously can't move to it.
            if (vector.X >= 0 && vector.Y >= 0 && vector.X < this.Map.MapWidth && vector.Y < this.Map.MapHeight && !this.Map.GetTile(vector).Blocked && !this.Map.GetTile(vector).IsOccupied)
            {
                // Unblock the previous tile so that another entity may occupy it.
                this.Map.GetTile(this.Position).IsOccupied = false;

                // Change our character's position to the new position.
                this.Position = vector;

                // Block the tile that we're moving to.
                this.Map.GetTile(vector).IsOccupied = true;
            }

            Packet packet = new Packet(PacketType.PlayerMovementPacket);
            packet.Message.Write(this.PlayerIndex);
            packet.Message.Write(this.Position);
            packet.Message.Write(direction);
            //this.Connection.SendMessage(packet, NetDeliveryMethod.ReliableOrdered, (int)ChannelTypes.WORLD);
            // SENDTOMAP
        }

        public void GiveItem(Item item)
        {
            if (_inventory.Count < Constants.MAX_INVENTORY_ITEMS)
            {
                _inventory.Add(item);

                if (this.InMap)
                    this.SendInventory();

                this.SendMessage("You have received " + item.Name);
            }
            else
            {
                this.SendMessage("Your inventory is full!");
            }
        }

        public void RemoveItem(Item item)
        {
            if (_inventory.Remove(item))
                this.SendInventory();
        }

        public void RemoveItem(int slotNum)
        {
            if (slotNum < _inventory.Count && slotNum >= 0)
            {
                _inventory.RemoveAt(slotNum);
                this.SendInventory();
            }
        }

        public void DropItem(int slotNum)
        {
            if (slotNum < 0 && slotNum >= _inventory.Count) return;

            var item = _inventory[slotNum];

            if (item != null)
            {
                this.Map.SpawnItem(item, new Vector(this.Position), 50000);
                this.RemoveItem(item);
            }
        }

        public void TryPickupItem(Vector mapItemPos)
        {
            MapItem mapItem = this.Map.GetMapItem(mapItemPos);

            if (mapItem == null) return;

            this.Map.RemoveMapItem(mapItem);

            this.GiveItem(mapItem.Item);
        }

        public void EnterGame()
        {
            this.LoggedIn = true;

            this.SetStat(Stats.Health, 100);

            if (this.MapNum < 0)
                this.MapNum = 0;

            this.JoinMap(ContentManager.Instance.MapManager.GetMap(this.MapNum));

            Console.WriteLine(this.Name + " has logged in!");

            //            var chatMessagePacket = new ChatMessagePacket();
            //            chatMessagePacket.WriteData(this.Name + " has logged in!");
            //            PlayerManager.Instance.BroadcastPacket(chatMessagePacket);

            var packet = new Packet(PacketType.ChatMessagePacket);
            packet.Message.Write(string.Format("Welcome to {0}, {1}", ServerConfiguration.GameName, this.Name));
            this.Connection.SendMessage(packet.Message, NetDeliveryMethod.Unreliable, (int)ChannelTypes.CHAT);
        }

        public void JoinMap(Map map)
        {
            if (this.Map != null)
                this.LeaveMap(false);

            map.AddPlayer(this);
            this.Map = map;
            this.MapNum = ContentManager.Instance.MapManager.GetMapIndex(map);

            this.InMap = false;

            Packet packet = new Packet(PacketType.MapCheckPacket);
            packet.Message.Write(this.Map.Name);
            packet.Message.Write(this.Map.Version);
            this.Connection.SendMessage(packet.Message, NetDeliveryMethod.ReliableOrdered, (int)ChannelTypes.WORLD);
        }

        public void SendMessage(string message)
        {
            Packet packet = new Packet(PacketType.ChatMessagePacket);
            packet.Message.Write(message);
            this.Connection.SendMessage(packet.Message, NetDeliveryMethod.Unreliable, (int)ChannelTypes.CHAT);
        }

        public void LeaveGame()
        {
            this.LeaveMap(true);

            this.Save(Constants.FILEPATH_PLAYERS);
            ContentManager.Instance.PlayerManager.RemovePlayer(this.PlayerIndex);
        }

        public void LeaveMap(bool leftGame)
        {
            this.Map.RemovePlayer(this, leftGame);
        }

        private void Respawn()
        {
        }

        public void SendPlayerData()
        {
            Packet packet = new Packet(PacketType.PlayerDataPacket);
            packet.Message.Write(this.GetPlayerData());
            this.Connection.SendMessage(packet.Message, NetDeliveryMethod.ReliableOrdered, (int)ChannelTypes.WORLD);

            this.SendInventory();
            this.SendVitals();
        }

        public void SendInventory()
        {
            Packet packet = new Packet(PacketType.UpdateInventoryPacket);
            packet.Message.Write(this.InventoryCount);
            foreach (var item in this.GetInventory())
            {
                packet.Message.Write(item.Name);
                packet.Message.Write(item.TextureNumber);
            }
            this.Connection.SendMessage(packet.Message, NetDeliveryMethod.ReliableOrdered, (int)ChannelTypes.WORLD);
        }

        public void SendVitals()
        {
            Packet packet = new Packet(PacketType.UpdatePlayerStatsPacket);
            packet.Message.Write(this.GetStat(Stats.Health));
            packet.Message.Write(0);
            this.Connection.SendMessage(packet.Message, NetDeliveryMethod.ReliableOrdered, (int)ChannelTypes.WORLD);
        }

        public NetBuffer GetPlayerData()
        {
            NetBuffer buffer = new NetBuffer();
            buffer.Write(this.PlayerIndex);
            buffer.Write(this.Name);
            buffer.Write(this.Level);
            buffer.Write(this.Position);
            buffer.Write(this.Direction);
            buffer.Write(this.TextureNumber);
            return buffer;
        }

        public void Save(string filePath)
        {
            using (var fileStream = new FileStream(filePath + this.Name + ".dat", FileMode.OpenOrCreate))
            {
                using (var binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(this.Name);
                    binaryWriter.Write(this.Password);
                    binaryWriter.Write(this.TextureNumber);
                    binaryWriter.Write(this.Position.X);
                    binaryWriter.Write(this.Position.Y);

                    binaryWriter.Write(_stats.Length);
                    foreach (var stat in _stats)
                    {
                        binaryWriter.Write(stat);
                    }
                }
            }
        }
    }
}