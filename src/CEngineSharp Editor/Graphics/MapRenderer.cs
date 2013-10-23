using CEngineSharp_Editor.World;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CEngineSharp_Editor.Graphics
{
    public class MapRenderer
    {
        private RenderWindow mapRenderWindow;

        private RenderWindow tileSetRenderWindow;

        public bool CanRender { get; set; }

        private bool mouseDown;

        private byte mouseButton;

        private Sprite selectedTileSprite;

        private Sprite currentTileSetSprite;

        private List<Sprite> tileSetSprites;

        private RectangleShape tileSelector;

        private View tileSetView;

        public uint CurrentTileSetWidth { get { return currentTileSetSprite.Texture.Size.X; } }

        public uint CurrentTileSetHeight { get { return currentTileSetSprite.Texture.Size.Y; } }

        private MapEditor.MapEditorProperties mapEditorProperties;

        public MapRenderer(IntPtr mapRenderWindowHandle, IntPtr tileSetRenderHandle, MapEditor.MapEditorProperties mapEditorProperties)
        {
            this.mapRenderWindow = new RenderWindow(mapRenderWindowHandle);
            this.tileSetRenderWindow = new RenderWindow(tileSetRenderHandle);

            this.mapRenderWindow.MouseButtonPressed += mapRenderWindow_MouseButtonPressed;
            this.mapRenderWindow.MouseButtonReleased += mapRenderWindow_MouseButtonReleased;
            this.mapRenderWindow.MouseMoved += mapRenderWindow_MouseMoved;

            this.tileSetRenderWindow.MouseButtonPressed += tileSetRenderWindow_MouseButtonPressed;

            this.LoadTileSets();

            this.mapEditorProperties = mapEditorProperties;

            this.CanRender = true;

            this.mapRenderWindow.SetActive(false);
            this.tileSetRenderWindow.SetActive(false);
            this.tileSetView = this.tileSetRenderWindow.DefaultView;

            this.mapEditorProperties.CurrentLayer = World.Map.Layers.Ground;

            new Thread(RenderLoop).Start();
        }

        public void ScrollTileSet(int x, int y)
        {
            this.tileSetRenderWindow.SetView(new View(new FloatRect(x, y, this.tileSetRenderWindow.Size.X, this.tileSetRenderWindow.Size.Y)));
        }

        public void ShowNextTileset()
        {
            int nextIndex = this.tileSetSprites.IndexOf(this.currentTileSetSprite) + 1;

            if (nextIndex >= this.tileSetSprites.Count)
                return;

            this.currentTileSetSprite = this.tileSetSprites[nextIndex];
        }

        public void ShowPreviousTileset()
        {
            int nextIndex = this.tileSetSprites.IndexOf(this.currentTileSetSprite) - 1;

            if (nextIndex < 0)
                return;

            this.currentTileSetSprite = this.tileSetSprites[nextIndex];
        }

        private void LoadTileSets()
        {
            DirectoryInfo dI = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/Data/Graphics/Tile Sets/");

            this.tileSetSprites = new List<Sprite>();

            foreach (var file in dI.GetFiles("*.png", SearchOption.TopDirectoryOnly))
            {
                tileSetSprites.Add(new Sprite(new Texture(file.FullName)));
            }

            this.currentTileSetSprite = tileSetSprites[0];

            this.tileSelector = new RectangleShape(new Vector2f(32, 32));
            this.tileSelector.OutlineColor = Color.Red;
            this.tileSelector.FillColor = Color.Transparent;
            this.tileSelector.OutlineThickness = 2f;
            this.selectedTileSprite = new Sprite(this.currentTileSetSprite.Texture, new IntRect((int)tileSelector.Position.X, (int)tileSelector.Position.Y, 32, 32));
        }

        private void tileSetRenderWindow_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            this.tileSelector.Position = tileSetRenderWindow.MapPixelToCoords(new Vector2i((e.X / 32) * 32, (e.Y / 32) * 32));

            this.selectedTileSprite = new Sprite(this.currentTileSetSprite.Texture, new IntRect((int)tileSelector.Position.X, (int)tileSelector.Position.Y, 32, 32));
        }

        private void mapRenderWindow_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (!mouseDown) return;

            int x = e.X / 32;
            int y = e.Y / 32;

            if (this.mouseButton == (byte)Mouse.Button.Left)
            {
                if (this.mapEditorProperties.CurrentMap.Tiles[x, y] == null)
                    this.mapEditorProperties.CurrentMap.Tiles[x, y] = new Map.Tile();

                this.mapEditorProperties.CurrentMap.Tiles[x, y].Layers[(int)this.mapEditorProperties.CurrentLayer] = new Map.Tile.Layer(new Sprite(this.selectedTileSprite.Texture, this.selectedTileSprite.TextureRect), x, y);
            }
            else
            {
                if (this.mapEditorProperties.CurrentMap.Tiles[x, y] == null || this.mapEditorProperties.CurrentMap.Tiles[x, y].Layers[(int)this.mapEditorProperties.CurrentLayer] == null)
                    return;

                this.mapEditorProperties.CurrentMap.Tiles[x, y].Layers[(int)this.mapEditorProperties.CurrentLayer].Sprite = null;
            }
        }

        private void mapRenderWindow_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            this.mouseDown = false;
        }

        private void mapRenderWindow_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            int x = e.X / 32;
            int y = e.Y / 32;

            this.mouseDown = true;

            this.mouseButton = (byte)e.Button;

            if (e.Button == Mouse.Button.Left)
            {
                if (this.mapEditorProperties.CurrentMap.Tiles[x, y] == null)
                    this.mapEditorProperties.CurrentMap.Tiles[x, y] = new Map.Tile();

                this.mapEditorProperties.CurrentMap.Tiles[x, y].Layers[(int)this.mapEditorProperties.CurrentLayer] = new Map.Tile.Layer(new Sprite(this.selectedTileSprite.Texture, this.selectedTileSprite.TextureRect), x, y);
            }
            else
            {
                if (this.mapEditorProperties.CurrentMap.Tiles[x, y] == null || this.mapEditorProperties.CurrentMap.Tiles[x, y].Layers[(int)this.mapEditorProperties.CurrentLayer] == null)
                    return;

                this.mapEditorProperties.CurrentMap.Tiles[x, y].Layers[(int)this.mapEditorProperties.CurrentLayer].Sprite = null;
            }
        }

        private void RenderLoop()
        {
            this.mapRenderWindow.SetActive(true);
            this.tileSetRenderWindow.SetActive(true);

            while (this.CanRender)
            {
                this.mapRenderWindow.DispatchEvents();
                this.tileSetRenderWindow.DispatchEvents();

                this.mapRenderWindow.Clear();
                this.tileSetRenderWindow.Clear();

                this.mapEditorProperties.CurrentMap.Draw(mapRenderWindow);

                this.tileSetRenderWindow.Draw(this.currentTileSetSprite);
                this.tileSetRenderWindow.Draw(this.tileSelector);

                this.mapRenderWindow.Display();
                this.tileSetRenderWindow.Display();
            }
        }
    }
}