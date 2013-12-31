using CEngineSharp_Client.World;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    internal class PlayerVitalUpdatePacket : Packet
    {
        public override void Execute(Netty netty)
        {
            int playerHP = this.DataBuffer.ReadInteger();
            int playerMP = this.DataBuffer.ReadInteger();

            GameWorld.GetPlayer(Globals.MyIndex).HP = playerHP;
        }

        public override int PacketID
        {
            get { return 17; }
        }
    }
}