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

        private Text mousePositionText;

        public List<Texture> TileSetTextures { get { return this.tileSetTextures; } }

        private Sprite selectedTileSprite;

        private Sprite currentTileSetSprite;

        private List<Texture> tileSetTextures;

        private RectangleShape tileSetSelector;

        private View tileSetView;

        public uint CurrentTileSetWidth { get { return currentTileSetSprite.Texture.Size.X; } }

        public uint CurrentTileSetHeight { get { return currentTileSetSprite.Texture.Size.Y; } }

        public bool Running { get; set; }

        public MapRenderer(IntPtr mapRenderWindowHandle, IntPtr tileSetRenderHandle, MapEditor.MapEditorProperties mapEditorProperties)
        {
            this.mapRenderWindow = new RenderWindow(mapRenderWindowHandle);
            this.tileSetRenderWindow = new RenderWindow(tileSetRenderHandle);
            this.mapRenderWindow.MouseButtonPressed += mapRenderWindow_MouseButtonPressed;
            this.mapRenderWindow.MouseMoved += mapRenderWindow_MouseMoved;
            this.tileSetRenderWindow.MouseButtonPressed += tileSetRenderWindow_MouseButtonPressed;
            this.tileSetRenderWindow.MouseMoved += tileSetRenderWindow_MouseMoved;
            this.tileSetRenderWindow.MouseButtonReleased += tileSetRenderWindow_MouseButtonReleased;

            this.mapEditorProperties = mapEditorProperties;
            this.mapEditorProperties.CurrentLayer = World.Map.Layers.Ground;
            this.mapEditorProperties.MapView = new View(this.mapRenderWindow.DefaultView);

            this.mousePositionText = new Text("", new Font(AppDomain.CurrentDomain.BaseDirectory + "/Data/Graphics/Fonts/MainFont.ttf"), 20);

            this.LoadTileSets();

            this.tileSetView = this.tileSetRenderWindow.DefaultView;

            this.Running = true;

            this.mapRenderWindow.SetActive(false);
            this.tileSetRenderWindow.SetActive(false);

            new Thread(UpdateLoop).Start();
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
            position = new Vector2i(position.X + this.mapEditorProperties.MapEditorLeft, position.Y + this.mapEditorProperties.MapEditorTop);

            for (int x = 0; x < this.selectedTileSprite.TextureRect.Width / 32; x++)
            {
                for (int y = 0; y < this.selectedTileSprite.TextureRect.Height / 32; y++)
                {
                    IntRect tileSpriteRect = new IntRect(this.selectedTileSprite.TextureRect.Left + (x * 32), this.selectedTileSprite.TextureRect.Top + (y * 32), 32, 32);
                    Sprite tileSprite = new Sprite(this.selectedTileSprite.Texture, tileSpriteRect);

                    if (x + position.X >= this.mapEditorProperties.CurrentMap.MapWidth || y + position.Y >= this.mapEditorProperties.CurrentMap.MapHeight)
                        return;

                    if (this.mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y) == null)
                        this.mapEditorProperties.CurrentMap.SetTile(position.X + x, position.Y + y, new Map.Tile());

                    Map.Tile.Layer layer = new Map.Tile.Layer(tileSprite, position.X + x, position.Y + y);
                    this.mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).Layers[(int)this.mapEditorProperties.CurrentLayer] = layer;

                    if (this.mapEditorProperties.TileBlockedAttribute)
                    {
                        this.mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).Blocked = true;

                        this.mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).BlockedCover = new RectangleShape(new SFML.Window.Vector2f(32, 32));
                        this.mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).BlockedCover.FillColor = new Color(255, 0, 0, 100);
                        this.mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).BlockedCover.Position = new Vector2f((position.X + x) * 32, (position.Y + y) * 32);
                    }
                    else
                    {
                        this.mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).Blocked = false;

                        this.mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).BlockedCover = null;
                    }
                }
            }
        }

        private void RemoveTile(Vector2i position)
        {
            position = new Vector2i(position.X + this.mapEditorProperties.MapEditorLeft, position.Y + this.mapEditorProperties.MapEditorTop);

            if (this.mapEditorProperties.CurrentMap.GetTile(position.X, position.Y) == null ||
                  this.mapEditorProperties.CurrentMap.GetTile(position.X, position.Y).Layers[(int)this.mapEditorProperties.CurrentLayer] == null)
            {
                return;
            }

            this.mapEditorProperties.CurrentMap.GetTile(position.X, position.Y).BlockedCover = null;
            this.mapEditorProperties.CurrentMap.GetTile(position.X, position.Y).Layers[(int)this.mapEditorProperties.CurrentLayer] = null;
        }

        private void UpdateLoop()
        {
            while (this.Running)
            {
                if (this.clearMap)
                {
                    for (int x = 0; x < this.mapEditorProperties.CurrentMap.MapWidth; x++)
                    {
                        for (int y = 0; y < this.mapEditorProperties.CurrentMap.MapHeight; y++)
                        {
                            this.RemoveTile(new Vector2i(x, y));
                        }
                    }

                    this.clearMap = false;
                }

                this.Render();
            }
        }

        private void Render()
        {
            this.mapRenderWindow.DispatchEvents();
            this.tileSetRenderWindow.DispatchEvents();

            this.mapRenderWindow.Clear();
            this.tileSetRenderWindow.Clear();

            this.mapRenderWindow.SetView(mapEditorProperties.MapView);

            if (this.mapEditorProperties.CurrentMap != null)
                this.mapEditorProperties.CurrentMap.Draw(this.mapRenderWindow, this.mapEditorProperties.MapEditorLeft, this.mapEditorProperties.MapEditorTop, this.mapEditorProperties.MapEditorWidth, this.mapEditorProperties.MapEditorHeight);

            this.mapRenderWindow.SetView(this.mapRenderWindow.DefaultView);

            this.mapRenderWindow.Draw(mousePositionText);

            this.tileSetRenderWindow.Draw(this.currentTileSetSprite);

            this.tileSetRenderWindow.Draw(this.tileSetSelector);

            this.mapRenderWindow.Display();
            this.tileSetRenderWindow.Display();
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

            this.selectedTileSprite = new Sprite(this.currentTileSetSprite.Texture, new IntRect((int)tileSetSelector.Position.X, (int)tileSetSelector.Position.Y, 32, 32));
        }

        public void ShowPreviousTileset()
        {
            int nextIndex = this.tileSetTextures.IndexOf(this.currentTileSetSprite.Texture) - 1;

            if (nextIndex < 0)
                return;

            this.currentTileSetSprite = new Sprite(this.tileSetTextures[nextIndex]);

            this.selectedTileSprite = new Sprite(this.currentTileSetSprite.Texture, new IntRect((int)tileSetSelector.Position.X, (int)tileSetSelector.Position.Y, 32, 32));
        }

        public void FillMap()
        {
            for (int x = 0; x < this.mapEditorProperties.CurrentMap.MapWidth; x++)
            {
                for (int y = 0; y < this.mapEditorProperties.CurrentMap.MapHeight; y++)
                {
                    this.PlaceTile(new Vector2i(x, y));
                }
            }
        }

        public void ClearMap()
        {
            this.clearMap = true;
        }

        private void mapRenderWindow_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            int mouseX = e.X / 32;
            int mouseY = e.Y / 32;

            mousePositionText.DisplayedString = "X: " + (mouseX + this.mapEditorProperties.MapEditorLeft) + " Y: " + (mouseY + this.mapEditorProperties.MapEditorTop);

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
    }
}