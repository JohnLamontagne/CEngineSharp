using CEngineSharp_Client.Audio;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.Networking;
using CEngineSharp_Client.Utilities;
using CEngineSharp_Utilities;
using Lidgren.Network;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.IO;
using System.Threading;
using TGUI;

namespace CEngineSharp_Client.Graphics
{
    public class MenuScreen : Screen
    {
        private Sprite _menuBackground;

        public MenuScreen(RenderWindow window)
            : base(window)
        {
            this.CanRender = true;

            this.TextureManager = new MenuTextureManager();
            this.TextureManager.LoadTextures();

            this.LoadInterface();
            // AudioManager.Instance.MusicManager.PlayMusic("MainMenu");
        }

        public MenuScreen()
        {
            this.CanRender = true;

            this.TextureManager = new MenuTextureManager();
            this.TextureManager.LoadTextures();

            //   AudioManager.Instance.MusicManager.PlayMusic("MainMenu");
        }

        protected override void LoadInterface()
        {
            _menuBackground = new Sprite(this.TextureManager.GetTexture("MenuBackground"));

            #region Status Label

            Label labelStatus = this.GUI.Add(new Label(), "labelStatus");
            labelStatus.Position = new Vector2f((Window.Size.X / 2) - (labelStatus.Size.X / 2), 150);
            labelStatus.Visible = false;

            #endregion Status Label

            #region News Label

            Label labelNews = this.GUI.Add(new Label(), "labelNews");
            LoadNews(labelNews);
            labelNews.Position = new Vector2f((Window.Size.X / 2) - (labelNews.Size.X / 2), 200);

            #endregion News Label

            #region Login Button

            Button loginButton = this.GUI.Add(new Button(Constants.FILEPATH_THEME_CONFIG), "buttonLogin");
            loginButton.Position = new Vector2f(100, 500);
            loginButton.Text = "Login";
            loginButton.LeftMouseClickedCallback += buttonLogin_LeftMouseClickedCallback;

            #endregion Login Button

            #region Registration Button

            Button registrationButton = this.GUI.Add(new Button(Constants.FILEPATH_THEME_CONFIG), "buttonRegistration");
            registrationButton.Position = new Vector2f(500, 500);
            registrationButton.Text = "Register";
            registrationButton.LeftMouseClickedCallback += buttonRegistration_LeftMouseClickedCallback;

            #endregion Registration Button

            #region User Label

            Label labelUsername = this.GUI.Add(new Label(), "labelUsername");
            labelUsername.Position = new Vector2f(100, 100);
            labelUsername.Text = "Username: ";
            labelUsername.Visible = false;

            #endregion User Label

            #region Password Label

            Label labelPassword = this.GUI.Add(new Label(), "labelPassword");
            labelPassword.Position = new Vector2f(100, 160);
            labelPassword.Text = "Password: ";
            labelPassword.Visible = false;

            #endregion Password Label

            #region Text User

            EditBox textUser = this.GUI.Add(new EditBox(Constants.FILEPATH_THEME_CONFIG), "textUser");
            textUser.Position = new Vector2f(255, 90);
            textUser.Size = new Vector2f(340, 40);
            textUser.Visible = false;

            #endregion Text User

            #region Text Password

            EditBox textPassword = this.GUI.Add(new EditBox(Constants.FILEPATH_THEME_CONFIG), "textPassword");
            textPassword.Position = new Vector2f(255, 150);
            textPassword.Size = new Vector2f(340, 40);
            textPassword.PasswordCharacter = "*";
            textPassword.Visible = false;

            #endregion Text Password

            #region SendLogin Button

            Button buttonSendLogin = this.GUI.Add(new Button(Constants.FILEPATH_THEME_CONFIG), "buttonSendLogin");
            buttonSendLogin.Position = new Vector2f(300, 250);
            buttonSendLogin.Text = "Login!";
            buttonSendLogin.LeftMouseClickedCallback += buttonSendLogin_LeftMouseClickedCallback;
            buttonSendLogin.Visible = false;

            #endregion SendLogin Button

            #region SendRegistration Button

            Button buttonSendRegistration = this.GUI.Add(new Button(Constants.FILEPATH_THEME_CONFIG), "buttonSendRegistration");
            buttonSendRegistration.Position = new Vector2f(300, 250);
            buttonSendRegistration.Text = "Register!";
            buttonSendRegistration.LeftMouseClickedCallback += buttonSendRegistartion_LeftMouseClickedCallback;
            buttonSendRegistration.Visible = false;

            #endregion SendRegistration Button

            #region Back Button

            Button buttonBack = this.GUI.Add(new Button(Constants.FILEPATH_THEME_CONFIG), "buttonBack");
            buttonBack.Position = new Vector2f(300, 400);
            buttonBack.Visible = false;
            buttonBack.Text = "Back";
            buttonBack.LeftMouseClickedCallback += buttonBack_LeftMouseClickedCallback;

            #endregion Back Button
        }

        public override void Render(GameTime gameTime)
        {
            Window.DispatchEvents();

            Window.Clear();

            Window.Draw(_menuBackground);

            this.GUI.Draw();

            Window.Display();

            Thread.Sleep(1);
        }

        private void LoadNews(Label labelNews)
        {
            string[] news = File.ReadAllLines(Constants.FILEPATH_DATA + "news.txt");

            foreach (var line in news)
            {
                labelNews.Text += line + "\n";
            }
        }

        public void SetMenuStatus(string status)
        {
            this.GUI.Get<Label>("labelStatus").Text = status;
            this.GUI.Get<Label>("labelStatus").Position = new Vector2f((Window.Size.X / 2) - (this.GUI.Get<Label>("labelStatus").Size.X / 2), 50);
        }

        private void buttonSendRegistartion_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            ServiceLocator.NetManager.Connect();

            var registrationPacket = new Packet(PacketType.RegistrationPacket);
            registrationPacket.Message.Write(this.GUI.Get<EditBox>("textUser").Text);
            registrationPacket.Message.Write(this.GUI.Get<EditBox>("textPassword").Text);

            ServiceLocator.NetManager.SendMessage(registrationPacket.Message, NetDeliveryMethod.ReliableOrdered, ChannelTypes.WORLD);
        }

        private void buttonBack_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            this.GUI.Get<Button>("buttonBack").Visible = false;
            this.GUI.Get<Button>("buttonSendRegistration").Visible = false;
            this.GUI.Get<Button>("buttonSendLogin").Visible = false;
            this.GUI.Get<EditBox>("textUser").Visible = false;
            this.GUI.Get<EditBox>("textPassword").Visible = false;
            this.GUI.Get<Label>("labelUsername").Visible = false;
            this.GUI.Get<Label>("labelPassword").Visible = false;
            this.GUI.Get<Label>("labelNews").Visible = true;
            this.GUI.Get<Button>("buttonLogin").Visible = true;
            this.GUI.Get<Button>("buttonRegistration").Visible = true;
        }

        private void buttonRegistration_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            this.GUI.Get<Button>("buttonSendRegistration").Visible = true;
            this.GUI.Get<Label>("labelUsername").Visible = true;
            this.GUI.Get<Label>("labelPassword").Visible = true;
            this.GUI.Get<Label>("labelStatus").Visible = true;
            this.GUI.Get<EditBox>("textUser").Visible = true;
            this.GUI.Get<EditBox>("textPassword").Visible = true;
            this.GUI.Get<Label>("labelNews").Visible = false;
            this.GUI.Get<Button>("buttonLogin").Visible = false;
            this.GUI.Get<Button>("buttonRegistration").Visible = false;
            this.GUI.Get<Button>("buttonBack").Visible = true;
        }

        private void buttonLogin_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            this.GUI.Get<Button>("buttonSendLogin").Visible = true;
            this.GUI.Get<Label>("labelUsername").Visible = true;
            this.GUI.Get<Label>("labelPassword").Visible = true;
            this.GUI.Get<Label>("labelStatus").Visible = true;
            this.GUI.Get<EditBox>("textUser").Visible = true;
            this.GUI.Get<EditBox>("textPassword").Visible = true;
            this.GUI.Get<Label>("labelNews").Visible = false;
            this.GUI.Get<Button>("buttonLogin").Visible = false;
            this.GUI.Get<Button>("buttonRegistration").Visible = false;
            this.GUI.Get<Button>("buttonBack").Visible = true;
        }

        private void buttonSendLogin_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            this.GUI.Get<Label>("labelStatus").Text = "Connecting to the server...";
            this.GUI.Get<Label>("labelStatus").Position = new Vector2f((Window.Size.X / 2) - (this.GUI.Get<Label>("labelStatus").Size.X / 2), 50);

            ServiceLocator.NetManager.Connect();

            this.GUI.Get<Label>("labelStatus").Text = "Connected! Sending login information...";
            this.GUI.Get<Label>("labelStatus").Position = new Vector2f((Window.Size.X / 2) - (this.GUI.Get<Label>("labelStatus").Size.X / 2), 50);

            var loginPacket = new Packet(PacketType.LoginPacket);
            loginPacket.Message.Write(this.GUI.Get<EditBox>("textUser").Text);
            loginPacket.Message.Write(this.GUI.Get<EditBox>("textPassword").Text);

            ServiceLocator.NetManager.SendMessage(loginPacket.Message, NetDeliveryMethod.ReliableOrdered, ChannelTypes.WORLD);
        }
    }
}