using CEngineSharp_Client.Net;
using CEngineSharp_Client.Networking;
using CEngineSharp_Client.Utilities;
using CEngineSharp_Client.World.Entity;
using CEngineSharp_Utilities;
using SFML.Graphics;
using SFML.System;
using System;
using System.IO;

namespace CEngineSharp_Client.World.Content_Managers
{
    public class MapManager
    {
        public Map Map { get; private set; }

        public MapManager()
        {
            ServiceLocator.NetManager.AddPacketHandler(PacketType.MapCheckPacket, this.HandleMapCheck);
            ServiceLocator.NetManager.AddPacketHandler(PacketType.MapDataPacket, this.HandleMapData);
            ServiceLocator.NetManager.AddPacketHandler(PacketType.DespawnMapItemPacket, this.DespawnMapItem);
            ServiceLocator.NetManager.AddPacketHandler(PacketType.SpawnMapItemPacket, this.SpawnMapItem);
        }

        private void SpawnMapItem(PacketReceivedEventArgs args)
        {
            var name = args.Message.ReadString();
            var textureNumber = args.Message.ReadInt32();
            var x = args.Message.ReadInt32();
            var y = args.Message.ReadInt32();
            var duration = args.Message.ReadInt32();

            var item = new Item(name, textureNumber)
            {
                Sprite =
                {
                    Position = new Vector2f(x * 32, y * 32)
                }
            };

            this.Map.SpawnMapItem(item, x, y, duration);
        }

        private void HandleMapData(PacketReceivedEventArgs args)
        {
            var map = new Map
            {
                Name = args.Message.ReadString(),
                Version = args.Message.ReadInt32()
            };

            map.ResizeMap(args.Message.ReadInt32(), args.Message.ReadInt32());

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    map.SetTile(x, y, new Map.Tile());

                    map.GetTile(x, y).Blocked = args.Message.ReadBoolean();

                    foreach (Map.Layers layer in Enum.GetValues(typeof(Map.Layers)))
                    {
                        if (!args.Message.ReadBoolean()) continue;

                        var textureNumber = args.Message.ReadInt32();
                        var left = args.Message.ReadInt32();
                        var top = args.Message.ReadInt32();
                        var width = args.Message.ReadInt32();
                        var height = args.Message.ReadInt32();

                        var textureRect = new IntRect(left, top, width, height);

                        var tileSprite = new Sprite(ServiceLocator.ScreenManager.ActiveScreen.TextureManager.GetTexture("tileset" + textureNumber.ToString()))
                        {
                            TextureRect = textureRect
                        };

                        map.GetTile(x, y).SetLayer(layer, new Map.Tile.Layer(tileSprite, x, y));
                    }
                }
            }

            var mapNpcCount = args.Message.ReadInt32();

            for (int i = 0; i < mapNpcCount; i++)
            {
                var npc = new Npc()
                {
                    Name = args.Message.ReadString(),
                    Level = args.Message.ReadInt32(),
                    Sprite = new Sprite(ServiceLocator.ScreenManager.ActiveScreen.TextureManager.GetTexture("npc" + args.Message.ReadInt32())),
                };
                var position = args.Message.ReadVector();
                npc.Position = new SFML.System.Vector2i(position.X, position.Y);
            }

            map.CacheMap();
            this.Map = map;

            // Notify the server that we're now in the game.
            var net = ServiceLocator.NetManager;
            var packet = new Packet(PacketType.MapCheckPacket);
            packet.Message.Write(true);
            net.SendMessage(packet.Message, Lidgren.Network.NetDeliveryMethod.ReliableOrdered, ChannelTypes.WORLD);
        }

        private void DespawnMapItem(PacketReceivedEventArgs args)
        {
            var mapItemX = args.Message.ReadInt32();
            var mapItemY = args.Message.ReadInt32();

            if (Client.InGame)
            {
                this.Map.RemoveMapItem(mapItemX, mapItemY);
            }
        }

        private void HandleMapCheck(PacketReceivedEventArgs args)
        {
            var mapName = args.Message.ReadString();
            var mapVersion = args.Message.ReadInt32();

            var mapExistence = File.Exists(Constants.FILEPATH_CACHE + "Maps/" + mapName + ".map");

            var mapVersionMatch = false;

            if (mapExistence == true)
            {
                this.Map = new World.Map();
                this.Map.LoadCache(mapName);
                mapVersionMatch = (this.Map.Version == mapVersion);
            }

            var net = ServiceLocator.NetManager;
            var packet = new Packet(PacketType.MapCheckPacket);

            packet.Message.Write(mapExistence & mapVersionMatch);

            net.SendMessage(packet.Message, Lidgren.Network.NetDeliveryMethod.ReliableOrdered, ChannelTypes.WORLD);
        }
    }
}