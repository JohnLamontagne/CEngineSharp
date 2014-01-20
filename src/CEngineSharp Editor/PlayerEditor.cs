using CEngineSharp_World.Entities;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
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
        private List<BasePlayer> players;
        private BasePlayer selectedPlayer;

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
                var newRect = new IntRect { Width = 32, Height = 32 };

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
            this.textHP.Text = this.selectedPlayer.GetStat(Stats.Health).ToString();
            this.textMP.Text = this.selectedPlayer.GetStat(Stats.Mana).ToString();
        }

        private void LoadPlayers()
        {
            this.playerListBox.Items.Clear();

            this.players = new List<BasePlayer>();

            var dI = new DirectoryInfo(dataPath + "/Players/");

            foreach (var file in dI.GetFiles("*.dat", SearchOption.TopDirectoryOnly))
            {
                this.players.Add(BasePlayer.LoadPlayer(file.FullName));
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
            var dI = new DirectoryInfo(this.dataPath + "/Accounts/");

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

        private void textHP_TextChanged(object sender, EventArgs e)
        {
            this.selectedPlayer.SetStat(Stats.Health, int.Parse(textHP.Text));
        }

        private void textMP_TextChanged(object sender, EventArgs e)
        {
            this.selectedPlayer.SetStat(Stats.Mana, int.Parse(textMP.Text));
        }

        private void playerTextureScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.selectedPlayer.TextureNumber = e.NewValue;
        }
    }
}