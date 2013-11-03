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

        private bool clearMap;

        private MapEditor.MapEditorProperties mapEditorProperties;

        public List<Texture> TileSetTextures { get { return this.tileSetTextures; } }

        private Sprite selectedTileSprite;

        private Sprite currentTileSetSprite;

        private List<Texture> tileSetTextures;

        private RectangleShape tileSetSelector;

        private View tileSetView;

        public uint CurrentTileSetWidth { get { return currentTileSetSprite.Texture.Size.X; } }

        public uint CurrentTileSetHeight { get { return currentTileSetSprite.Texture.Size.Y; } }

        public bool CanRender { get; set; }

        public MapRenderer(IntPtr mapRenderWindowHandle, IntPtr tileSetRenderHandle, MapEditor.MapEditorProperties mapEditorProperties)
        {
            this.mapRenderWindow = new RenderWindow(mapRenderWindowHandle);
            this.tileSetRenderWindow = new RenderWindow(tileSetRenderHandle);

            this.mapRenderWindow.MouseButtonPressed += mapRenderWindow_MouseButtonPressed;
            this.mapRenderWindow.MouseMoved += mapRenderWindow_MouseMoved;

            this.tileSetRenderWindow.MouseButtonPressed += tileSetRenderWindow_MouseButtonPressed;
            this.tileSetRenderWindow.MouseMoved += tileSetRenderWindow_MouseMoved;
            this.tileSetRenderWindow.MouseButtonReleased += tileSetRenderWindow_MouseButtonReleased;

            this.LoadTileSets();

            this.mapEditorProperties = mapEditorProperties;

            this.CanRender = true;

            this.mapRenderWindow.SetActive(false);
            this.tileSetRenderWindow.SetActive(false);
            this.tileSetView = this.tileSetRenderWindow.DefaultView;

            this.mapEditorProperties.CurrentLayer = World.Map.Layers.Ground;

            new Thread(RenderLoop).Start();
        }

        private void tileSetRenderWindow_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                Vector2f realPosition = this.tileSetRenderWindow.MapPixelToCoords(new Vector2i(e.X, e.Y));

                int width = (int)(realPosition.X - this.tileSetSelector.Position.X);
                int height = (int)(realPosition.Y - this.tileSetSelector.Position.Y);

                if (width < 0)
                    width -= 32;
                else
                    width += 32;

                if (height < 0)
                    height -= 32;
                else
                    height += 32;

                width = (width / 32) * 32;
                height = (height / 32) * 32;

                this.tileSetSelector.Size = new Vector2f(width, height);

                this.selectedTileSprite = this.selectedTileSprite = new Sprite(this.currentTileSetSprite.Texture, new IntRect((int)tileSetSelector.Position.X, (int)tileSetSelector.Position.Y, width, height));
            }
        }

        private void tileSetRenderWindow_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                Vector2f realPosition = this.tileSetRenderWindow.MapPixelToCoords(new Vector2i(e.X, e.Y));

                int width = (int)(realPosition.X - this.tileSetSelector.Position.X);
                int height = (int)(realPosition.Y - this.tileSetSelector.Position.Y);

                this.tileSetSelector.Size = new Vector2f(width, height);
            }
        }

        private void tileSetRenderWindow_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                this.tileSetSelector.Position = tileSetRenderWindow.MapPixelToCoords((new Vector2i((e.X / 32) * 32, (e.Y / 32) * 32)));
                this.tileSetSelector.Size = new Vector2f(32, 32);

                this.selectedTileSprite = new Sprite(this.currentTileSetSprite.Texture, new IntRect((int)tileSetSelector.Position.X, (int)tileSetSelector.Position.Y, 32, 32));
            }
        }

        private void LoadTileSets()
        {
            DirectoryInfo dI = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/Data/Graphics/Tile Sets/");

            this.tileSetTextures = new List<Texture>();

            foreach (var file in dI.GetFiles("*.png", SearchOption.TopDirectoryOnly))
            {
                tileSetTextures.Add(new Texture(file.FullName));
            }

            this.currentTileSetSprite = new Sprite(tileSetTextures[0]);

            this.tileSetSelector = new RectangleShape(new Vector2f(32, 32));
            this.tileSetSelector.OutlineColor = Color.Red;
            this.tileSetSelector.FillColor = Color.Transparent;
            this.tileSetSelector.OutlineThickness = 2f;

            this.selectedTileSprite = new Sprite(this.currentTileSetSprite.Texture, new IntRect((int)tileSetSelector.Position.X, (int)tileSetSelector.Position.Y, 32, 32));
        }

        private void PlaceTile(Vector2i position)
        {
            for (int x = 0; x < this.selectedTileSprite.TextureRect.Width / 32; x++)
            {
                for (int y = 0; y < this.selectedTileSprite.TextureRect.Height / 32; y++)
                {
                    IntRect tileSpriteRect = new IntRect(this.selectedTileSprite.TextureRect.Left + (x * 32), this.selectedTileSprite.TextureRect.Top + (y * 32), 32, 32);
                    Sprite tileSprite = new Sprite(this.selectedTileSprite.Texture, tileSpriteRect);

                    if (x + position.X >= this.mapEditorProperties.CurrentMap.Tiles.GetLength(0) || y + position.Y >= this.mapEditorProperties.CurrentMap.Tiles.GetLength(1))
                        return;

                    if (this.mapEditorProperties.CurrentMap.Tiles[position.X + x, position.Y + y] == null)
                        this.mapEditorProperties.CurrentMap.Tiles[position.X + x, position.Y + y] = new Map.Tile();

                    this.mapEditorProperties.CurrentMap.Tiles[position.X + x, position.Y + y].Layers[(int)this.mapEditorProperties.CurrentLayer] = new Map.Tile.Layer(tileSprite, position.X + x, position.Y + y);
                }
            }
        }

        private void RemoveTile(Vector2i position)
        {
            if (this.mapEditorProperties.CurrentMap.Tiles[position.X, position.Y] == null ||
                  this.mapEditorProperties.CurrentMap.Tiles[position.X, position.Y].Layers[(int)this.mapEditorProperties.CurrentLayer] == null)
            {
                return;
            }

            this.mapEditorProperties.CurrentMap.Tiles[position.X, position.Y].Layers[(int)this.mapEditorProperties.CurrentLayer] = null;
        }

        private void mapRenderWindow_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            int mouseX = e.X / 32;
            int mouseY = e.Y / 32;

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                this.PlaceTile(new Vector2i(mouseX, mouseY));
            }
            else if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                this.RemoveTile(new Vector2i(mouseX, mouseY));
            }
        }

        private void mapRenderWindow_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            int mouseX = e.X / 32;
            int mouseY = e.Y / 32;

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                this.PlaceTile(new Vector2i(mouseX, mouseY));
            }
            else if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                RemoveTile(new Vector2i(mouseX, mouseY));
            }
        }

        private void RenderLoop()
        {
            this.mapRenderWindow.SetActive(true);
            this.tileSetRenderWindow.SetActive(true);

            while (this.CanRender)
            {
                if (this.clearMap)
                {
                    for (int x = 0; x < this.mapEditorProperties.CurrentMap.Tiles.GetLength(0); x++)
                    {
                        for (int y = 0; y < this.mapEditorProperties.CurrentMap.Tiles.GetLength(1); y++)
                        {
                            this.RemoveTile(new Vector2i(x, y));
                        }
                    }

                    this.clearMap = false;
                }

                this.mapRenderWindow.DispatchEvents();
                this.tileSetRenderWindow.DispatchEvents();

                this.mapRenderWindow.Clear();
                this.tileSetRenderWindow.Clear();

                if (this.mapEditorProperties.CurrentMap != null)
                    this.mapEditorProperties.CurrentMap.Draw(mapRenderWindow);

                this.tileSetRenderWindow.Draw(this.currentTileSetSprite);

                this.tileSetRenderWindow.Draw(this.tileSetSelector);

                this.mapRenderWindow.Display();
                this.tileSetRenderWindow.Display();
            }
        }

        public void ScrollTileSet(int x, int y)
        {
            this.tileSetRenderWindow.SetView(new View(new FloatRect(x, y, this.tileSetRenderWindow.Size.X, this.tileSetRenderWindow.Size.Y)));
        }

        public void ShowNextTileset()
        {
            int nextIndex = this.tileSetTextures.IndexOf(this.currentTileSetSprite.Texture) + 1;

            if (nextIndex >= this.tileSetTextures.Count)
                return;

            this.currentTileSetSprite = new Sprite(this.tileSetTextures[nextIndex]);
        }

        public void ShowPreviousTileset()
        {
            int nextIndex = this.tileSetTextures.IndexOf(this.currentTileSetSprite.Texture) - 1;

            if (nextIndex < 0)
                return;

            this.currentTileSetSprite = new Sprite(this.tileSetTextures[nextIndex]);
        }

        public void FillMap()
        {
            for (int x = 0; x < this.mapEditorProperties.CurrentMap.Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < this.mapEditorProperties.CurrentMap.Tiles.GetLength(1); y++)
                {
                    this.PlaceTile(new Vector2i(x, y));
                }
            }
        }

        public void ClearMap()
        {
            this.clearMap = true;
        }
    }
}