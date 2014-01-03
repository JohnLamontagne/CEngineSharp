using CEngineSharp_Editor.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEngineSharp_Editor
{
    public partial class ItemEditor : Form, IChildFormActions
    {
        private string _dataPath;

        private List<Item> items;

        private Item selectedItem;

        public ItemEditor(string dataPath)
        {
            InitializeComponent();

            _dataPath = dataPath;

            this.itemTextureScrollBar.Minimum = 1;

            this.itemTextureScrollBar.LargeChange = 1;

            this.itemTextureScrollBar.Maximum = (new DirectoryInfo(Constants.FILEPATH_GRAPHICS + "/Items/")).GetFiles("*.png").Length;

            this.LoadItems();
        }

        public void SaveData()
        {
            this.SaveItems();
        }

        public void LoadData()
        {
            this.LoadItems();
        }

        private void LoadItems()
        {
            this.itemListBox.Items.Clear();

            this.items = new List<Item>();

            DirectoryInfo dI = new DirectoryInfo(_dataPath + "/Items/");

            foreach (var file in dI.GetFiles("*.dat", SearchOption.TopDirectoryOnly))
            {
                this.items.Add(Item.Load(file.FullName));

                this.itemListBox.Items.Add(this.items[this.items.Count - 1].Name);
            }

            if (this.items.Count == 0)
            {
                Item item = new Item();
                item.Name = "Untitled";
                item.TextureNumber = 0;
                this.items.Add(item);
                this.itemListBox.Items.Add(item.Name);
            }

            this.itemListBox.SelectedIndex = 0;

            this.selectedItem = this.items[this.itemListBox.SelectedIndex];

            this.PopulateData();
        }

        private void SaveItems()
        {
            DirectoryInfo dI = new DirectoryInfo(_dataPath + "/Items/");

            // Delete all of the old files (they will be replaced). We must do this to insure that we don't keep any renamed items.
            foreach (var file in dI.GetFiles("*.dat", SearchOption.TopDirectoryOnly))
                file.Delete();

            foreach (var item in this.items)
            {
                item.Save(_dataPath);
            }
        }

        private void PopulateData()
        {
            this.textName.Text = selectedItem.Name;

            this.itemSpritePic.Load(Constants.FILEPATH_GRAPHICS + "/Items/" + selectedItem.TextureNumber + ".png");
        }

        private void addItemButton_Click(object sender, EventArgs e)
        {
            Item item = new Item();
            item.Name = "Untitled";

            this.items.Add(item);
            this.itemListBox.Items.Add(item.Name);
        }

        private void itemListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.itemListBox.SelectedIndex < 0 || this.itemListBox.SelectedIndex > this.items.Count)
                return;

            this.selectedItem = this.items[itemListBox.SelectedIndex];

            this.PopulateData();
        }

        private void textName_TextChanged(object sender, EventArgs e)
        {
            this.selectedItem.Name = this.textName.Text;

            int index = this.itemListBox.SelectedIndex;

            this.itemListBox.Items[index] = this.textName.Text;

            this.itemListBox.SelectedIndex = index;
        }

        private void itemTextureScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.selectedItem.TextureNumber = e.NewValue;

            this.itemSpritePic.Load(Constants.FILEPATH_GRAPHICS + "/Items/" + selectedItem.TextureNumber + ".png");
        }
    }
}