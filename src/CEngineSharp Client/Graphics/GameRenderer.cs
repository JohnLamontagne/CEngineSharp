using CEngineSharp_Client.Net;
using CEngineSharp_Client.Net.Packets;
using CEngineSharp_Client.Net.Packets.SocialPackets;
using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Client.World.Entity;
using SFML.Graphics;
using SFML.Window;
using TGUI;

namespace CEngineSharp_Client.Graphics
{
    public class GameRenderer : Renderer
    {
        private int _fpsCounter;
        private int _fpsTimer;
        private bool _inventoryVisible;


        public GameRenderer(RenderWindow window)
            : base(window)
        {
            Window.KeyPressed += _window_KeyPressed;
            Window.KeyReleased += _window_KeyReleased;
            Window.Resized += _window_Resized;
            Window.MouseButtonPressed += _window_MouseButtonPressed;

            RenderManager.Instance.CurrentResolutionWidth = (int)Window.GetView().Size.X;
            RenderManager.Instance.CurrentResolutionHeight = (int)Window.GetView().Size.Y;

            this.CanRender = true;
        }

        public override void Render(GameTime gameTime)
        {
            Window.DispatchEvents();

            Window.Clear();

            if (Client.InGame)
            {
                if (MapManager.Map != null)
                    MapManager.Map.Draw(Window);

                this.Gui.Draw();

                if (this._inventoryVisible)
                    PlayerManager.GetPlayer(PlayerManager.MyIndex).DrawInventory(Window);

                var healthBar = this.Gui.Get<LoadingBar>("healthBar");
                if (PlayerManager.GetPlayer(PlayerManager.MyIndex).Hp != healthBar.Value)
                {
                    healthBar.Value = PlayerManager.GetPlayer(PlayerManager.MyIndex).Hp;
                    healthBar.Text = "HP: " + healthBar.Value + "/" + healthBar.Maximum;
                }
            }

            Window.Display();

            #region Fps Logic

            if (this._fpsTimer < gameTime.TotalElapsedTime)
            {
                this.Gui.Get<Label>("labelFps").Text = "Fps: " + _fpsCounter;
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
                var chatBox = this.Gui.Get<ChatBox>("textChat");

                chatBox.AddLine(message, color);
            }
            catch (Exception)
            {
            }
        }

        public void DisplayAlert(string title, string alertMessage, int x, int y, Color color)
        {
            var messageBoxAlert = this.Gui.Get<MessageBox>("messageBoxAlert");

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
            this.Gui.RemoveAllWidgets();

            Color color = Color.Black;
            color.A = 100;
            ChatBox textChat = this.Gui.Add(new ChatBox(ThemeConfigurationPath), "textChat");
            textChat.Position = new Vector2f(5, 350);
            textChat.Size = new Vector2f(400, 200);
            textChat.BackgroundColor = color;

            EditBox textMyChat = this.Gui.Add(new EditBox(ThemeConfigurationPath), "textMyChat");
            textMyChat.Position = new Vector2f(5, textChat.Position.Y + textChat.Size.Y + 5);
            textMyChat.Size = new Vector2f(textChat.Size.X, 40);
            textMyChat.ReturnKeyPressedCallback += textMyChat_ReturnKeyPressedCallback;
            textMyChat.Transparency = 150;

            MessageBox messageBoxAlert = this.Gui.Add(new MessageBox(ThemeConfigurationPath), "messageBoxAlert");
            messageBoxAlert.Visible = false;
            messageBoxAlert.ClosedCallback += messageBoxAlert_ClosedCallback;
            messageBoxAlert.Add(new Label(), "labelAlert");
            messageBoxAlert.Size = new Vector2f(400, 100);

            Label labelFps = this.Gui.Add(new Label(ThemeConfigurationPath), "labelFps");
            labelFps.TextSize = 30;
            labelFps.TextColor = Color.Black;
            labelFps.Position = new Vector2f(700, 10);

            Button buttonInventory = this.Gui.Add(new Button(ThemeConfigurationPath), "buttonInventory");
            buttonInventory.Text = "Inventory";
            buttonInventory.Position = new Vector2f(550, 600);
            buttonInventory.Size = new Vector2f(100, 25);
            buttonInventory.Visible = true;
            buttonInventory.LeftMouseClickedCallback += buttonInventory_LeftMouseClickedCallback;


            Button buttonLogout = this.Gui.Add(new Button(ThemeConfigurationPath), "buttonLogout");
            buttonLogout.Text = "Logout";
            buttonLogout.Position = new Vector2f(700, 600);
            buttonLogout.Size = new Vector2f(100, 25);
            buttonLogout.Visible = true;
            buttonLogout.LeftMouseClickedCallback += buttonLogout_LeftMouseClickedCallback;

            Picture picInventory = this.Gui.Add(new Picture(Constants.FILEPATH_GRAPHICS + "/Gui/Inventory.png"), "picInventory");
            picInventory.Position = new Vector2f(500, 400);
            picInventory.LeftMouseClickedCallback += picInventory_LeftMouseClickedCallback;
            picInventory.Visible = false;

            LoadingBar healthBar = this.Gui.Add(new LoadingBar(ThemeConfigurationPath), "healthBar");
            healthBar.Text = "HP";
            healthBar.Size = new Vector2f(300, 20);
            healthBar.Position = new Vector2f(0, 10);
            healthBar.Maximum = 100;


        }

        private void buttonInventory_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            this._inventoryVisible = !this._inventoryVisible;

            this.Gui.Get<Picture>("picInventory").Visible = this._inventoryVisible;

        }

        private void picInventory_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            PlayerManager.GetPlayer(PlayerManager.MyIndex).TryDropInventoryItem((int)e.Mouse.X, (int)e.Mouse.Y);
        }

        private void _window_KeyReleased(object sender, KeyEventArgs e)
        {
            Player player = PlayerManager.GetPlayer(PlayerManager.MyIndex);

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

            var player = PlayerManager.GetPlayer(PlayerManager.MyIndex);

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
                    MapManager.Map.TryPickupItem();
                    break;

                case Keyboard.Key.LShift:
                    var mapNpc = MapManager.Map.GetMapNpc(0);
                    mapNpc.Move(mapNpc.X + 1, mapNpc.Y, Directions.Right);
                    break;
            }
        }

        private void messageBoxAlert_ClosedCallback(object sender, CallbackArgs e)
        {
            RenderManager.Instance.RenderState = RenderStates.RenderMenu;
            NetManager.Instance.Disconnect();
        }

        private void buttonLogout_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {

            NetManager.Instance.Disconnect();
        }

        private void textMyChat_ReturnKeyPressedCallback(object sender, CallbackArgs e)
        {
            var textMyChat = this.Gui.Get<EditBox>("textMyChat");
            var chatMessagePacket = new ChatMessagePacket();

            chatMessagePacket.WriteData(textMyChat.Text);
            NetManager.Instance.SendPacket(chatMessagePacket);
            textMyChat.Text = "";
        }

        private void _window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
        }

        private void _window_Resized(object sender, SizeEventArgs e)
        {
            RenderManager.Instance.CurrentResolutionHeight = (int)e.Height;
            RenderManager.Instance.CurrentResolutionWidth = (int)e.Width;
        }
    }
}