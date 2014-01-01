using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Client.World.Entity;
using SharpNetty;

namespace CEngineSharp_Client.Net.Packets
{
    public class LoginPacket : Packet
    {
        public void WriteData(string username, string password)
        {
            this.DataBuffer.WriteString(username);
            this.DataBuffer.WriteString(password);
        }

        public override void Execute(Netty netty)
        {
            if (this.DataBuffer.ReadByte() == 1)
            {
                PlayerManager.MyIndex = this.DataBuffer.ReadInteger();

                RenderManager.RenderState = RenderStates.Render_Game;
            }
            else
            {
                MenuRenderer menuRenderer = RenderManager.CurrentRenderer as MenuRenderer;

                menuRenderer.SetMenuStatus(this.DataBuffer.ReadString());

                Networking.Disconnect();
            }
        }

        public override int PacketID
        {
            get { return 3; }
        }
    }
}