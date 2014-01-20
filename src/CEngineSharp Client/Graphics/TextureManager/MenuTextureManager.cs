using CEngineSharp_Client.Graphics.TextureManager;
using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace CEngineSharp_Client.Graphics
{
    public class MenuTextureManager : ITextureManager
    {
        private Dictionary<string, Texture> _textures;

        public void LoadTextures()
        {
            _textures = new Dictionary<string, Texture>
            {
                {
                    "MenuBackground",
                    new Texture(AppDomain.CurrentDomain.BaseDirectory + @"\Data\Graphics\Backgrounds\MainMenu.png")
                }
            };
        }

        public void UnloadTextures()
        {
            _textures.Clear();
        }

        public Dictionary<string, Texture> GetTextures()
        {
            throw new NotImplementedException();
        }

        public Texture GetTexture(string textureName)
        {
            return _textures[textureName];
        }


        public string GetTextureName(Texture texture)
        {
            throw new NotImplementedException();
        }
    }
}