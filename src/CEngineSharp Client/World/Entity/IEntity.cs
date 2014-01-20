using SFML.Graphics;
using SFML.Window;

namespace CEngineSharp_Client.World.Entity
{
    public interface IEntity
    {
        string Name { get; set; }

        Vector2i Position { get; set; }

        Sprite Sprite { get; set; }

        void Draw(RenderTarget target);
    }
}