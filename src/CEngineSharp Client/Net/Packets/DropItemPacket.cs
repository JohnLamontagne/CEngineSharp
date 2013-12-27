﻿using CEngineSharp_Client.World;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class DropItemPacket : Packet
    {
        public void WriteData(int slotNum)
        {
            this.DataBuffer.WriteInteger(slotNum);
        }

        public override void Execute(Netty netty)
        {
            // Client->Server only packet.
        }

        public override int PacketID
        {
            get { return 13; }
        }
    }
}