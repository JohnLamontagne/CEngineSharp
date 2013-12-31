using CEngineSharp_Client.Graphics;
using CEngineSharp_Client.Net;
using CEngineSharp_Client.Net.Packets;
using CEngineSharp_Client.World.Content_Managers;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using TGUI;

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

        public Camera Camera { get; set; }

        public int HP { get; set; }

        private List<Item> _inventory;

        private byte previousStep;

        public byte Step
        {
            get
            {
                return (byte)(_sprite.TextureRect.Left / 32);
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
            _inventory = new List<Item>();

            this.Direction = Directions.Down;
            this.Step = 0;
            this.CanMove = true;
            this.PlayerSpeed = .2f;
            this.Camera = new Camera(this);
        }

        public int InvenCordToSlot(int invenX, int invenY)
        {
            GameRenderer gameRenderer = (RenderManager.CurrentRenderer as GameRenderer);

            int invenPosX = (int)gameRenderer.Gui.Get<Picture>("picInventory").Position.X;
            int invenPosY = (int)gameRenderer.Gui.Get<Picture>("picInventory").Position.Y;

            int x = (invenX - invenPosX) / 32;
            int y = (invenY - invenPosY) / 32;

            return (x + (y * 5));
        }

        public Item GetInventoryItem(int slotNum)
        {
            if (slotNum >= _inventory.Count || slotNum < 0)
                return null;

            return _inventory[slotNum];
        }

        public Item GetInventoryItem(int invenX, int invenY)
        {
            return this.GetInventoryItem(this.InvenCordToSlot(invenX, invenY));
        }

        public void ClearInventory()
        {
            _inventory.Clear();
        }

        public void AddInventoryItem(Item item)
        {
            _inventory.Add(item);

            int slotNum = _inventory.Count - 1;

            GameRenderer gameRenderer = (RenderManager.CurrentRenderer as GameRenderer);

            float invenPosX = gameRenderer.Gui.Get<Picture>("picInventory").Position.X;
            float invenPosY = gameRenderer.Gui.Get<Picture>("picInventory").Position.Y;

            int itemPosY = (int)invenPosY + (32 * (slotNum / 5));

            int itemPosX = (int)invenPosX + (32 * (slotNum - (((itemPosY - (int)invenPosY) / 32) * 5)));

            item.Sprite.Position = new Vector2f(itemPosX, itemPosY);
        }

        public void TryDropInventoryItem(int invenX, int invenY)
        {
            int slotNum = this.InvenCordToSlot(invenX, invenY);

            if (this.GetInventoryItem(slotNum) != null)
            {
                DropItemPacket dropItemPacket = new DropItemPacket();
                dropItemPacket.WriteData(slotNum);
                Networking.SendPacket(dropItemPacket);
            }
        }

        public void Warp(int newX, int newY, Directions direction)
        {
            this.Direction = direction;

            this.X = newX;
            this.Y = newY;

            this.PlayerSprite.Position = new Vector2f(newX * 32, newY * 32);
        }

        public void Move(int newX, int newY, Directions direction)
        {
            this.Direction = direction;
            int oldX = this.X;
            int oldY = this.Y;

            this.X = newX;
            this.Y = newY;

            MapManager.Map.GetTile(oldX, oldY).IsOccupied = false;
            MapManager.Map.GetTile(this.X, this.Y).IsOccupied = true;

            if (this.Step == 0)
            {
                this.previousStep = 0;
                this.Step++;
            }
            else if (this.Step == 2)
            {
                this.previousStep = 2;
                this.Step--;
            }
            else if (this.Step == 1)
            {
                if (this.previousStep == 2)
                    this.Step--;
                else
                    this.Step++;
            }
        }

        public void TryMove()
        {
            if (Globals.KeyDirection != Directions.None)
            {
                this.IsMoving = true;
            }
            else
            {
                this.IsMoving = false;
            }

            if (this.IsMoving && this.CanMove)
            {
                var movementPacket = new MovementPacket();

                this.Direction = Globals.KeyDirection;

                switch (this.Direction)
                {
                    case Directions.Down:

                        // Client side check to see if the tile is blocked.
                        if (this.Y < (MapManager.Map.Height - 1) && !MapManager.Map.GetTile(this.X, this.Y + 1).Blocked && !MapManager.Map.GetTile(this.X, this.Y + 1).IsOccupied)
                        {
                            MapManager.Map.GetTile(this.X, this.Y).IsOccupied = false;
                            this.Y += 1;
                            movementPacket.WriteData(this.X, this.Y, this.Direction);
                            Networking.SendPacket(movementPacket);
                            this.CanMove = false;
                        }

                        break;

                    case Directions.Up:
                        // Client side check to see if the tile is blocked.
                        if (this.Y > 0 && !MapManager.Map.GetTile(this.X, this.Y - 1).Blocked && !MapManager.Map.GetTile(this.X, this.Y - 1).IsOccupied)
                        {
                            MapManager.Map.GetTile(this.X, this.Y).IsOccupied = false;
                            this.Y -= 1;
                            movementPacket.WriteData(this.X, this.Y, this.Direction);
                            Networking.SendPacket(movementPacket);
                            this.CanMove = false;
                        }

                        break;

                    case Directions.Right:
                        // Client side check to see if the tile is blocked.
                        if (this.X < (MapManager.Map.Width - 1) && !MapManager.Map.GetTile(this.X + 1, this.Y).Blocked && !MapManager.Map.GetTile(this.X + 1, this.Y).IsOccupied)
                        {
                            MapManager.Map.GetTile(this.X, this.Y).IsOccupied = false;
                            this.X += 1;
                            movementPacket.WriteData(this.X, this.Y, this.Direction);
                            Networking.SendPacket(movementPacket);
                            this.CanMove = false;
                        }

                        break;

                    case Directions.Left:
                        // Client side check to see if the tile is blocked.
                        if (this.X > 0 && !MapManager.Map.GetTile(this.X - 1, this.Y).Blocked && !MapManager.Map.GetTile(this.X - 1, this.Y).IsOccupied)
                        {
                            MapManager.Map.GetTile(this.X, this.Y).IsOccupied = false;
                            this.X -= 1;
                            movementPacket.WriteData(this.X, this.Y, this.Direction);
                            Networking.SendPacket(movementPacket);
                            this.CanMove = false;
                        }

                        break;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            int x = this.X * 32;
            int y = this.Y * 32;

            if (x == this.PlayerSprite.Position.X && y == this.PlayerSprite.Position.Y)
            {
                this.CanMove = true;

                this.Camera.SnapToTarget();

                this.Camera.Update(this.PlayerSpeed * gameTime.UpdateTime);

                return;
            }

            Vector2f position = this.PlayerSprite.Position;

            if (x < this.PlayerSprite.Position.X)
            {
                position.X = this.PlayerSprite.Position.X - (this.PlayerSpeed * gameTime.UpdateTime);

                if (x >= position.X)
                {
                    this.CanMove = true;
                    position.X = x;
                }
            }
            else if (x > this.PlayerSprite.Position.X)
            {
                position.X = this.PlayerSprite.Position.X + (this.PlayerSpeed * gameTime.UpdateTime);

                if (x <= position.X)
                {
                    this.CanMove = true;
                    position.X = x;
                }
            }

            if (y < this.PlayerSprite.Position.Y)
            {
                position.Y = this.PlayerSprite.Position.Y - (this.PlayerSpeed * gameTime.UpdateTime);

                if (y >= position.Y)
                {
                    this.CanMove = true;
                    position.Y = y;
                }
            }
            else if (y > this.PlayerSprite.Position.Y)
            {
                position.Y = this.PlayerSprite.Position.Y + (this.PlayerSpeed * gameTime.UpdateTime);

                if (y <= position.Y)
                {
                    this.CanMove = true;
                    position.Y = y;
                }
            }

            this.PlayerSprite.Position = position;

            this.Camera.Update(this.PlayerSpeed * gameTime.UpdateTime);
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(_sprite);
        }

        public void DrawInventory(RenderWindow window)
        {
            foreach (var item in _inventory)
                window.Draw(item.Sprite);
        }
    }
}