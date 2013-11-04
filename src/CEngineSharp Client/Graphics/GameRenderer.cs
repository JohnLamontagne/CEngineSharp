using CEngineSharp_Client.Graphics.TextureManager;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.Net.Packets;
using CEngineSharp_Client.World;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Client.World.Entity;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using TGUI;

namespace CEngineSharp_Client.Graphics
{
    public class GameRenderer : Renderer
    {
        private int fpsCounter;
        private int fpsTimer;

        public GameTextureManager TextureManager { get; private set; }

        private Sprite testSprite;

        public GameRenderer(RenderWindow window)
            : base(window)
        {
            _window.KeyPressed += _window_KeyPressed;
            _window.KeyReleased += _window_KeyReleased;

            this.TextureManager = new GameTextureManager();
            this.TextureManager.LoadTextures();
        }

        private void _window_KeyReleased(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Up:
                    if (GameWorld.Players[Globals.MyIndex].Direction == Directions.Up)
                    {
                        GameWorld.Players[Globals.MyIndex].IsMoving = false;
                    }
                    break;

                case Keyboard.Key.Down:
                    if (GameWorld.Players[Globals.MyIndex].Direction == Directions.Down)
                    {
                        GameWorld.Players[Globals.MyIndex].IsMoving = false;
                    }
                    break;

                case Keyboard.Key.Right:
                    if (GameWorld.Players[Globals.MyIndex].Direction == Directions.Right)
                    {
                        GameWorld.Players[Globals.MyIndex].IsMoving = false;
                    }
                    break;

                case Keyboard.Key.Left:
                    if (GameWorld.Players[Globals.MyIndex].Direction == Directions.Left)
                    {
                        GameWorld.Players[Globals.MyIndex].IsMoving = false;
                    }
                    break;
            }
        }

        private void _window_KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Up:
                    this.MovePlayer(Directions.Up);
                    break;

                case Keyboard.Key.Down:
                    this.MovePlayer(Directions.Down);
                    break;

                case Keyboard.Key.Right:
                    this.MovePlayer(Directions.Right);
                    break;

                case Keyboard.Key.Left:
                    this.MovePlayer(Directions.Left);
                    break;
            }
        }

        private void MovePlayer(Directions direction)
        {
            GameWorld.Players[Globals.MyIndex].Direction = direction;
            GameWorld.Players[Globals.MyIndex].IsMoving = true;
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
            messageBoxAlert.ClosedCallback += messageBoxAlert_ClosedCallback;
            messageBoxAlert.Add(new Label(), "labelAlert");
            messageBoxAlert.Size = new Vector2f(400, 100);

            Label labelFps = _gui.Add(new Label(themeConfigurationPath), "labelFps");
            labelFps.TextSize = 30;
            labelFps.TextColor = Color.Black;
            labelFps.Position = new Vector2f(10, 10);
        }

        private void messageBoxAlert_ClosedCallback(object sender, CallbackArgs e)
        {
            RenderManager.SetRenderState(RenderStates.Render_Menu);
            Networking.Disconnect();
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

            if (MapManager.Map != null)
                MapManager.Map.Draw(_window);

            this.RenderPlayers();

            _gui.Draw();

            _window.Display();

            if (this.fpsTimer < Client.GameTime.GetTotalTimeElapsed())
            {
                _gui.Get<Label>("labelFps").Text = "Fps: " + this.fpsCounter;
                this.fpsCounter = 0;
                this.fpsTimer = (int)Client.GameTime.GetTotalTimeElapsed() + 1000;
            }

            this.fpsCounter++;
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