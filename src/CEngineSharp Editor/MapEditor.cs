using CEngineSharp_Editor.Graphics;
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
    public partial class MapEditor : Form, IChildFormActions
    {
        private MapEditorProperties mapEditorProperties;
        private MapRenderer mapRenderer;

        private List<Map> maps;

        public class MapEditorProperties
        {
            private MapEditor _mapEditor;

            [Browsable(false)]
            public Map CurrentMap { get; set; }

            [CategoryAttribute("Tile Settings"), DefaultValueAttribute(Map.Layers.Ground)]
            public Map.Layers CurrentLayer { get; set; }

            [CategoryAttribute("Map Properties"), DefaultValueAttribute("Untitled")]
            public string Name
            {
                get { return this.CurrentMap.Name; }
                set
                {
                    this.CurrentMap.Name = value;
                    this._mapEditor.RefreshMapList();
                }
            }

            public MapEditorProperties(MapEditor mapEditor)
            {
                _mapEditor = mapEditor;
            }
        }

        public MapEditor()
        {
            InitializeComponent();

            mapEditorProperties = new MapEditorProperties(this);

            mapRenderer = new MapRenderer(this.mapDisplayPicture.Handle, this.tileSetPicture.Handle, mapEditorProperties);

            this.mapEditorProperties.CurrentLayer = Map.Layers.Ground;

            this.LoadMaps();

            this.tileSetScrollX.Maximum = (int)mapRenderer.CurrentTileSetWidth / 32;
            this.tileSetScrollY.Maximum = (int)mapRenderer.CurrentTileSetHeight / 32;

            this.mapPropertyGrid.SelectedObject = mapEditorProperties;
        }

        private void LoadMaps()
        {
            DirectoryInfo dI = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/Data/Maps/");

            this.maps = new List<Map>();

            this.mapList.Items.Clear();

            foreach (var file in dI.GetFiles("*.map", SearchOption.TopDirectoryOnly))
            {
                Map map = Map.LoadMap(file.FullName, this.mapRenderer.TileSetTextures);

                this.maps.Add(map);
                this.mapList.Items.Add(map.Name);
            }

            if (this.maps.Count != 0)
                this.mapEditorProperties.CurrentMap = this.maps[0];
            else
            {
                this.mapEditorProperties.CurrentMap = new Map();
                this.mapEditorProperties.CurrentMap.ResizeMap(25, 25);
                this.mapEditorProperties.CurrentMap.Name = "Untitled";
                this.maps.Add(this.mapEditorProperties.CurrentMap);
                this.mapList.Items.Add(this.mapEditorProperties.CurrentMap.Name);
                this.mapList.SelectedIndex = 0;
            }
        }

        public void RefreshMapList()
        {
            this.mapList.Items.Clear();
            foreach (var map in this.maps)
            {
                this.mapList.Items.Add(map.Name);
            }
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

        private void newMapButton_Click(object sender, EventArgs e)
        {
            Map map = new Map();
            map.Name = "Untitled " + maps.Count;
            map.ResizeMap(25, 25);

            this.maps.Add(map);
            this.mapList.Items.Add(map.Name);
        }

        private void removeMapButton_Click(object sender, EventArgs e)
        {
            int index = this.mapList.SelectedIndex;

            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/Data/Maps/" + this.mapEditorProperties.CurrentMap.Name + ".map");
            this.maps.RemoveAt(index);

            if (this.maps.Count == 0)
            {
                this.maps.Add(new Map());
                this.mapEditorProperties.CurrentMap = this.maps[0];
                this.mapEditorProperties.CurrentMap.Name = "Untitled";
                this.mapEditorProperties.CurrentMap.ResizeMap(25, 25);

                this.RefreshMapList();

                this.mapList.SelectedIndex = 0;
            }
            else
            {
                this.mapEditorProperties.CurrentMap = this.maps[index - 1];

                this.RefreshMapList();

                this.mapList.SelectedIndex = index - 1;
            }
        }

        private void MapEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mapRenderer.CanRender = false;
            this.SaveData();
        }

        public void SaveData()
        {
            foreach (var map in this.maps)
            {
                map.Save(this.mapRenderer.TileSetTextures);
            }
        }

        public void LoadData()
        {
            this.LoadMaps();
        }

        private void mapList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.mapEditorProperties.CurrentMap = this.maps[mapList.SelectedIndex];
            this.mapPropertyGrid.Refresh();
        }

        private void fillMapButton_Click(object sender, EventArgs e)
        {
            this.mapRenderer.FillMap();
        }

        private void clearMapButton_Click(object sender, EventArgs e)
        {
            this.mapRenderer.ClearMap();
        }

        private void MapEditor_Load(object sender, EventArgs e)
        {
            this.mapList.SelectedIndex = 0;
        }
    }
}