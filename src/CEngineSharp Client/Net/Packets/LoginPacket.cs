using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class LoginPacket : Packet
    {
        public void WriteData(string username, string password)
        {
            this.PacketBuffer.WriteString(username);
            this.PacketBuffer.WriteString(password);
        }

        public override void Execute(Netty netty, int socketIndex)
        {
            if (this.PacketBuffer.ReadByte() == 1)
            {
                Globals.MyIndex = this.PacketBuffer.ReadInteger();

                RenderManager.SetRenderState(RenderStates.Render_Game);

                GameRenderer gameRenderer = RenderManager.CurrentRenderer as GameRenderer;

                gameRenderer.LoadGameTextures();

                GameWorld.Players.Add(Globals.MyIndex, new Player(gameRenderer.CharacterTextures["Bob"]));

                return;
            }

            MenuRenderer menuRenderer = RenderManager.CurrentRenderer as MenuRenderer;

            menuRenderer.SetMenuStatus(this.PacketBuffer.ReadString());
        }

        public override string PacketID
        {
            get { return "Login"; }
        }
    }
}