using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.Net.Packets;
using SFML.Graphics;
using SFML.Window;
using System;

namespace CEngineSharp_Client.World.Entity
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

        public bool CanMove { get; set; }

        public bool IsMoving { get; set; }

        public float PlayerSpeed { get; set; }

        public int Step
        {
            get
            {
                return _sprite.TextureRect.Left / 32;
            }
            set
            {
                _sprite.TextureRect = new IntRect(value * 32, (int)this.Direction * 32, 32, 32);
            }
        }

        public Directions Direction
        {
            get
            {
                return (Directions)(_sprite.TextureRect.Top / 32);
            }
            set
            {
                _sprite.TextureRect = new IntRect(this.Step * 32, (int)value * 32, 32, 32);
            }
        }

        public Player(Texture texture)
        {
            _sprite = new Sprite(texture);
            this.Direction = Directions.Down;
            this.Step = 0;
            this.CanMove = true;
            this.PlayerSpeed = 2f;
        }

        public void MovePlayer(int newX, int newY)
        {
            this.X = newX;
            this.Y = newY;

            if (this.Step < 2)
            {
                this.Step++;
            }
            else
            {
                this.Step = 0;
            }
        }

        public void Update()
        {
            if (this.IsMoving && this.CanMove)
            {
                var movementPacket = new MovementPacket();

                switch (this.Direction)
                {
                    case Directions.Down:
                        movementPacket.WriteData(GameWorld.Players[Globals.MyIndex].X, GameWorld.Players[Globals.MyIndex].Y + 1);
                        break;

                    case Directions.Up:
                        movementPacket.WriteData(GameWorld.Players[Globals.MyIndex].X, GameWorld.Players[Globals.MyIndex].Y - 1);
                        break;

                    case Directions.Right:
                        movementPacket.WriteData(GameWorld.Players[Globals.MyIndex].X + 1, GameWorld.Players[Globals.MyIndex].Y);
                        break;

                    case Directions.Left:
                        movementPacket.WriteData(GameWorld.Players[Globals.MyIndex].X - 1, GameWorld.Players[Globals.MyIndex].Y);
                        break;
                }

                Networking.SendPacket(movementPacket);
                this.CanMove = false;
            }

            if (this.PlayerSprite.Position.X != this.X * 32 || this.PlayerSprite.Position.Y != this.Y * 32)
                this.UpdateSpritePosition();
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(_sprite);
        }

        private void UpdateSpritePosition()
        {
            int x = this.X * 32;
            int y = this.Y * 32;
            Vector2f position = this.PlayerSprite.Position;

            if (x < this.PlayerSprite.Position.X)
            {
                position.X = this.PlayerSprite.Position.X - this.PlayerSpeed;
            }
            else if (x > this.PlayerSprite.Position.X)
            {
                position.X = this.PlayerSprite.Position.X + this.PlayerSpeed;
            }

            if (y < this.PlayerSprite.Position.Y)
            {
                position.Y = this.PlayerSprite.Position.Y - this.PlayerSpeed;
            }
            else if (y > this.PlayerSprite.Position.Y)
            {
                position.Y = this.PlayerSprite.Position.Y + this.PlayerSpeed;
            }

            this.PlayerSprite.Position = position;

            if (this.PlayerSprite.Position.X == x && this.PlayerSprite.Position.Y == y)
            {
                this.CanMove = true;
            }
        }
    }
}