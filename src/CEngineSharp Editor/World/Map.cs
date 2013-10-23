using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace CEngineSharp_Editor.World
{
    public class Map
    {
        public enum Layers
        {
            Ground,
            Mask
        }

        public class Tile
        {
            public bool Blocked { get; set; }

            public bool IsOccupied { get; set; }

            public Layer[] Layers { get; set; }

            public class Layer
            {
                public Sprite Sprite { get; set; }

                public Layer(Sprite sprite, int posX, int posY)
                {
                    this.Sprite = sprite;
                    this.Sprite.Position = new SFML.Window.Vector2f(posX * 32, posY * 32);
                }
            }

            public Tile()
            {
                this.Layers = new Layer[(int)Map.Layers.Mask + 1];
            }
        }

        public Tile[,] Tiles
        {
            get;
            set;
        }

        public Map(int mapEditorWidth, int mapEditorHeight)
        {
            this.Tiles = new Tile[mapEditorWidth / 32, mapEditorHeight / 32];
        }

        public void ResizeMap(int newWidth, int newHeight)
        {
            var newArray = new Tile[newWidth, newHeight];
            int columnCount = Tiles.GetLength(1);
            int columnCount2 = newHeight;
            int columns = Tiles.GetUpperBound(0);
            for (int co = 0; co <= columns; co++)
                Array.Copy(Tiles, co * columnCount, newArray, co * columnCount2, columnCount);

            Tiles = newArray;
        }

        public void Draw(RenderWindow window)
        {
            foreach (var tile in Tiles)
            {
                if (tile == null) continue;

                foreach (var layer in tile.Layers)
                {
                    if (layer == null || layer.Sprite == null)
                        continue;

                    window.Draw(layer.Sprite);
                }
            }
        }
    }
}