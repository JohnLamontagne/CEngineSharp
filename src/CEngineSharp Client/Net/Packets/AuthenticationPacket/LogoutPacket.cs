using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Utilities;
using SharpNetty;

namespace CEngineSharp_Client.Net.Packets.AuthenticationPacket
{
    public class LogoutPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            int playerIndex = this.DataBuffer.ReadInteger();

            if (PlayerManager.MyIndex == playerIndex)
            {
                RenderManager.Instance.RenderState = RenderStates.RenderMenu;

                PlayerManager.ClearPlayers();

                return;
            }

            PlayerManager.RemovePlayer(playerIndex);
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.LogoutPacket; }
        }
    }
}