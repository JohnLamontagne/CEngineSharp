using CEngineSharp_Editor.World;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEngineSharp_Editor
{
    public partial class NpcEditor : Form, IChildFormActions
    {
        private List<Npc> npcs;

        private Npc selectedNpc;

        private string dataPath;

        private bool exiting;

        private RenderWindow npcSpriteDisplay;

        private Sprite npcSprite;

        public NpcEditor(string dataPath)
        {
            InitializeComponent();
            this.dataPath = dataPath;

            this.npcTextureScrollBar.Minimum = 0;

            this.npcTextureScrollBar.LargeChange = 1;

            this.npcTextureScrollBar.Maximum = (new DirectoryInfo(Constants.FILEPATH_GRAPHICS + "/Characters/")).GetFiles("*.png").Length - 1;

            this.npcSpriteDisplay = new RenderWindow(this.npcSpritePic.Handle);
            this.npcSpriteDisplay.SetActive(false);

            this.LoadNpcs();

            this.exiting = false;

            new Thread(this.NpcAnimationLogic).Start();
        }

        private void LoadNpcs()
        {
            this.npcListBox.Items.Clear();

            this.npcs = new List<Npc>();

            DirectoryInfo dI = new DirectoryInfo(dataPath + "/Npcs/");

            foreach (var file in dI.GetFiles("*.dat", SearchOption.TopDirectoryOnly))
            {
                this.npcs.Add(Npc.Load(file.FullName));
                this.npcListBox.Items.Add(this.npcs[this.npcs.Count - 1].Name);
            }

            if (this.npcs.Count == 0)
            {
                Npc npc = new Npc();
                npc.Name = "Untitled";
                npc.TextureNumber = 0;
                this.npcs.Add(npc);
                this.npcListBox.Items.Add(npc.Name);
            }

            this.npcListBox.SelectedIndex = 0;

            this.selectedNpc = this.npcs[this.npcListBox.SelectedIndex];

            this.PopulateData();
        }

        private void SaveNpcs()
        {
            DirectoryInfo dI = new DirectoryInfo(this.dataPath + "/Npcs/");

            // Delete all of the old files (they will be replaced). We must do this to insure that we don't keep any renamed items.
            foreach (var file in dI.GetFiles("*.dat", SearchOption.TopDirectoryOnly))
                file.Delete();

            foreach (var npc in this.npcs)
            {
                npc.Save(this.dataPath);
            }
        }


        private void PopulateData()
        {
            this.textName.Text = this.selectedNpc.Name;
            this.npcSprite = new Sprite(new Texture(Constants.FILEPATH_GRAPHICS + "/Characters/" + this.selectedNpc.TextureNumber + ".png"));
            this.textHP.Text = this.selectedNpc.HP.ToString();
            this.textMP.Text = this.selectedNpc.MP.ToString();
            this.textStrength.Text = this.selectedNpc.Strength.ToString();
        }

        public void SaveData()
        {
            this.SaveNpcs();
        }

        public void LoadData()
        {
            this.LoadNpcs();
        }

        private void NpcAnimationLogic()
        {
            this.npcSpriteDisplay.SetFramerateLimit(8);
            this.npcSpriteDisplay.SetActive(true);

            while (!this.exiting)
            {
                this.npcSpriteDisplay.Clear();

                var oldRect = this.npcSprite.TextureRect;
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

                this.npcSprite.TextureRect = newRect;

                this.npcSpriteDisplay.Draw(this.npcSprite);

                this.npcSpriteDisplay.Display();
            }
        }

        private void npcTextureScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.selectedNpc.TextureNumber = e.NewValue;
            this.npcSprite = new Sprite(new Texture(Constants.FILEPATH_GRAPHICS + "/Characters/" + e.NewValue + ".png"));
        }

        private void textName_TextChanged(object sender, EventArgs e)
        {
            this.selectedNpc.Name = this.textName.Text;

            int index = this.npcListBox.SelectedIndex;

            this.npcListBox.Items[index] = this.textName.Text;

            this.npcListBox.SelectedIndex = index;
        }

        private void addNpcButton_Click(object sender, EventArgs e)
        {
            Npc item = new Npc();
            item.Name = "Untitled";

            this.npcs.Add(item);
            this.npcListBox.Items.Add(item.Name);
        }

        private void npcListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.npcListBox.SelectedIndex < 0 || this.npcListBox.SelectedIndex > this.npcs.Count)
                return;

            this.selectedNpc = this.npcs[this.npcListBox.SelectedIndex];

            this.PopulateData();
        }

        private void textHP_TextChanged(object sender, EventArgs e)
        {
            this.selectedNpc.HP = int.Parse(textHP.Text);
        }

        private void textMP_TextChanged(object sender, EventArgs e)
        {
            this.selectedNpc.MP = int.Parse(textMP.Text);
        }

        private void textStrength_TextChanged(object sender, EventArgs e)
        {
            this.selectedNpc.Strength = int.Parse(textStrength.Text);
        }
    }
}
