using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.Utilities;
using SFML.Graphics;

namespace CEngineSharp_Client.World
{
    public class Item
    {
        public string Name { get; private set; }

        public Sprite Sprite { get; private set; }

        public Item(string name, int itemTextureNumber)
        {
            this.Name = name;
            this.Sprite = new Sprite(ServiceLocator.ScreenManager.ActiveScreen.TextureManager.GetTexture("item" + itemTextureNumber));
        }
    }
}