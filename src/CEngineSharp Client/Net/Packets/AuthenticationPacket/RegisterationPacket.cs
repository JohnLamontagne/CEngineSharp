using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Utilities;
using SharpNetty;
using System.Diagnostics;

namespace CEngineSharp_Client.Net.Packets.AuthenticationPacket
{
    public class RegisterationPacket : Packet
    {
        public override void Execute(Netty netty)
        {
            // Alright, so when the player rec. the go ahead to get into the game, this code will go through
            if (this.DataBuffer.ReadByte() == 1)
            {
                PlayerManager.MyIndex = this.DataBuffer.ReadInteger();

                RenderManager.Instance.RenderState = RenderStates.RenderGame;
            }

            else
            {
                var menuRenderer = RenderManager.Instance.CurrentRenderer as MenuRenderer;

                // This should NEVER be true if all is well.
                Debug.Assert(menuRenderer != null, "Graphical transition error!");
                menuRenderer.SetMenuStatus(this.DataBuffer.ReadString());
            }
        }

        public void WriteData(string user, string pass)
        {
            this.DataBuffer.WriteString(user);
            this.DataBuffer.WriteString(pass);
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.RegistrationPacket; }
        }
    }
}