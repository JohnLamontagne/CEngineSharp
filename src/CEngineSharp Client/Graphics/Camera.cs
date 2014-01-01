using CEngineSharp_Client.World.Content_Managers;
using CEngineSharp_Client.World.Entity;
using SFML.Graphics;
using SFML.Window;
using System;

namespace CEngineSharp_Client.Graphics
{
    public class Camera
    {
        private View _view;
        private Player _target;

        public Player Target { get; set; }

        public float ViewWidth { get { return _view.Size.X; } }

        public float ViewHeight { get { return _view.Size.Y; } }

        public float ViewLeft { get; set; }

        public float ViewTop { get; set; }

        public Camera(Player target)
        {
            this._view = new View((RenderManager.CurrentRenderer as GameRenderer).GetWindow().DefaultView);
            this.Target = target;
        }

        public void SetTarget(Player target)
        {
            _target = target;
        }

        public Player GetTarget()
        {
            return _target;
        }

        public void SetCenter(Vector2f center)
        {
            this.ViewLeft += center.X - this._view.Center.X;
            this.ViewTop += center.Y - this._view.Center.Y;
            this._view.Center = center;
        }

        public Vector2f GetCenter()
        {
            return this._view.Center;
        }

        public View GetView()
        {
            return _view;
        }

        public void SetView(View view)
        {
            _view = view;
        }

        public void Zoom(float factor)
        {
            _view.Zoom(factor);
        }

        public void SnapToTarget()
        {
            int x = this.Target.X * 32;
            int y = this.Target.Y * 32;

            Vector2f center = this.GetCenter();

            if (x >= (RenderManager.CurrentResolutionWidth / 2) && x <= (MapManager.Map.Width * 32) - (RenderManager.CurrentResolutionWidth / 2))
            {
                center.X = x;
            }
            else if (x > ((MapManager.Map.Width * 32) - (RenderManager.CurrentResolutionWidth / 2)))
            {
                center.X = (MapManager.Map.Width * 32) - (RenderManager.CurrentResolutionWidth / 2);
            }

            if (y >= (RenderManager.CurrentResolutionHeight / 2) && y <= (MapManager.Map.Height * 32) - (RenderManager.CurrentResolutionHeight / 2))
            {
                center.Y = y;
            }
            else if (y > ((MapManager.Map.Height * 32) - (RenderManager.CurrentResolutionHeight / 2)))
            {
                center.Y = (MapManager.Map.Height * 32) - (RenderManager.CurrentResolutionHeight / 2);
            }

            this.SetCenter(center);
        }

        public void Update(float playerSpeed)
        {
            this.SetCenter(this.Target.PlayerSprite.Position);

            float x = this.Target.PlayerSprite.Position.X;
            float y = this.Target.PlayerSprite.Position.Y;

            Vector2f center = this.GetCenter();

            if (x >= (RenderManager.CurrentResolutionWidth / 2) && x <= (MapManager.Map.Width * 32) - (RenderManager.CurrentResolutionWidth / 2))
            {
                if (x < _view.Center.X)
                {
                    center.X = _view.Center.X - playerSpeed;
                }
                else if (x > _view.Center.X)
                {
                    center.X = _view.Center.X + playerSpeed;
                }
            }

            if (y >= (RenderManager.CurrentResolutionHeight / 2) && y <= (MapManager.Map.Height * 32) - (RenderManager.CurrentResolutionHeight / 2))
            {
                if (y < _view.Center.Y)
                {
                    center.Y = _view.Center.Y - playerSpeed;
                }
                else if (y > _view.Center.Y)
                {
                    center.Y = _view.Center.Y + playerSpeed;
                }
            }

            if (center.X < (RenderManager.CurrentResolutionWidth / 2))
                center.X = RenderManager.CurrentResolutionWidth / 2;

            if (x >= (MapManager.Map.Width * 32) - (RenderManager.CurrentResolutionWidth / 2))
                center.X = (MapManager.Map.Width * 32) - (RenderManager.CurrentResolutionWidth / 2);

            if (center.Y < (RenderManager.CurrentResolutionHeight / 2))
                center.Y = RenderManager.CurrentResolutionHeight / 2;

            if (y >= (MapManager.Map.Height * 32) - (RenderManager.CurrentResolutionHeight / 2))
                center.Y = (MapManager.Map.Height * 32) - (RenderManager.CurrentResolutionHeight / 2);

            this.SetCenter(center);
        }
    }
}