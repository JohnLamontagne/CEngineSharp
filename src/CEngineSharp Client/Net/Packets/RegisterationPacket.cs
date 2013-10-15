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

                // Problem #1 is that I store the graphics class through a static reference in Program
                //Program.GameGraphics.LoadGameTextures();

                //GameWorld.Players.Add(Globals.MyIndex, new Player(Program.GameGraphics.CharacterTextures["Bob"]));
            }
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