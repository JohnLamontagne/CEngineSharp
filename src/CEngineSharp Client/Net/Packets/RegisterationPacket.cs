using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Entity;
using SharpNetty;

namespace CEngineSharp_Client.Net.Packets
{
    public class RegisterationPacket : Packet
    {
        public override void Execute(Netty netty, int socketIndex)
        {
            // Alright, so when the player rec. the go ahead to get into the game, this code will go through
            if (this.DataBuffer.ReadByte() == 1)
            {
                Globals.MyIndex = this.DataBuffer.ReadInteger();

                RenderManager.SetRenderState(RenderStates.Render_Game);
            }
            else
            {
                MenuRenderer menuRenderer = RenderManager.CurrentRenderer as MenuRenderer;

                menuRenderer.SetMenuStatus(this.DataBuffer.ReadString());
            }
        }

        public void WriteData(string user, string pass)
        {
            this.DataBuffer.WriteString(user);
            this.DataBuffer.WriteString(pass);
        }

        public override string PacketID
        {
            get { return "Registeration"; }
        }
    }
}