using CEngineSharp_Client.Audio;
using CEngineSharp_Client.Graphics.TextureManager;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.Networking;
using CEngineSharp_Client.Utilities;
using CEngineSharp_Client.World.Entity;
using CEngineSharp_Utilities;
using Lidgren.Network;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using TGUI;

namespace CEngineSharp_Client.Graphics
{
    public class GameScreen : Screen
    {
        private int _fpsCounter;
        private int _fpsTimer;
        private bool _inventoryVisible;

        private View _mainView;

        public GameScreen(RenderWindow window)
            : base(window)
        {
            Window.KeyPressed += _window_KeyPressed;
            Window.KeyReleased += _window_KeyReleased;
            Window.Resized += _window_Resized;
            Window.MouseButtonPressed += _window_MouseButtonPressed;

            this.CanRender = true;

            AudioManager.Instance.MusicManager.StopMusic();

            _mainView = this.Window.GetView();

            var net = ServiceLocator.NetManager;
            net.AddPacketHandler(PacketType.AlertMessagePacket, this.HandleAlertMessage);
            net.AddPacketHandler(PacketType.ChatMessagePacket, this.HandleChatMessage);

            this.TextureManager = new GameTextureManager();
            this.TextureManager.LoadTextures();

            this.LoadInterface();
        }

        private void HandleAlertMessage(PacketReceivedEventArgs args)
        {
            var alertTitle = args.Message.ReadString();
            var alertMessage = args.Message.ReadString();
            var alertX = args.Message.ReadInt32();
            var alertY = args.Message.ReadInt32();
            var r = args.Message.ReadByte();
            var g = args.Message.ReadByte();
            var b = args.Message.ReadByte();
            var alertColor = new Color(r, g, b);

            this.DisplayAlert(alertTitle, alertMessage, alertX, alertY, alertColor);
        }

        private void HandleChatMessage(PacketReceivedEventArgs args)
        {
            var message = args.Message.ReadString();

            this.AddChatMessage(message, Color.White);
        }

        public override void Render(GameTime gameTime)
        {
            Window.DispatchEvents();

            Window.Clear();

            if (Client.InGame)
            {
                if (ServiceLocator.WorldManager.MapManager.Map != null)
                {
                    this.Window.SetView(ServiceLocator.WorldManager.PlayerManager.GetPlayer(ServiceLocator.WorldManager.PlayerManager.ClientID).Camera.View);
                    ServiceLocator.WorldManager.MapManager.Map.Draw(Window);
                    this.Window.SetView(_mainView);
                }

                this.GUI.Draw();

                if (this._inventoryVisible)
                    ServiceLocator.WorldManager.PlayerManager.GetPlayer(ServiceLocator.WorldManager.PlayerManager.ClientID).DrawInventory(Window);

                var healthBar = this.GUI.Get<LoadingBar>("healthBar");
                if (ServiceLocator.WorldManager.PlayerManager.GetPlayer(ServiceLocator.WorldManager.PlayerManager.ClientID).Hp != healthBar.Value)
                {
                    healthBar.Value = ServiceLocator.WorldManager.PlayerManager.GetPlayer(ServiceLocator.WorldManager.PlayerManager.ClientID).Hp;
                    healthBar.Text = "HP: " + healthBar.Value + "/" + healthBar.Maximum;
                }
            }

            Window.Display();

            #region Fps Logic

            if (this._fpsTimer < gameTime.TotalElapsedTime)
            {
                this.GUI.Get<Label>("labelFps").Text = "Fps: " + _fpsCounter;
                this._fpsCounter = 0;
                this._fpsTimer = (int)gameTime.TotalElapsedTime + 1000;
            }

            this._fpsCounter++;

            #endregion Fps Logic
        }

        public void AddChatMessage(string message, Color color)
        {
            try
            {
                var chatBox = this.GUI.Get<ChatBox>("textChat");

                chatBox.AddLine(message, color);
            }
            catch (Exception)
            {
            }
        }

        public void DisplayAlert(string title, string alertMessage, int x, int y, Color color)
        {
            var messageBoxAlert = this.GUI.Get<MessageBox>("messageBoxAlert");

            messageBoxAlert.Title = title;
            messageBoxAlert.Position = new Vector2f(x, y);
            messageBoxAlert.TextColor = color;
            messageBoxAlert.Visible = true;
            messageBoxAlert.Get<Label>("labelAlert").Text = alertMessage;
            messageBoxAlert.Get<Label>("labelAlert").TextSize = 20;
            messageBoxAlert.Get<Label>("labelAlert").Position = new Vector2f(40, 50);
        }

        protected override void LoadInterface()
        {
            this.GUI.RemoveAllWidgets();

            Color color = Color.Black;
            color.A = 100;
            ChatBox textChat = this.GUI.Add(new ChatBox(Constants.FILEPATH_THEME_CONFIG), "textChat");
            textChat.Position = new Vector2f(5, 350);
            textChat.Size = new Vector2f(400, 200);
            textChat.BackgroundColor = color;

            EditBox textMyChat = this.GUI.Add(new EditBox(Constants.FILEPATH_THEME_CONFIG), "textMyChat");
            textMyChat.Position = new Vector2f(5, textChat.Position.Y + textChat.Size.Y + 5);
            textMyChat.Size = new Vector2f(textChat.Size.X, 40);
            textMyChat.ReturnKeyPressedCallback += textMyChat_ReturnKeyPressedCallback;
            textMyChat.Transparency = 150;

            MessageBox messageBoxAlert = this.GUI.Add(new MessageBox(Constants.FILEPATH_THEME_CONFIG), "messageBoxAlert");
            messageBoxAlert.Visible = false;
            messageBoxAlert.ClosedCallback += messageBoxAlert_ClosedCallback;
            messageBoxAlert.Add(new Label(), "labelAlert");
            messageBoxAlert.Size = new Vector2f(400, 100);

            Label labelFps = this.GUI.Add(new Label(Constants.FILEPATH_THEME_CONFIG), "labelFps");
            labelFps.TextSize = 30;
            labelFps.TextColor = Color.Black;
            labelFps.Position = new Vector2f(700, 10);

            Button buttonInventory = this.GUI.Add(new Button(Constants.FILEPATH_THEME_CONFIG), "buttonInventory");
            buttonInventory.Text = "Inventory";
            buttonInventory.Position = new Vector2f(550, 600);
            buttonInventory.Size = new Vector2f(100, 25);
            buttonInventory.Visible = true;
            buttonInventory.LeftMouseClickedCallback += buttonInventory_LeftMouseClickedCallback;

            Button buttonLogout = this.GUI.Add(new Button(Constants.FILEPATH_THEME_CONFIG), "buttonLogout");
            buttonLogout.Text = "Logout";
            buttonLogout.Position = new Vector2f(700, 600);
            buttonLogout.Size = new Vector2f(100, 25);
            buttonLogout.Visible = true;
            buttonLogout.LeftMouseClickedCallback += buttonLogout_LeftMouseClickedCallback;

            Picture picInventory = this.GUI.Add(new Picture(Constants.FILEPATH_GRAPHICS + "/Gui/Inventory.png"), "picInventory");
            picInventory.Position = new Vector2f(500, 400);
            picInventory.LeftMouseClickedCallback += picInventory_LeftMouseClickedCallback;
            picInventory.Visible = false;

            LoadingBar healthBar = this.GUI.Add(new LoadingBar(Constants.FILEPATH_THEME_CONFIG), "healthBar");
            healthBar.Text = "HP";
            healthBar.Size = new Vector2f(300, 20);
            healthBar.Position = new Vector2f(0, 10);
            healthBar.Maximum = 100;
        }

        private void buttonInventory_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            this._inventoryVisible = !this._inventoryVisible;

            this.GUI.Get<Picture>("picInventory").Visible = this._inventoryVisible;
        }

        private void picInventory_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            ServiceLocator.WorldManager.PlayerManager.GetPlayer(ServiceLocator.WorldManager.PlayerManager.ClientID).TryDropInventoryItem((int)e.Mouse.X, (int)e.Mouse.Y);
        }

        private void _window_KeyReleased(object sender, KeyEventArgs e)
        {
            Player player = ServiceLocator.WorldManager.PlayerManager.GetPlayer(ServiceLocator.WorldManager.PlayerManager.ClientID);

            switch (e.Code)
            {
                case Keyboard.Key.Up:
                    if (player != null && player.Direction == Directions.Up)
                        player.IsMoving = false;
                    break;

                case Keyboard.Key.Down:
                    if (player != null && player.Direction == Directions.Down)
                        player.IsMoving = false;
                    break;

                case Keyboard.Key.Right:
                    if (player != null && player.Direction == Directions.Right)
                        player.IsMoving = false;
                    break;

                case Keyboard.Key.Left:
                    if (player != null && player.Direction == Directions.Left)
                        player.IsMoving = false;
                    break;
            }
        }

        private void _window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (!Client.InGame) return;

            var player = ServiceLocator.WorldManager.PlayerManager.GetPlayer(ServiceLocator.WorldManager.PlayerManager.ClientID);

            switch (e.Code)
            {
                case Keyboard.Key.Up:

                    player.Direction = Directions.Up;
                    player.IsMoving = true;
                    break;

                case Keyboard.Key.Down:
                    player.Direction = Directions.Down;
                    player.IsMoving = true;
                    break;

                case Keyboard.Key.Right:
                    player.Direction = Directions.Right;
                    player.IsMoving = true;
                    break;

                case Keyboard.Key.Left:
                    player.Direction = Directions.Left;
                    player.IsMoving = true;
                    break;

                case Keyboard.Key.Space:
                    ServiceLocator.WorldManager.MapManager.Map.TryPickupItem();
                    break;
            }
        }

        private void messageBoxAlert_ClosedCallback(object sender, CallbackArgs e)
        {
            ServiceLocator.ScreenManager.SetActiveScreen("mainMenu");
            ServiceLocator.NetManager.Disconnect();
        }

        private void buttonLogout_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            ServiceLocator.ScreenManager.SetActiveScreen("mainMenu");
            ServiceLocator.NetManager.Disconnect();
        }

        private void textMyChat_ReturnKeyPressedCallback(object sender, CallbackArgs e)
        {
            var textMyChat = this.GUI.Get<EditBox>("textMyChat");
            var chatMessagePacket = new Packet(PacketType.ChatMessagePacket);

            chatMessagePacket.Message.Write(textMyChat.Text);
            ServiceLocator.NetManager.SendMessage(chatMessagePacket.Message, NetDeliveryMethod.Unreliable, ChannelTypes.CHAT);
            textMyChat.Text = "";
        }

        private void _window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
        }

        private void _window_Resized(object sender, SizeEventArgs e)
        {
            //this.Window.SetView(new View(new FloatRect(0f, 0f, e.Width, e.Height)));

            //RenderManager.Instance.CurrentResolutionWidth = (int)Window.GetView().Size.X;
            //RenderManager.Instance.CurrentResolutionHeight = (int)Window.GetView().Size.Y;

            //PlayerManager.GetPlayer(PlayerManager.MyIndex).Camera.SetView(this.Window.GetView());
        }
    }
}