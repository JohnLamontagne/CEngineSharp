using CEngineSharp_Server.Net.Packets;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;
using System.IO;

namespace CEngineSharp_Server.World
{
    public class Player : IEntity
    {
        public enum AccessLevels
        {
            Player = 0,
            Player_Moderator,
            Moderator,
            Admin
        }

        private SharpNetty.NettyServer.Connection _connection;
        private ushort[] _vitals = new ushort[(int)Vitals.Energy + 1];
        private ushort[] _stats = new ushort[(int)Stats.Strength + 1];
        private readonly int _playerIndex;

        public string Name { get; set; }

        public ushort Level { get; set; }

        public bool LoggedIn { get; set; }

        public string IP { get; protected set; }

        public string Password { get; set; }

        public AccessLevels AccessLevel { get; set; }

        public Map Map { get; set; }

        public Vector2i Position { get; set; }

        public int PlayerIndex { get { return _playerIndex; } }

        public int MapNum
        {
            get
            {
                return MapManager.GetMapIndex(this.Map);
            }
        }

        public Player(NettyServer.Connection connection, int index)
        {
            string ip = connection.Socket.RemoteEndPoint.ToString();
            this.IP = connection.Socket.RemoteEndPoint.ToString().Remove(ip.IndexOf(':'), ip.Length - ip.IndexOf(':'));
            _connection = connection;
            _playerIndex = index;
        }

        public ushort GetVital(Vitals vital)
        {
            return _vitals[(int)vital];
        }

        public void SetVital(Vitals vital, ushort value)
        {
            _vitals[(int)vital] = value;
        }

        public ushort GetStat(Stats stat)
        {
            return _stats[(int)stat];
        }

        public void SetStat(Stats stat, ushort value)
        {
            _stats[(int)stat] = value;
        }

        public void Attack(IEntity attacker)
        {
            int damage = attacker.GetDamage();

            if (this.GetVital(Vitals.HitPoints) - damage <= 0)
                this.Die(attacker);
        }

        public void Die(IEntity murderer)
        {
            this.SendMessage("Oh dear, you are dead!");

            // Respawn the player.
            this.Respawn();
        }

        public void MoveTo(Vector2i vector)
        {
            //// If the tile is blocked, we obviously can't move to it.
            //if (this.Map.Tiles[vector.X, vector.Y].Blocked || this.Map.Tiles[vector.X, vector.Y].IsOccupied)
            //    return;

            // Unblock the previous tile so that another entity may occupy it.
            //this.Map.Tiles[this.Position.X, this.Position.Y].IsOccupied = false;

            // Change our character's position to the new position.
            this.Position = vector;

            // Block the tile that we're moving to.
            //this.Map.Tiles[this.Position.X, this.Position.Y].IsOccupied = true;

            var movementPacket = new MovementPacket();
            movementPacket.WriteData(this.PlayerIndex, this.Position);
            this.Map.SendPacket(movementPacket);
        }

        public void EnterGame()
        {
            var messagePacket = new ChatMessagePacket();
            messagePacket.WriteData(string.Format("Welcome to {0}, {1}", ServerConfiguration.GameName, this.Name));
            this.SendPacket(messagePacket);
        }

        public void LeaveGame()
        {
            this.Map.Players.Remove(this);
        }

        private void Respawn()
        {
        }

        public int GetDamage()
        {
            Random random = new Random();

            return (int)((this.GetStat(Stats.Strength) * random.NextDouble()) + random.Next(1, 10));
        }

        public void Interact(IEntity interactor)
        {
            throw new NotImplementedException();
        }

        public void SendPacket(Packet packet)
        {
            _connection.SendPacket(packet);
        }

        public void JoinMap(Map map)
        {
            if (this.Map != null)
                this.Map.Players.Remove(this);

            map.Players.Add(this);
            this.Map = map;
        }

        public void SendMessage(string message)
        {
            var chatMessagePacket = new ChatMessagePacket();
            chatMessagePacket.WriteData(message);
            this.SendPacket(chatMessagePacket);
        }
    }
}