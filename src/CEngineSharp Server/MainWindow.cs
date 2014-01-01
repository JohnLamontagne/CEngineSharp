using CEngineSharp_Server;
using CEngineSharp_Server.GameLogic;
using CEngineSharp_Server.Net;
using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World;
using CEngineSharp_Server.World.Content_Managers;
using System;
using System.IO;
using System.Threading;
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

        private delegate void RemovePlayerFromTableDelegate(int playerIndex);

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

            this.dataGridContextMenu = new ContextMenuStrip();
            this.dataGridContextMenu.Items.Add("Kick", null, new EventHandler(this.HandleMenuKick));
            this.playersDataGrid.ContextMenuStrip = this.dataGridContextMenu;
            this.playersDataGrid.AllowUserToAddRows = false;

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

        private void HandleMenuKick(object sender, EventArgs e)
        {
            for (int i = 0; i < this.playersDataGrid.RowCount; i++)
            {
                if (this.playersDataGrid[0, i].Selected)
                {
                    Networking.KickPlayer((int)this.playersDataGrid[0, i].Value);
                }
            }
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
                this.playersDataGrid.Rows.Add(player.PlayerIndex, player.Name, player.Level, player.IP, player.AccessLevel, player.GetVital(Vitals.HitPoints), player.GetVital(Vitals.ManaPoints));
            }
        }

        public void RemovePlayerFromGrid(int playerIndex)
        {
            if (this.InvokeRequired)
                this.Invoke(new RemovePlayerFromTableDelegate(this.RemovePlayerFromGrid), playerIndex);
            else
            {
                foreach (DataGridViewRow row in this.playersDataGrid.Rows)
                {
                    if ((int)row.Cells[0].Value == playerIndex)
                    {
                        this.playersDataGrid.Rows.Remove(row);
                        return;
                    }
                }
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