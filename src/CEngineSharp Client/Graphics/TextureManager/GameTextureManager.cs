using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CEngineSharp_Client.Graphics.TextureManager
{
    public class GameTextureManager : ITextureManager
    {
        private Dictionary<string, Texture> _textures;

        public Dictionary<string, Texture> GetTextures()
        {
            return _textures;
        }

        public Texture GetTexture(string textureName)
        {
            return _textures[textureName];
        }

        public string GetTextureName(Texture texture)
        {
            return (from value in _textures where value.Value == texture select value.Key).FirstOrDefault();
        }

        public void LoadTextures()
        {
            _textures = new Dictionary<string, Texture>();

            this.LoadCharacterTextures();
            this.LoadTileSetTextures();
            this.LoadItemTextures();
        }

        private void LoadCharacterTextures()
        {
            var dI = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"/Data/Graphics/Characters/");

            foreach (var file in dI.GetFiles("*.png"))
            {
                _textures.Add("character" + file.Name.Remove(file.Name.Length - 4, 4), new Texture(file.FullName));
            }
        }

        private void LoadTileSetTextures()
        {
            var dI = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"/Data/Graphics/Tilesets/");

            foreach (var file in dI.GetFiles("*.png"))
            {
                _textures.Add("tileset" + file.Name.Remove(file.Name.Length - 4, 4), new Texture(file.FullName));
            }
        }

        private void LoadItemTextures()
        {
            var dI = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"/Data/Graphics/Items/");

            foreach (var file in dI.GetFiles("*.png"))
            {
                _textures.Add("item" + file.Name.Remove(file.Name.Length - 4, 4), new Texture(file.FullName));
            }
        }

        public void UnloadTextures()
        {
        }
    }
}