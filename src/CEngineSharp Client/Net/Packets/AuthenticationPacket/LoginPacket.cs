using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Utilities;
using SharpNetty;
using System.Diagnostics;

namespace CEngineSharp_Client.Net.Packets.AuthenticationPacket
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

                RenderManager.Instance.RenderState = RenderStates.RenderGame;
            }
            else
            {
                var menuRenderer = RenderManager.Instance.CurrentRenderer as MenuRenderer;


                // This should NEVER be true if all is well.
                Debug.Assert(menuRenderer != null, "Graphical transition error!");
                menuRenderer.SetMenuStatus(this.DataBuffer.ReadString());

                NetManager.Instance.Disconnect();
            }
        }

        public override int PacketID
        {
            get { return (int)PacketTypes.LoginPacket; }
        }
    }
}