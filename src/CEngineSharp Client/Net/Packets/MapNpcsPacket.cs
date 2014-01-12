using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Client.World.Entity;
using SFML.Graphics;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class MapNpcsPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            int mapNpcCount = this.DataBuffer.ReadInteger();

            for (int i = 0; i < mapNpcCount; i++)
            {
                var mapNpc = new Npc();
                mapNpc.Name = this.DataBuffer.ReadString();

                mapNpc.Level = this.DataBuffer.ReadInteger();

                var mapNpcTexture = RenderManager.TextureManager.GetTexture("character" + this.DataBuffer.ReadInteger());

                mapNpc.Sprite = new Sprite(mapNpcTexture);

                mapNpc.Speed = .2f;


                int x = this.DataBuffer.ReadInteger();
                int y = this.DataBuffer.ReadInteger();

                mapNpc.Warp(x, y, Directions.Down);

                MapManager.Map.SpawnMapNpc(mapNpc);

                Console.WriteLine("Spawning npc: " + mapNpc.Name);
            }
        }

        public override int PacketID
        {
            get { return 18; }
        }
    }
}