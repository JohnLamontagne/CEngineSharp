using CEngineSharp_Server.GameLogic;
using CEngineSharp_Server.Net;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEngineSharp_Server
{
    public partial class MainWindow : Form
    {
        private TextWriter _textWriter;

        private ServerLoop _serverLoop;

        private static Thread _serverLoopThread;

        private delegate void SetTitleDelegate(string value);

        private delegate void AddPlayerToTableDelegate(Player player);

        public MainWindow()
        {
            InitializeComponent();

            this.textServerOutput.GotFocus += textServerOutput_GotFocus;
        }

        private void textServerOutput_GotFocus(object sender, EventArgs e)
        {
            this.textCommandInput.Select();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            _textWriter = new TextBoxStreamWriter(this.textServerOutput);

            Console.SetOut(_textWriter);

            ServerConfiguration.LoadConfig();

            this.Text = ServerConfiguration.GameName + " - " + ServerConfiguration.ServerIP + ":" + ServerConfiguration.ServerPort + " - Player Count: " + PlayerManager.PlayerCount +
                " - Debug Mode: " + (ServerConfiguration.SupressionLevel == ErrorHandler.ErrorLevels.Low ? "On" : "Off");

            // Load the game world.
            Server.LoadWorld();

            // Prepare the server to start accepting connections.
            Networking.Start();

            _serverLoop = new ServerLoop();
            _serverLoopThread = new Thread(_serverLoop.Start);
            _serverLoopThread.Start();

            this.textCommandInput.Select();
        }

        public void SetTitle(string text)
        {
            if (this.InvokeRequired)
                this.Invoke(new SetTitleDelegate(this.SetTitle), text);
            else
                this.Text = text;
        }

        public void AddPlayerToGrid(Player player)
        {
            if (this.InvokeRequired)
                this.Invoke(new AddPlayerToTableDelegate(this.AddPlayerToGrid), player);
            else
            {
                this.dataGridPlayers.Rows.Add(player.Name, player.Level, player.IP, player.AccessLevel, player.GetVital(Vitals.HitPoints), player.GetVital(Vitals.ManaPoints));
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Globals.ShuttingDown = true;
        }

        private void textCommandInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Server.HandleCommand(textCommandInput.Text);

                e.Handled = true;
                e.SuppressKeyPress = true;

                textCommandInput.Text = "";
            }
        }
    }
}