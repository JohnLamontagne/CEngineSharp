using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
using SharpNetty;
using System;

namespace CEngineSharp_Server.Net.Packets
{
    public class ChatMessagePacket : Packet
    {
        public void WriteData(string message)
        {
            this.PacketBuffer.Flush();

            this.PacketBuffer.WriteString(message);
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            // todo Get the chat message type.

            string chatMessage = PlayerManager.GetPlayer(socketIndex).Name + " says: " + this.PacketBuffer.ReadString();

            this.WriteData(chatMessage);

            PlayerManager.GetPlayer(socketIndex).Map.SendPacket(this);
        }

        public override string PacketID
        {
            get { return "ChatMessage"; }
        }
    }
}