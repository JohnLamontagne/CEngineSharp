using CEngineSharp_Server.World;
using SharpNetty;
using System;
using System.Diagnostics;

namespace CEngineSharp_Server.Net.Packets
{
    public class MapDataPacket : Packet
    {
        public void WriteData(Map map)
        {
            this.PacketBuffer.WriteString(map.Name);
            this.PacketBuffer.WriteInteger(map.MapWidth);
            this.PacketBuffer.WriteInteger(map.MapHeight);

            for (int x = 0; x < map.MapWidth; x++)
            {
                for (int y = 0; y < map.MapHeight; y++)
                {
                    foreach (Map.Layers layer in Enum.GetValues(typeof(Map.Layers)))
                    {
                        if (map.GetTile(x, y).Layers[(int)layer] == null)
                        {
                            this.PacketBuffer.WriteBool(false);
                            continue;
                        }

                        this.PacketBuffer.WriteBool(true);
                        this.PacketBuffer.WriteInteger(map.GetTile(x, y).Layers[(int)layer].TextureNumber);
                        this.PacketBuffer.WriteInteger(map.GetTile(x, y).Layers[(int)layer].SpriteRect.Left);
                        this.PacketBuffer.WriteInteger(map.GetTile(x, y).Layers[(int)layer].SpriteRect.Top);
                        this.PacketBuffer.WriteInteger(map.GetTile(x, y).Layers[(int)layer].SpriteRect.Width);
                        this.PacketBuffer.WriteInteger(map.GetTile(x, y).Layers[(int)layer].SpriteRect.Height);
                    }
                }
            }
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            // Server->Client packet, only.
        }

        public override string PacketID
        {
            get { return "MapDataPacket"; }
        }
    }
}