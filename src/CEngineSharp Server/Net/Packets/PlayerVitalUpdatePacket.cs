using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Entities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class PlayerVitalUpdatePacket : Packet
    {
        public override void Execute(Netty netty)
        {
            // Server->Client only.
        }

        public void WriteData(Player player)
        {
            this.DataBuffer.WriteInteger(player.GetVital(Vitals.HitPoints));
            this.DataBuffer.WriteInteger(player.GetVital(Vitals.ManaPoints));
        }

        public override int PacketID
        {
            get { return 17; }
        }
    }
}