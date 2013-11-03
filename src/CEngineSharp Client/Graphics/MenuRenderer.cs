using CEngineSharp_Client.Graphics.TextureManager;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.Net.Packets;
using SFML.Graphics;
using SFML.Window;
using System;
using System.IO;
using TGUI;

namespace CEngineSharp_Client.Graphics
{
    public class MenuRenderer : Renderer
    {
        public MenuTextureManager TextureManager { get; private set; }

        private Sprite menuBackground;

        public MenuRenderer(RenderWindow window)
            : base(window)
        {
            this.TextureManager = new MenuTextureManager();
            this.TextureManager.LoadTextures();

            this.menuBackground = new Sprite(this.TextureManager.MenuBackgroundTexture);
        }

        public MenuRenderer()
        {
            this.TextureManager = new MenuTextureManager();
            this.TextureManager.LoadTextures();

            this.menuBackground = new Sprite(this.TextureManager.MenuBackgroundTexture);
        }

        protected override void LoadInterface()
        {
            #region Status Label

            Label labelStatus = _gui.Add(new Label(), "labelStatus");
            labelStatus.Position = new Vector2f((_window.Size.X / 2) - (labelStatus.Size.X / 2), 150);
            labelStatus.Visible = false;

            #endregion Status Label

            #region News Label

            Label labelNews = _gui.Add(new Label(), "labelNews");
            LoadNews(labelNews);
            labelNews.Position = new Vector2f((_window.Size.X / 2) - (labelNews.Size.X / 2), 200);

            #endregion News Label

            #region Login Button

            Button loginButton = _gui.Add(new Button(themeConfigurationPath), "buttonLogin");
            loginButton.Position = new Vector2f(labelNews.Position.X, 500);
            loginButton.Text = "Login";
            loginButton.LeftMouseClickedCallback += buttonLogin_LeftMouseClickedCallback;

            #endregion Login Button

            #region Registration Button

            Button registrationButton = _gui.Add(new Button(themeConfigurationPath), "buttonRegistration");
            registrationButton.Position = new Vector2f(labelNews.Position.X + (labelNews.Size.X - registrationButton.Size.X), 500);
            registrationButton.Text = "Register";
            registrationButton.LeftMouseClickedCallback += buttonRegistration_LeftMouseClickedCallback;

            #endregion Registration Button

            #region User Label

            Label labelUsername = _gui.Add(new Label(), "labelUsername");
            labelUsername.Position = new Vector2f(100, 100);
            labelUsername.Text = "Username: ";
            labelUsername.Visible = false;

            #endregion User Label

            #region Password Label

            Label labelPassword = _gui.Add(new Label(), "labelPassword");
            labelPassword.Position = new Vector2f(100, 160);
            labelPassword.Text = "Password: ";
            labelPassword.Visible = false;

            #endregion Password Label

            #region Text User

            EditBox textUser = _gui.Add(new EditBox(themeConfigurationPath), "textUser");
            textUser.Position = new Vector2f(255, 90);
            textUser.Size = new Vector2f(340, 40);
            textUser.Visible = false;

            #endregion Text User

            #region Text Password

            EditBox textPassword = _gui.Add(new EditBox(themeConfigurationPath), "textPassword");
            textPassword.Position = new Vector2f(255, 150);
            textPassword.Size = new Vector2f(340, 40);
            textPassword.PasswordCharacter = "*";
            textPassword.Visible = false;

            #endregion Text Password

            #region SendLogin Button

            Button buttonSendLogin = _gui.Add(new Button(themeConfigurationPath), "buttonSendLogin");
            buttonSendLogin.Position = new Vector2f(300, 250);
            buttonSendLogin.Text = "Login!";
            buttonSendLogin.LeftMouseClickedCallback += buttonSendLogin_LeftMouseClickedCallback;
            buttonSendLogin.Visible = false;

            #endregion SendLogin Button

            #region SendRegistration Button

            Button buttonSendRegistration = _gui.Add(new Button(themeConfigurationPath), "buttonSendRegistration");
            buttonSendRegistration.Position = new Vector2f(300, 250);
            buttonSendRegistration.Text = "Register!";
            buttonSendRegistration.LeftMouseClickedCallback += buttonSendRegistartion_LeftMouseClickedCallback;
            buttonSendRegistration.Visible = false;

            #endregion SendRegistration Button

            #region Back Button

            Button buttonBack = _gui.Add(new Button(themeConfigurationPath), "buttonBack");
            buttonBack.Position = new Vector2f(300, 400);
            buttonBack.Visible = false;
            buttonBack.Text = "Back";
            buttonBack.LeftMouseClickedCallback += buttonBack_LeftMouseClickedCallback;

            #endregion Back Button
        }

        private void buttonSendRegistartion_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            Networking.Connect();

            var registrationPacket = new RegisterationPacket();
            registrationPacket.WriteData(_gui.Get<EditBox>("textUser").Text, _gui.Get<EditBox>("textPassword").Text);

            Networking.SendPacket(registrationPacket);
        }

        private void buttonBack_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            _gui.Get<Button>("buttonBack").Visible = false;
            _gui.Get<Button>("buttonSendRegistration").Visible = false;
            _gui.Get<Button>("buttonSendLogin").Visible = false;
            _gui.Get<EditBox>("textUser").Visible = false;
            _gui.Get<EditBox>("textPassword").Visible = false;
            _gui.Get<Label>("labelUsername").Visible = false;
            _gui.Get<Label>("labelPassword").Visible = false;
            _gui.Get<Label>("labelNews").Visible = true;
            _gui.Get<Button>("buttonLogin").Visible = true;
            _gui.Get<Button>("buttonRegistration").Visible = true;
        }

        private void buttonRegistration_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            _gui.Get<Button>("buttonSendRegistration").Visible = true;
            _gui.Get<Label>("labelUsername").Visible = true;
            _gui.Get<Label>("labelPassword").Visible = true;
            _gui.Get<Label>("labelStatus").Visible = true;
            _gui.Get<EditBox>("textUser").Visible = true;
            _gui.Get<EditBox>("textPassword").Visible = true;
            _gui.Get<Label>("labelNews").Visible = false;
            _gui.Get<Button>("buttonLogin").Visible = false;
            _gui.Get<Button>("buttonRegistration").Visible = false;
            _gui.Get<Button>("buttonBack").Visible = true;
        }

        private void buttonLogin_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            _gui.Get<Button>("buttonSendLogin").Visible = true;
            _gui.Get<Label>("labelUsername").Visible = true;
            _gui.Get<Label>("labelPassword").Visible = true;
            _gui.Get<Label>("labelStatus").Visible = true;
            _gui.Get<EditBox>("textUser").Visible = true;
            _gui.Get<EditBox>("textPassword").Visible = true;
            _gui.Get<Label>("labelNews").Visible = false;
            _gui.Get<Button>("buttonLogin").Visible = false;
            _gui.Get<Button>("buttonRegistration").Visible = false;
            _gui.Get<Button>("buttonBack").Visible = true;
        }

        private void buttonSendLogin_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            _gui.Get<Label>("labelStatus").Text = "Connecting to the server...";
            _gui.Get<Label>("labelStatus").Position = new Vector2f((_window.Size.X / 2) - (_gui.Get<Label>("labelStatus").Size.X / 2), 50);
            Networking.Connect();

            _gui.Get<Label>("labelStatus").Text = "Connected! Sending login information...";
            _gui.Get<Label>("labelStatus").Position = new Vector2f((_window.Size.X / 2) - (_gui.Get<Label>("labelStatus").Size.X / 2), 50);

            var loginPacket = new LoginPacket();
            loginPacket.WriteData(_gui.Get<EditBox>("textUser").Text, _gui.Get<EditBox>("textPassword").Text);

            Networking.SendPacket(loginPacket);
        }

        private void LoadNews(Label labelNews)
        {
            string[] news = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "/Data/News.txt");

            foreach (var line in news)
            {
                labelNews.Text += line + "\n";
            }
        }

        public void SetMenuStatus(string status)
        {
            _gui.Get<Label>("labelStatus").Text = status;
            _gui.Get<Label>("labelStatus").Position = new Vector2f((_window.Size.X / 2) - (_gui.Get<Label>("labelStatus").Size.X / 2), 50);
        }

        public override void Render()
        {
            _window.DispatchEvents();

            _window.Clear();

            _window.Draw(this.menuBackground);

            _gui.Draw();

            _window.Display();
        }
    }
}