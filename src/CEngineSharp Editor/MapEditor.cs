using CEngineSharp_Editor.Graphics;
using CEngineSharp_Editor.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEngineSharp_Editor
{
    public partial class MapEditor : Form
    {
        private MapRenderer mapRenderer;
        private MapEditorProperties mapEditorProperties;

        public MapEditor()
        {
            InitializeComponent();

            mapEditorProperties = new MapEditorProperties();

            mapEditorProperties.CurrentMap = new Map(this.Width, this.Height);

            mapRenderer = new MapRenderer(this.mapDisplayPicture.Handle, this.tileSetPicture.Handle, mapEditorProperties);

            this.tileSetScrollX.Maximum = (int)mapRenderer.CurrentTileSetWidth / 32;
            this.tileSetScrollY.Maximum = (int)mapRenderer.CurrentTileSetHeight / 32;

            this.mapPropertyGrid.SelectedObject = mapEditorProperties;
        }

        private void tileSetScrollY_Scroll(object sender, ScrollEventArgs e)
        {
            this.mapRenderer.ScrollTileSet(this.tileSetScrollX.Value * 32, e.NewValue * 32);
        }

        private void tileSetScrollX_Scroll(object sender, ScrollEventArgs e)
        {
            this.mapRenderer.ScrollTileSet(e.NewValue * 32, this.tileSetScrollY.Value * 32);
        }

        private void tileSetBackButton_Click(object sender, EventArgs e)
        {
            this.mapRenderer.ShowPreviousTileset();
        }

        private void tileSetForwardButton_Click(object sender, EventArgs e)
        {
            this.mapRenderer.ShowNextTileset();
        }

        public class MapEditorProperties
        {
            [Browsable(false)]
            public Map CurrentMap { get; set; }

            [CategoryAttribute("Tile Settings"), DefaultValueAttribute(Map.Layers.Ground)]
            public Map.Layers CurrentLayer { get; set; }
        }
    }
}