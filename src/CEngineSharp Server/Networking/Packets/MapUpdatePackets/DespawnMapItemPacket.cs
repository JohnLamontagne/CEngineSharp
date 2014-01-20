using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Maps;
using CEngineSharp_Utilities;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Networking.Packets.MapUpdatePackets
{
    internal class DespawnMapItemPacket : Packet
    {
        public void WriteData(MapItem mapItem)
        {
            try
            {
                this.DataBuffer.WriteVector(mapItem.Position);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);
            }
        }

        public override void Execute(Netty netty)
        {
            // Server->Client only.
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.DespawnMapItemPacket; }
        }
    }
}