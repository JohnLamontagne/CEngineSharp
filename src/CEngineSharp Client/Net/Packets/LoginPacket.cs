using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Entity;
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

                // We need to wait for the transition to occur, otherwise we won't be able to make any valid calls to the Game Renderer.
                while ((RenderManager.CurrentRenderer as GameRenderer) == null)
                {
                    System.Threading.Thread.Sleep(1);
                }

                // It looks like we'll be able to makes calls to the Game Renderer object (finally), let's go ahead and create a reference to it.
                GameRenderer gameRenderer = RenderManager.CurrentRenderer as GameRenderer;

                // Set our player's character-texture.
                GameWorld.Players.Add(Globals.MyIndex, new Player(gameRenderer.TextureManager.GetCharacterTexture("Bob")));

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