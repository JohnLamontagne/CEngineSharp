using CEngineSharp_Client.Graphics;
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

        public int X { get; private set; }

        public int Y { get; private set; }

        public Player(Texture texture)
        {
            _sprite = new Sprite(texture);
        }

        public void MovePlayer(int newX, int newY)
        {
            this.X = newX;
            this.Y = newY;

            Vector2f spritePosition = new Vector2f();

            if (this.X <= Globals.CurrentResolutionWidth / 2)
            {
                spritePosition.X = this.X;
            }
            else if (this.X > ((Globals.CurrentResolutionWidth / 2) / 32) && this.X < (GameWorld.CurrentMap.Width - ((Globals.CurrentResolutionWidth / 2) / 32)))
            {
                spritePosition.X = Globals.CurrentResolutionWidth / 2;
            }
            else
            {
                spritePosition.X = GameWorld.CurrentMap.Width - Math.Abs(GameWorld.CurrentMap.Width - this.X);
            }

            if (this.Y <= Globals.CurrentResolutionHeight / 2)
            {
                spritePosition.Y = this.Y;
            }
            else if (this.Y > ((Globals.CurrentResolutionHeight / 2) / 32) && this.Y < (GameWorld.CurrentMap.Height - ((Globals.CurrentResolutionHeight / 2) / 32)))
            {
                spritePosition.Y = Globals.CurrentResolutionHeight / 2;
            }
            else
            {
                spritePosition.Y = GameWorld.CurrentMap.Height - Math.Abs(GameWorld.CurrentMap.Height - this.Y);
            }

            this.PlayerSprite.Position = spritePosition;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(_sprite);
        }
    }
}