using CEngineSharp_Client.World.Content_Managers;
using SFML.Graphics;
using SFML.Window;

using System;

namespace CEngineSharp_Client.World.Entity
{
    public class Npc : IEntity
    {
        private byte _previousStep;

        public string Name { get; set; }

        public Sprite Sprite { get; set; }

        public bool IsMoving { get; set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public int Level { get; set; }

        public float Speed { get; set; }

        public byte Step
        {
            get
            {
                return (byte)(this.Sprite.TextureRect.Left / 32);
            }
            set
            {
                this.Sprite.TextureRect = new IntRect(value * 32, (int)this.Direction * 32, 32, 32);
            }
        }

        public Directions Direction
        {
            get
            {
                return (Directions)(Sprite.TextureRect.Top / 32);
            }
            set
            {
                this.Sprite.TextureRect = new IntRect(this.Step * 32, (int)value * 32, 32, 32);
            }
        }

        public void Update(GameTime gameTime)
        {
            var position = new Vector2f();

            var x = this.X * 32;
            var y = this.Y * 32;

            if (x < this.Sprite.Position.X)
            {
                position.X = this.Sprite.Position.X - (this.Speed * gameTime.UpdateTime);

                if (x >= position.X)
                {
                    position.X = x;
                }
            }
            else if (x > this.Sprite.Position.X)
            {
                position.X = this.Sprite.Position.X + (this.Speed * gameTime.UpdateTime);

                if (x <= position.X)
                {
                    position.X = x;
                }
            }
            else
            {
                position.X = x;
            }

            if (y < this.Sprite.Position.Y)
            {
                position.Y = this.Sprite.Position.Y - (this.Speed * gameTime.UpdateTime);

                if (y >= position.Y)
                {
                    position.Y = y;
                }
            }
            else if (y > this.Sprite.Position.Y)
            {
                position.Y = this.Sprite.Position.Y + (this.Speed * gameTime.UpdateTime);

                if (y <= position.Y)
                {
                    position.Y = y;
                }
            }
            else
            {
                position.Y = y;
            }

            this.Sprite.Position = position;

        }

        public void Warp(int newX, int newY, Directions direction)
        {
            this.Direction = direction;

            MapManager.Map.GetTile(this.X, this.Y).IsOccupied = false;

            this.X = newX;
            this.Y = newY;

            MapManager.Map.GetTile(this.X, this.Y).IsOccupied = true;

            this.Sprite.Position = new Vector2f(newX * 32, newY * 32);
        }

        public void Move(int newX, int newY, Directions direction)
        {
            this.Direction = direction;

            MapManager.Map.GetTile(this.X, this.Y).IsOccupied = false;

            this.X = newX;
            this.Y = newY;

            MapManager.Map.GetTile(this.X, this.Y).IsOccupied = true;

            if (this.Step == 0)
            {
                this._previousStep = 0;
                this.Step++;
            }
            else if (this.Step == 2)
            {
                this._previousStep = 2;
                this.Step--;
            }
            else if (this.Step == 1)
            {
                if (this._previousStep == 2)
                    this.Step--;
                else
                    this.Step++;
            }

            this.IsMoving = true;
        }

        public void Draw(RenderTarget target)
        {
            target.Draw(this.Sprite);
        }


        public Vector2i Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}