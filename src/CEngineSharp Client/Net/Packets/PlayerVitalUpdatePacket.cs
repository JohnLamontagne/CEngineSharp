using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
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

            PlayerManager.GetPlayer(PlayerManager.MyIndex).HP = playerHP;
        }

        public override int PacketID
        {
            get { return 17; }
        }
    }
}