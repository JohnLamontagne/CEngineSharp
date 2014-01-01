using SFML.Graphics;
using System.Collections.Generic;

namespace CEngineSharp_Client.Graphics.TextureManager
{
    public interface ITextureManager
    {
        Dictionary<string, Texture> GetTextures();

        Texture GetTexture(string textureName);

        string GetTextureName(Texture texture);

        void LoadTextures();

        void UnloadTextures();
    }
}