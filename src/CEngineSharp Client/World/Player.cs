using SFML.Graphics;
using SFML.Window;
using System;

namespace CEngineSharp_Client.World
{
    public class Player
    {
        private readonly Sprite _sprite;

        public Sprite PlayerSprite
        {
            get { return _sprite; }
        }

        public int X
        {
            get { return (int)_sprite.Position.X / 32; }
        }

        public int Y
        {
            get { return (int)_sprite.Position.Y / 32; }
        }

        public Player(Texture texture)
        {
            _sprite = new Sprite(texture);
        }

        public void MovePlayer(int newX, int newY)
        {
            _sprite.Position = new Vector2f(newX * 32, newY * 32);
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(_sprite);
        }
    }
}