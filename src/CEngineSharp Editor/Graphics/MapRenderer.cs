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
        private readonly RenderWindow _mapRenderWindow;

        private readonly RenderWindow _tileSetRenderWindow;

        private bool _clearMap;

        private readonly MapEditor.MapEditorProperties _mapEditorProperties;

        private readonly Text _mousePositionText;

        public List<Texture> TileSetTextures { get { return this._tileSetTextures; } }

        private Sprite _selectedTileSprite;

        private Sprite _currentTileSetSprite;

        private List<Texture> _tileSetTextures;

        private RectangleShape _tileSetSelector;

        private View _tileSetView;

        public uint CurrentTileSetWidth { get { return _currentTileSetSprite.Texture.Size.X; } }

        public uint CurrentTileSetHeight { get { return _currentTileSetSprite.Texture.Size.Y; } }

        public bool Running { get; set; }

        public MapRenderer(IntPtr mapRenderWindowHandle, IntPtr tileSetRenderHandle, MapEditor.MapEditorProperties mapEditorProperties)
        {
            this._mapRenderWindow = new RenderWindow(mapRenderWindowHandle);
            this._tileSetRenderWindow = new RenderWindow(tileSetRenderHandle);
            this._mapRenderWindow.MouseButtonPressed += mapRenderWindow_MouseButtonPressed;
            this._mapRenderWindow.MouseMoved += mapRenderWindow_MouseMoved;
            this._tileSetRenderWindow.MouseButtonPressed += tileSetRenderWindow_MouseButtonPressed;
            this._tileSetRenderWindow.MouseMoved += tileSetRenderWindow_MouseMoved;
            this._tileSetRenderWindow.MouseButtonReleased += tileSetRenderWindow_MouseButtonReleased;

            this._mapEditorProperties = mapEditorProperties;
            this._mapEditorProperties.CurrentLayer = World.Map.Layers.Ground;
            this._mapEditorProperties.MapView = new View(this._mapRenderWindow.DefaultView);

            this._mousePositionText = new Text("", new Font(AppDomain.CurrentDomain.BaseDirectory + "/Data/Graphics/Fonts/MainFont.ttf"), 20);

            this.LoadTileSets();

            this._tileSetView = this._tileSetRenderWindow.DefaultView;

            this.Running = true;

            this._mapRenderWindow.SetActive(false);
            this._tileSetRenderWindow.SetActive(false);

            new Thread(UpdateLoop).Start();
        }

        private void LoadTileSets()
        {
            var dI = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/Data/Graphics/Tile Sets/");

            this._tileSetTextures = new List<Texture>();

            foreach (var file in dI.GetFiles("*.png", SearchOption.TopDirectoryOnly))
            {
                _tileSetTextures.Add(new Texture(file.FullName));
            }

            this._currentTileSetSprite = new Sprite(_tileSetTextures[0]);

            this._tileSetSelector = new RectangleShape(new Vector2f(32, 32));
            this._tileSetSelector.OutlineColor = Color.Red;
            this._tileSetSelector.FillColor = Color.Transparent;
            this._tileSetSelector.OutlineThickness = 2f;

            this._selectedTileSprite = new Sprite(this._currentTileSetSprite.Texture, new IntRect((int)_tileSetSelector.Position.X, (int)_tileSetSelector.Position.Y, 32, 32));
        }

        private void PlaceTile(Vector2i position)
        {
            position = new Vector2i(position.X + this._mapEditorProperties.MapEditorLeft, position.Y + this._mapEditorProperties.MapEditorTop);

            for (int x = 0; x < this._selectedTileSprite.TextureRect.Width / 32; x++)
            {
                for (int y = 0; y < this._selectedTileSprite.TextureRect.Height / 32; y++)
                {
                    var tileSpriteRect = new IntRect(this._selectedTileSprite.TextureRect.Left + (x * 32), this._selectedTileSprite.TextureRect.Top + (y * 32), 32, 32);
                    var tileSprite = new Sprite(this._selectedTileSprite.Texture, tileSpriteRect);

                    if (x + position.X >= this._mapEditorProperties.CurrentMap.MapWidth || y + position.Y >= this._mapEditorProperties.CurrentMap.MapHeight)
                        return;

                    if (this._mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y) == null)
                        this._mapEditorProperties.CurrentMap.SetTile(position.X + x, position.Y + y, new Map.Tile());

                    var layer = new Map.Tile.Layer(tileSprite, position.X + x, position.Y + y);
                    this._mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).Layers[(int)this._mapEditorProperties.CurrentLayer] = layer;

                    if (this._mapEditorProperties.TileBlockedAttribute)
                    {
                        this._mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).Blocked = true;

                        this._mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).BlockedCover = new RectangleShape(new SFML.Window.Vector2f(32, 32));
                        this._mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).BlockedCover.FillColor = new Color(255, 0, 0, 100);
                        this._mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).BlockedCover.Position = new Vector2f((position.X + x) * 32, (position.Y + y) * 32);
                    }
                    else
                    {
                        this._mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).Blocked = false;

                        this._mapEditorProperties.CurrentMap.GetTile(position.X + x, position.Y + y).BlockedCover = null;
                    }
                }
            }
        }

        private void RemoveTile(Vector2i position)
        {
            position = new Vector2i(position.X + this._mapEditorProperties.MapEditorLeft, position.Y + this._mapEditorProperties.MapEditorTop);

            if (this._mapEditorProperties.CurrentMap.GetTile(position.X, position.Y) == null ||
                  this._mapEditorProperties.CurrentMap.GetTile(position.X, position.Y).Layers[(int)this._mapEditorProperties.CurrentLayer] == null)
            {
                return;
            }

            this._mapEditorProperties.CurrentMap.GetTile(position.X, position.Y).BlockedCover = null;
            this._mapEditorProperties.CurrentMap.GetTile(position.X, position.Y).Layers[(int)this._mapEditorProperties.CurrentLayer] = null;
        }

        private void UpdateLoop()
        {
            while (this.Running)
            {
                if (this._clearMap)
                {
                    for (int x = 0; x < this._mapEditorProperties.CurrentMap.MapWidth; x++)
                    {
                        for (int y = 0; y < this._mapEditorProperties.CurrentMap.MapHeight; y++)
                        {
                            this.RemoveTile(new Vector2i(x, y));
                        }
                    }

                    this._clearMap = false;
                }

                this.Render();
            }
        }

        private void Render()
        {
            this._mapRenderWindow.DispatchEvents();
            this._tileSetRenderWindow.DispatchEvents();

            this._mapRenderWindow.Clear();
            this._tileSetRenderWindow.Clear();

            this._mapRenderWindow.SetView(_mapEditorProperties.MapView);

            if (this._mapEditorProperties.CurrentMap != null)
                this._mapEditorProperties.CurrentMap.Draw(this._mapRenderWindow, this._mapEditorProperties.MapEditorLeft, this._mapEditorProperties.MapEditorTop, this._mapEditorProperties.MapEditorWidth, this._mapEditorProperties.MapEditorHeight);

            this._mapRenderWindow.SetView(this._mapRenderWindow.DefaultView);

            this._mapRenderWindow.Draw(_mousePositionText);

            this._tileSetRenderWindow.Draw(this._currentTileSetSprite);

            this._tileSetRenderWindow.Draw(this._tileSetSelector);

            this._mapRenderWindow.Display();
            this._tileSetRenderWindow.Display();
        }

        public void ScrollTileSet(int x, int y)
        {
            this._tileSetRenderWindow.SetView(new View(new FloatRect(x, y, this._tileSetRenderWindow.Size.X, this._tileSetRenderWindow.Size.Y)));
        }

        public void ShowNextTileset()
        {
            int nextIndex = this._tileSetTextures.IndexOf(this._currentTileSetSprite.Texture) + 1;

            if (nextIndex >= this._tileSetTextures.Count)
                return;

            this._currentTileSetSprite = new Sprite(this._tileSetTextures[nextIndex]);

            this._selectedTileSprite = new Sprite(this._currentTileSetSprite.Texture, new IntRect((int)_tileSetSelector.Position.X, (int)_tileSetSelector.Position.Y, 32, 32));
        }

        public void ShowPreviousTileset()
        {
            int nextIndex = this._tileSetTextures.IndexOf(this._currentTileSetSprite.Texture) - 1;

            if (nextIndex < 0)
                return;

            this._currentTileSetSprite = new Sprite(this._tileSetTextures[nextIndex]);

            this._selectedTileSprite = new Sprite(this._currentTileSetSprite.Texture, new IntRect((int)_tileSetSelector.Position.X, (int)_tileSetSelector.Position.Y, 32, 32));
        }

        public void FillMap()
        {
            for (int x = 0; x < this._mapEditorProperties.CurrentMap.MapWidth; x++)
            {
                for (int y = 0; y < this._mapEditorProperties.CurrentMap.MapHeight; y++)
                {
                    this.PlaceTile(new Vector2i(x, y));
                }
            }
        }

        public void ClearMap()
        {
            this._clearMap = true;
        }

        private void mapRenderWindow_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            int mouseX = e.X / 32;
            int mouseY = e.Y / 32;

            _mousePositionText.DisplayedString = "X: " + (mouseX + this._mapEditorProperties.MapEditorLeft) + " Y: " + (mouseY + this._mapEditorProperties.MapEditorTop);

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
                Vector2f realPosition = this._tileSetRenderWindow.MapPixelToCoords(new Vector2i(e.X, e.Y));

                int width = (int)(realPosition.X - this._tileSetSelector.Position.X);
                int height = (int)(realPosition.Y - this._tileSetSelector.Position.Y);

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

                this._tileSetSelector.Size = new Vector2f(width, height);

                this._selectedTileSprite = this._selectedTileSprite = new Sprite(this._currentTileSetSprite.Texture, new IntRect((int)_tileSetSelector.Position.X, (int)_tileSetSelector.Position.Y, width, height));
            }
        }

        private void tileSetRenderWindow_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                Vector2f realPosition = this._tileSetRenderWindow.MapPixelToCoords(new Vector2i(e.X, e.Y));

                int width = (int)(realPosition.X - this._tileSetSelector.Position.X);
                int height = (int)(realPosition.Y - this._tileSetSelector.Position.Y);

                this._tileSetSelector.Size = new Vector2f(width, height);
            }
        }

        private void tileSetRenderWindow_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                this._tileSetSelector.Position = _tileSetRenderWindow.MapPixelToCoords((new Vector2i((e.X / 32) * 32, (e.Y / 32) * 32)));
                this._tileSetSelector.Size = new Vector2f(32, 32);

                this._selectedTileSprite = new Sprite(this._currentTileSetSprite.Texture, new IntRect((int)_tileSetSelector.Position.X, (int)_tileSetSelector.Position.Y, 32, 32));
            }
        }
    }
}