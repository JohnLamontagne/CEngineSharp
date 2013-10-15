using CEngineSharp_Client.Net;
using CEngineSharp_Client.Net.Packets;
using CEngineSharp_Client.World;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGUI;

namespace CEngineSharp_Client.Graphics
{
    public class GameRenderer : Renderer
    {
        public Dictionary<string, Texture> CharacterTextures;

        public GameRenderer(RenderWindow window)
            : base(window)
        {
            this.CharacterTextures = new Dictionary<string, Texture>();
        }

        protected override void LoadInterface()
        {
            _gui.RemoveAllWidgets();

            ChatBox textChat = _gui.Add(new ChatBox(themeConfigurationPath), "textChat");
            textChat.Position = new Vector2f(5, 350);
            textChat.Size = new Vector2f(400, 200);
            textChat.Transparency = 150;

            EditBox textMyChat = _gui.Add(new EditBox(themeConfigurationPath), "textMyChat");
            textMyChat.Position = new Vector2f(5, textChat.Position.Y + textChat.Size.Y + 5);
            textMyChat.Size = new Vector2f(textChat.Size.X, 40);
            textMyChat.ReturnKeyPressedCallback += textMyChat_ReturnKeyPressedCallback;
            textMyChat.Transparency = 150;

            MessageBox messageBoxAlert = _gui.Add(new MessageBox(themeConfigurationPath), "messageBoxAlert");
            messageBoxAlert.Visible = false;

            messageBoxAlert.Add(new Label(), "labelAlert");
            messageBoxAlert.Size = new Vector2f(400, 100);
        }

        public void LoadGameTextures()
        {
            DirectoryInfo characterDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/Data/Graphics/Characters/");

            foreach (var file in characterDir.GetFiles("*.png", SearchOption.AllDirectories))
            {
                this.CharacterTextures.Add(file.Name.Replace(".png", ""), new Texture(file.FullName));
            }
        }

        private void textMyChat_ReturnKeyPressedCallback(object sender, CallbackArgs e)
        {
            var textMyChat = _gui.Get<EditBox>("textMyChat");
            var chatMessagePacket = new ChatMessagePacket();

            chatMessagePacket.WriteData(textMyChat.Text);
            Networking.SendPacket(chatMessagePacket);
            textMyChat.Text = "";
        }

        public override void Render()
        {
            _window.DispatchEvents();

            _window.Clear();

            this.RenderPlayers();

            _gui.Draw();

            _window.Display();
        }

        private void RenderPlayers()
        {
            foreach (var player in GameWorld.Players)
            {
                player.Value.Draw(_window);
            }
        }

        public void AddChatMessage(string message, Color color)
        {
            _gui.Get<ChatBox>("textChat").AddLine(message, color);
        }

        public void DisplayAlert(string title, string alertMessage, int x, int y, Color color)
        {
            var messageBoxAlert = _gui.Get<MessageBox>("messageBoxAlert");

            messageBoxAlert.Title = title;
            messageBoxAlert.Position = new Vector2f(x, y);
            messageBoxAlert.TextColor = color;
            messageBoxAlert.Visible = true;
            messageBoxAlert.Get<Label>("labelAlert").Text = alertMessage;
            messageBoxAlert.Get<Label>("labelAlert").TextSize = 20;
            messageBoxAlert.Get<Label>("labelAlert").Position = new Vector2f(40, 50);
        }
    }
}