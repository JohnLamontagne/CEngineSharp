using CEngineSharp_Client.Net;
using CEngineSharp_Client.Net.Packets;
using CEngineSharp_Client.World;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TGUI;

namespace CEngineSharp_Client
{
    public class Graphics
    {
        private RenderWindow _renderWindow;
        private Gui _gui;

        private string themeConfigurationPath = @"C:\Users\John\Documents\GitHub\CEngineSharp\src\CEngineSharp Client\bin\Debug\Data\Graphics\Gui\Black.conf";

        public Dictionary<string, Texture> CharacterTextures;

        public static RenderStates RenderState;

        public Graphics()
        {
            _renderWindow = new RenderWindow(new VideoMode(800, 600), "CEngine#");
            _renderWindow.SetFramerateLimit(30);
            _gui = new Gui(_renderWindow);
            _gui.GlobalFont = new Font(@"C:\Users\John\Documents\GitHub\CEngineSharp\src\CEngineSharp Client\bin\Debug\Data\Graphics\Fonts\Georgia.ttf");
            this.CharacterTextures = new Dictionary<string, Texture>();

            PrepareMenuGui();
        }

        public void LoadGameTextures()
        {
            DirectoryInfo characterDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/Data/Graphics/Characters/");

            foreach (var file in characterDir.GetFiles("*.png", SearchOption.AllDirectories))
            {
                this.CharacterTextures.Add(file.Name.Replace(".png", ""), new Texture(file.FullName));
            }
        }

        private void PrepareGameGui()
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

        private void PrepareMenuGui()
        {
            #region News Label

            Label labelNews = _gui.Add(new Label(), "labelNews");
            LoadNews(labelNews);
            labelNews.Position = new Vector2f((_renderWindow.Size.X / 2) - (labelNews.Size.X / 2), 200);

            #endregion News Label

            #region Login Button

            Button loginButton = _gui.Add(new Button(themeConfigurationPath), "buttonLogin");
            loginButton.Position = new Vector2f(100, 500);
            loginButton.Text = "Login";
            loginButton.LeftMouseClickedCallback += buttonLogin_LeftMouseClickedCallback;

            #endregion Login Button

            #region Registration Button

            Button registrationButton = _gui.Add(new Button(themeConfigurationPath), "buttonRegistration");
            registrationButton.Position = new Vector2f(470, 500);
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

        private void textMyChat_ReturnKeyPressedCallback(object sender, CallbackArgs e)
        {
            var textMyChat = _gui.Get<EditBox>("textMyChat");
            var chatMessagePacket = new ChatMessagePacket();

            chatMessagePacket.WriteData(textMyChat.Text);
            Networking.SendPacket(chatMessagePacket);
            textMyChat.Text = "";
        }

        private void LoadNews(Label labelNews)
        {
            string[] news = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "/Data/News.txt");

            foreach (var line in news)
            {
                labelNews.Text += line + "\n";
            }
        }

        private void buttonSendRegistartion_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            Networking.Connect();

            var registrationPacket = new RegisterationPacket();
            registrationPacket.WriteData(_gui.Get<EditBox>("textUser").Text, _gui.Get<EditBox>("textPassword").Text);

            Networking.SendPacket(registrationPacket);
        }

        private void buttonRegistration_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            _gui.Get<Button>("buttonSendRegistration").Visible = true;
            _gui.Get<Label>("labelUsername").Visible = true;
            _gui.Get<Label>("labelPassword").Visible = true;
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
            _gui.Get<EditBox>("textUser").Visible = true;
            _gui.Get<EditBox>("textPassword").Visible = true;
            _gui.Get<Label>("labelNews").Visible = false;
            _gui.Get<Button>("buttonLogin").Visible = false;
            _gui.Get<Button>("buttonRegistration").Visible = false;
            _gui.Get<Button>("buttonBack").Visible = true;
        }

        private void buttonSendLogin_LeftMouseClickedCallback(object sender, CallbackArgs e)
        {
            Networking.Connect();

            var loginPacket = new LoginPacket();
            loginPacket.WriteData(_gui.Get<EditBox>("textUser").Text, _gui.Get<EditBox>("textPassword").Text);

            Networking.SendPacket(loginPacket);
        }

        public void Render()
        {
            while (_renderWindow.IsOpen())
            {
                _renderWindow.DispatchEvents();

                _renderWindow.Clear();

                switch (Graphics.RenderState)
                {
                    case RenderStates.Menu_Game_Transition:
                        PrepareGameGui();
                        Graphics.RenderState = RenderStates.Game;

                        Program.GameGraphics.ToString();
                        break;

                    case RenderStates.Menu:
                        RenderMenu();
                        break;

                    case RenderStates.Game:
                        RenderGame();
                        break;
                }

                _gui.Draw();

                _renderWindow.Display();
            }
        }

        private void RenderMenu()
        {
        }

        private void RenderGame()
        {
            RenderPlayers();
        }

        private void RenderPlayers()
        {
            foreach (var player in GameWorld.Players)
            {
                player.Value.Draw(_renderWindow);
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