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

        public string Name { get; set; }

        public ushort Level { get; set; }

        public bool LoggedIn { get; set; }

        public string IP { get; protected set; }

        public string Password { get; set; }

        public AccessLevels AccessLevel { get; set; }

        public Map Map { get; set; }

        public int MapNum
        {
            get
            {
                return MapManager.GetMapIndex(this.Map);
            }
        }

        public Player(NettyServer.Connection connection)
        {
            string ip = connection.Socket.RemoteEndPoint.ToString();
            this.IP = connection.Socket.RemoteEndPoint.ToString().Remove(ip.IndexOf(':'), ip.Length - ip.IndexOf(':'));
            _connection = connection;
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
            var messagePacket = new ChatMessagePacket();
            messagePacket.WriteData("Oh dear, you have died!");
            this.SendPacket(messagePacket);

            // Respawn the player.
            this.Respawn();
        }

        public void EnterGame()
        {
            var messagePacket = new ChatMessagePacket();
            messagePacket.WriteData(string.Format("Welcome to {0}, {1}", ServerConfiguration.GameName, this.Name));
            this.SendPacket(messagePacket);
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
            map.Players.Add(this);
            this.Map = map;
        }
    }
}