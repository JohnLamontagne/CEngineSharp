using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

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
            foreach (KeyValuePair<string, Texture> value in _textures)
                if (value.Value == texture) return value.Key;

            return null;
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
            DirectoryInfo dI = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"/Data/Graphics/Characters/");

            foreach (var file in dI.GetFiles("*.png"))
            {
                _textures.Add("character" + file.Name.Remove(file.Name.Length - 4, 4), new Texture(file.FullName));
            }
        }

        private void LoadTileSetTextures()
        {
            DirectoryInfo dI = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"/Data/Graphics/TileSets/");

            foreach (var file in dI.GetFiles("*.png"))
            {
                _textures.Add("tileset" + file.Name.Remove(file.Name.Length - 4, 4), new Texture(file.FullName));
            }
        }

        private void LoadItemTextures()
        {
            DirectoryInfo dI = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"/Data/Graphics/Items/");

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