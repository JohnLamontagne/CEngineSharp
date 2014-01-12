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
        public class MapEditorProperties
        {
            [Browsable(false)]
            private MapEditor _mapEditor;

            [Browsable(false)]
            public Map CurrentMap { get; set; }

            [Browsable(false)]
            public int MapEditorLeft { get; set; }

            [Browsable(false)]
            public int MapEditorTop { get; set; }

            [Browsable(false)]
            public SFML.Graphics.View MapView { get; set; }

            [Browsable(false)]
            public int MapEditorWidth { get { return _mapEditor.mapDisplayPicture.Width / 32; } }

            [Browsable(false)]
            public int MapEditorHeight { get { return _mapEditor.mapDisplayPicture.Height / 32; } }

            [CategoryAttribute("Tile Settings")]
            public bool TileBlockedAttribute { get; set; }

            [CategoryAttribute("Map Properties")]
            public int MapWidth
            {
                get { return this.CurrentMap.MapWidth; }
                set
                {
                    this.CurrentMap.ResizeMap(value, this.MapHeight);
                    _mapEditor.mapScrollX.Maximum = this.MapWidth - this.MapEditorWidth;
                }
            }

            [CategoryAttribute("Map Properties")]
            public int MapHeight
            {
                get { return this.CurrentMap.MapHeight; }
                set
                {
                    this.CurrentMap.ResizeMap(this.MapWidth, value);
                    _mapEditor.mapScrollY.Maximum = this.MapHeight - this.MapEditorHeight;
                }
            }

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

        private MapEditorProperties mapEditorProperties;
        private MapRenderer mapRenderer;

        private List<Map> maps;

        private string dataPath;

        public MapEditor(string dataPath)
        {
            mapEditorProperties = new MapEditorProperties(this);

            InitializeComponent();

            mapRenderer = new MapRenderer(this.mapDisplayPicture.Handle, this.tileSetPicture.Handle, mapEditorProperties);

            this.mapEditorProperties.CurrentLayer = Map.Layers.Ground;

            this.dataPath = dataPath;

            this.LoadMaps();

            this.tileSetScrollX.Maximum = (int)mapRenderer.CurrentTileSetWidth / 32;
            this.tileSetScrollY.Maximum = (int)mapRenderer.CurrentTileSetHeight / 32;

            this.mapScrollY.SmallChange = 1;
            this.mapScrollY.LargeChange = 1;

            this.mapScrollX.SmallChange = 1;
            this.mapScrollX.LargeChange = 1;

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
            this.mapRenderer.Running = false;
            this.SaveData();
            this.mapRenderer = null;
        }

        private void mapList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.mapEditorProperties.CurrentMap = this.maps[mapList.SelectedIndex];
            this.mapPropertyGrid.Refresh();

            this.Text = this.mapEditorProperties.CurrentMap.Name + " Version: " + this.mapEditorProperties.CurrentMap.Version;
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
            this.mapScrollX.Maximum = this.mapEditorProperties.MapWidth - this.mapEditorProperties.MapEditorWidth;
            this.mapScrollY.Maximum = this.mapEditorProperties.MapHeight - this.mapEditorProperties.MapEditorHeight;
        }

        private void mapScrollY_Scroll(object sender, ScrollEventArgs e)
        {
            this.mapEditorProperties.MapView.Move(new SFML.Window.Vector2f(0, (e.NewValue - e.OldValue) * 32));
            this.mapEditorProperties.MapEditorTop = e.NewValue;
        }

        private void mapScrollX_Scroll(object sender, ScrollEventArgs e)
        {
            this.mapEditorProperties.MapView.Move(new SFML.Window.Vector2f((e.NewValue - e.OldValue) * 32, 0));
            this.mapEditorProperties.MapEditorLeft = e.NewValue;
        }

        public void SaveData()
        {
            foreach (var map in this.maps)
            {
                map.Save(this.dataPath + "/Maps/", this.mapRenderer.TileSetTextures);
            }
        }

        public void LoadData()
        {
            this.LoadMaps();
        }

        private void LoadMaps()
        {
            DirectoryInfo dI = new DirectoryInfo(dataPath + "/Maps/");

            this.maps = new List<Map>();

            this.mapList.Items.Clear();

            foreach (var file in dI.GetFiles("*.map", SearchOption.TopDirectoryOnly))
            {
                Map map = Map.LoadMap(file.FullName, mapRenderer.TileSetTextures);

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

    }
}