using CEngineSharp_Server.Utilities;
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
        private string _name;
        private ushort _level;
        private string _password;
        private bool _loggedIn = false;
        private int _mapNum;
        private string _ip;
        private AccessLevels _accessLevel;
        private ushort[] _vitals = new ushort[(int)Vitals.Energy + 1];

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public ushort Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        public bool LoggedIn
        {
            get { return _loggedIn; }
            set { _loggedIn = value; }
        }

        public string IP
        {
            get { return _ip; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public AccessLevels AccessLevel
        {
            get { return _accessLevel; }
            set { _accessLevel = value; }
        }

        public int MapNum
        {
            get { return _mapNum; }

            protected set { _mapNum = value; }
        }

        public Player(NettyServer.Connection connection)
        {
            string ip = connection.Socket.RemoteEndPoint.ToString();
            _ip = connection.Socket.RemoteEndPoint.ToString().Remove(ip.IndexOf(':'), ip.Length - ip.IndexOf(':'));
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

        public void Attack(IEntity attacker)
        {
            throw new NotImplementedException();
        }

        public void Interact(IEntity interactor)
        {
            throw new NotImplementedException();
        }

        public bool IsDead()
        {
            throw new NotImplementedException();
        }

        public void SendPacket(Packet packet)
        {
            _connection.SendPacket(packet);
        }
    }
}