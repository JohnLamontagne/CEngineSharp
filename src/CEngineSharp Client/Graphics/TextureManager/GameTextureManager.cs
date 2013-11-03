using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace CEngineSharp_Client.Graphics.TextureManager
{
    public class GameTextureManager : ITextureManager
    {
        private Dictionary<string, Texture> characterTextures;

        private List<Texture> tileSetTextures;

        public Texture GetCharacterTexture(string textureName)
        {
            return this.characterTextures[textureName];
        }

        public Texture GetTileSetTexture(int index)
        {
            return tileSetTextures[index];
        }

        public int GetTileSetTextureIndex(Texture texture)
        {
            return tileSetTextures.IndexOf(texture);
        }

        public void LoadTextures()
        {
            this.characterTextures = new Dictionary<string, Texture>();
            this.tileSetTextures = new List<Texture>();

            this.LoadCharacterTextures();
            this.LoadTileSetTextures();
        }

        private void LoadCharacterTextures()
        {
            DirectoryInfo dI = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"/Data/Graphics/Characters/");

            foreach (var file in dI.GetFiles("*.png"))
            {
                this.characterTextures.Add(file.Name.Remove(file.Name.Length - 4, 4), new Texture(file.FullName));
            }
        }

        private void LoadTileSetTextures()
        {
            DirectoryInfo dI = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"/Data/Graphics/TileSets/");

            foreach (var file in dI.GetFiles("*.png"))
            {
                this.tileSetTextures.Add(new Texture(file.FullName));
            }
        }

        public void UnloadTextures()
        {
        }
    }
}