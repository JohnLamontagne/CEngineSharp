using CEngineSharp_Client.Graphicss;
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

                Program.CurrentRenderer = new GameRenderer(Program.CurrentRenderer.GetWindow());

                //Program.GameGraphics.LoadGameTextures();

                //GameWorld.Players.Add(Globals.MyIndex, new Player(Program.GameGraphics.CharacterTextures["Bob"]));
            }

            //Program.GameGraphics.SetMenuStatus(this.PacketBuffer.ReadString());
        }

        public override string PacketID
        {
            get { return "Login"; }
        }
    }
}