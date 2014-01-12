using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Maps;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class MapDataPacket : Packet
    {
        public void WriteData(Map map)
        {
            this.DataBuffer.WriteString(map.Name);

            this.DataBuffer.WriteInteger(map.Version);

            this.DataBuffer.WriteInteger(map.MapWidth);
            this.DataBuffer.WriteInteger(map.MapHeight);

            for (int x = 0; x < map.MapWidth; x++)
            {
                for (int y = 0; y < map.MapHeight; y++)
                {
                    this.DataBuffer.WriteBool(map.GetTile(x, y).Blocked);

                    foreach (Layers layer in Enum.GetValues(typeof(Layers)))
                    {
                        if (map.GetTile(x, y).GetLayer(layer) == null)
                        {
                            this.DataBuffer.WriteBool(false);
                            continue;
                        }

                        this.DataBuffer.WriteBool(true);

                        this.DataBuffer.WriteInteger(map.GetTile(x, y).GetLayer(layer).TextureNumber);
                        this.DataBuffer.WriteInteger(map.GetTile(x, y).GetLayer(layer).SpriteRect.Left);
                        this.DataBuffer.WriteInteger(map.GetTile(x, y).GetLayer(layer).SpriteRect.Top);
                        this.DataBuffer.WriteInteger(map.GetTile(x, y).GetLayer(layer).SpriteRect.Width);
                        this.DataBuffer.WriteInteger(map.GetTile(x, y).GetLayer(layer).SpriteRect.Height);
                    }
                }
            }
        }

        public override void Execute(Netty netty)
        {
            // Server->Client packet, only.
        }

        public override int PacketID
        {
            get { return 6; }
        }
    }
}