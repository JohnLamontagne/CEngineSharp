using CEngineSharp_Client.Graphics.TextureManager;
using SFML.Graphics;
using System;

namespace CEngineSharp_Client.Graphics
{
    public class MenuTextureManager : ITextureManager
    {
        public Texture MenuBackgroundTexture { get; private set; }

        public void LoadTextures()
        {
            this.MenuBackgroundTexture = new Texture(AppDomain.CurrentDomain.BaseDirectory + @"\Data\Graphics\Backgrounds\MainMenu.png");
        }

        public void UnloadTextures()
        {
            this.MenuBackgroundTexture = null;
        }
    }
}