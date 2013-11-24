using CEngineSharp_Server.World;
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
                    foreach (Map.Layers layer in Enum.GetValues(typeof(Map.Layers)))
                    {
                        if (map.GetTile(x, y).Layers[(int)layer] == null)
                        {
                            this.DataBuffer.WriteBool(false);
                            continue;
                        }

                        this.DataBuffer.WriteBool(true);
                        this.DataBuffer.WriteInteger(map.GetTile(x, y).Layers[(int)layer].TextureNumber);
                        this.DataBuffer.WriteInteger(map.GetTile(x, y).Layers[(int)layer].SpriteRect.Left);
                        this.DataBuffer.WriteInteger(map.GetTile(x, y).Layers[(int)layer].SpriteRect.Top);
                        this.DataBuffer.WriteInteger(map.GetTile(x, y).Layers[(int)layer].SpriteRect.Width);
                        this.DataBuffer.WriteInteger(map.GetTile(x, y).Layers[(int)layer].SpriteRect.Height);
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