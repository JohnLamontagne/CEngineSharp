using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.World;
using SharpNetty;

namespace CEngineSharp_Client.Net
{
    public static class Networking
    {
        private static NettyClient nettyClient;

        public static bool Connect()
        {
            nettyClient = new NettyClient(true);

            if (nettyClient.Connected)
                nettyClient.Disconnect();

            nettyClient.Handle_ConnectionLost = Handle_ConnectionLost;

            return nettyClient.Connect("127.0.0.1", 4000, 1);
        }

        public static void Disconnect()
        {
            nettyClient.Disconnect();
            GameWorld.ClearPlayers();
            Globals.InGame = false;
        }

        private static void Handle_ConnectionLost()
        {
            nettyClient = null;
            GameWorld.ClearPlayers();

            RenderManager.SetRenderState(RenderStates.Render_Menu);
        }

        public static void SendPacket(Packet packet)
        {
            nettyClient.SendPacket(packet);
        }
    }
}