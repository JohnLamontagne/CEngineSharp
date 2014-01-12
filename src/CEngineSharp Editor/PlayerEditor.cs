using CEngineSharp_Editor.World;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CEngineSharp_Editor
{
    public partial class PlayerEditor : Form, IChildFormActions
    {
        private Sprite playerSprite;
        private RenderWindow playerSpriteDisplay;
        private bool exiting;
        private string dataPath;
        private List<Player> players;
        private Player selectedPlayer;

        public PlayerEditor(string dataPath)
        {
            InitializeComponent();

            this.dataPath = dataPath;

            this.playerTextureScrollBar.Minimum = 0;

            this.playerTextureScrollBar.LargeChange = 1;

            this.playerTextureScrollBar.Maximum = (new DirectoryInfo(Constants.FILEPATH_GRAPHICS + "/Characters/")).GetFiles("*.png").Length - 1;

            this.playerSpriteDisplay = new RenderWindow(this.playerSpritePic.Handle);
            this.playerSpriteDisplay.SetActive(false);

            this.LoadPlayers();

            this.exiting = false;

            new Thread(this.NpcAnimationLogic).Start();
        }

        private void NpcAnimationLogic()
        {
            this.playerSpriteDisplay.SetFramerateLimit(8);
            this.playerSpriteDisplay.SetActive(true);

            while (!this.exiting)
            {
                this.playerSpriteDisplay.Clear();

                var oldRect = this.playerSprite.TextureRect;
                var newRect = new IntRect();
                newRect.Width = 32;
                newRect.Height = 32;

                if (oldRect.Left >= 64)
                {
                    newRect.Left = 0;

                    if (oldRect.Top >= 96)
                    {
                        newRect.Top = 0;
                    }
                    else
                    {
                        newRect.Top = oldRect.Top + 32;
                    }
                }
                else
                {
                    newRect.Left = oldRect.Left + 32;
                    newRect.Top = oldRect.Top;
                }

                this.playerSprite.TextureRect = newRect;

                this.playerSpriteDisplay.Draw(this.playerSprite);

                this.playerSpriteDisplay.Display();
            }
        }

        private void PopulateData()
        {
            if (this.selectedPlayer == null) return;


            this.textName.Text = this.selectedPlayer.Name;
            this.textPassword.Text = this.selectedPlayer.Password;
            this.playerSprite = new Sprite(new Texture(Constants.FILEPATH_GRAPHICS + "/Characters/" + this.selectedPlayer.TextureNumber + ".png"));
            this.textHP.Text = this.selectedPlayer.HP.ToString();
            this.textMP.Text = this.selectedPlayer.MP.ToString();
        }

        private void LoadPlayers()
        {
            this.playerListBox.Items.Clear();

            this.players = new List<Player>();

            DirectoryInfo dI = new DirectoryInfo(dataPath + "/Accounts/");

            foreach (var file in dI.GetFiles("*.dat", SearchOption.TopDirectoryOnly))
            {
                this.players.Add(Player.Load(file.FullName));
                this.playerListBox.Items.Add(this.players[this.players.Count - 1].Name);
            }

            if (this.playerListBox.Items.Count > 0)
            {
                this.playerListBox.SelectedIndex = 0;
                this.selectedPlayer = this.players[this.playerListBox.SelectedIndex];
            }

            this.PopulateData();
        }

        private void SavePlayers()
        {
            DirectoryInfo dI = new DirectoryInfo(this.dataPath + "/Accounts/");

            // Delete all of the old files (they will be replaced). We must do this to insure that we don't keep any renamed items.
            foreach (var file in dI.GetFiles("*.dat", SearchOption.TopDirectoryOnly))
                file.Delete();

            foreach (var player in this.players)
            {
                player.Save(this.dataPath);
            }
        }

        public void SaveData()
        {
            this.SavePlayers();
        }

        public void LoadData()
        {
            this.LoadPlayers();
        }
    }
}