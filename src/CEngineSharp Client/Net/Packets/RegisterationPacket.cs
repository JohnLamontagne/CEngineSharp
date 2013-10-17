using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using SharpNetty;
using System;

namespace CEngineSharp_Client.Net.Packets
{
    public class RegisterationPacket : Packet
    {
        public override void Execute(Netty netty, int socketIndex)
        {
            // Alright, so when the player rec. the go ahead to get into the game, this code will go through
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

        public void WriteData(string user, string pass)
        {
            this.PacketBuffer.WriteString(user);
            this.PacketBuffer.WriteString(pass);
        }

        public override string PacketID
        {
            get { return "Registeration"; }
        }
    }
}