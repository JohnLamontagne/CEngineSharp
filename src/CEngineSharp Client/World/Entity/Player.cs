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

        private bool SpriteCenteredX { get; set; }

        private bool SpriteCenteredY { get; set; }

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
            this.PlayerSpeed = 2f;
            this.Camera = new Camera(this);
        }

        public int InvenCordToSlot(int invenX, int invenY)
        {
            GameRenderer gameRenderer = (RenderManager.CurrentRenderer as GameRenderer);

            int invenPosX = (int)gameRenderer.Gui.Get<Picture>("picInventory").Position.X;
            int invenPosY = (int)gameRenderer.Gui.Get<Picture>("picInventory").Position.Y;

            int x = (invenPosX - invenX) / 32;
            int y = (invenPosY - invenY) / 32;

            // slotnum = (x + (y * 5))
            // slotnum = x + (y * 5)
            //

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
            invenX *= 32;
            invenY *= 32;

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
            Item item = this.GetInventoryItem(invenX, invenY);

            if (item != null)
            {
                DropItemPacket dropItemPacket = new DropItemPacket();
                dropItemPacket.WriteData(item);
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

            int prevX = this.X * 32;
            int prevY = this.Y * 32;

            if (this.X == newX && this.Y == newY)
            {
                this.Step = 0;
            }
            else
            {
                this.X = newX;
                this.Y = newY;

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
                        movementPacket.WriteData(GameWorld.GetPlayer(Globals.MyIndex).X, GameWorld.GetPlayer(Globals.MyIndex).Y + 1, this.Direction);
                        break;

                    case Directions.Up:
                        movementPacket.WriteData(GameWorld.GetPlayer(Globals.MyIndex).X, GameWorld.GetPlayer(Globals.MyIndex).Y - 1, this.Direction);
                        break;

                    case Directions.Right:
                        movementPacket.WriteData(GameWorld.GetPlayer(Globals.MyIndex).X + 1, GameWorld.GetPlayer(Globals.MyIndex).Y, this.Direction);
                        break;

                    case Directions.Left:
                        movementPacket.WriteData(GameWorld.GetPlayer(Globals.MyIndex).X - 1, GameWorld.GetPlayer(Globals.MyIndex).Y, this.Direction);
                        break;
                }

                Networking.SendPacket(movementPacket);
                this.CanMove = false;
            }
        }

        public void Update()
        {
            int x = this.X * 32;
            int y = this.Y * 32;
            Vector2f position = this.PlayerSprite.Position;

            this.Camera.Update();

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