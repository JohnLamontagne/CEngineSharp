using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Utilities;
using SFML.Window;
using SharpNetty;

namespace CEngineSharp_Client.Net.Packets.MapUpdatePackets
{
    internal class SpawnMapItemPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            var name = this.DataBuffer.ReadString();
            var textureNumber = this.DataBuffer.ReadInteger();
            var x = this.DataBuffer.ReadInteger();
            var y = this.DataBuffer.ReadInteger();
            var duration = this.DataBuffer.ReadInteger();

            var item = new Item(name, textureNumber)
            {
                Sprite =
                {
                    Position = new Vector2f(x * 32, y * 32)
                }
            };

            MapManager.Map.SpawnMapItem(item, x, y, duration);
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.SpawnMapItemPacket; }
        }
    }
}