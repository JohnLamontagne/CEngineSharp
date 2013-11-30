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

        public int ViewWidth { get { return (int)_view.Size.X; } }

        public int ViewHeight { get { return (int)_view.Size.Y; } }

        public int ViewLeft { get; set; }

        public int ViewTop { get; set; }

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
            this.ViewLeft += (int)(center.X - this._view.Center.X);
            this.ViewTop += (int)(center.Y - this._view.Center.Y);
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

            if (x >= (Globals.CurrentResolutionWidth / 2) && x <= (MapManager.Map.Width * 32) - (Globals.CurrentResolutionWidth / 2))
            {
                center.X = x;
            }
            else if (x > ((MapManager.Map.Width * 32) - (Globals.CurrentResolutionWidth / 2)))
            {
                center.X = (MapManager.Map.Width * 32) - (Globals.CurrentResolutionWidth / 2);
            }

            if (y >= (Globals.CurrentResolutionHeight / 2) && y <= (MapManager.Map.Height * 32) - (Globals.CurrentResolutionHeight / 2))
            {
                center.Y = y;
            }
            else if (y > ((MapManager.Map.Height * 32) - (Globals.CurrentResolutionHeight / 2)))
            {
                center.Y = (MapManager.Map.Height * 32) - (Globals.CurrentResolutionHeight / 2);
            }

            this.SetCenter(center);
        }

        public void Update()
        {
            int x = this.Target.X * 32;
            int y = this.Target.Y * 32;

            Vector2f center = this.GetCenter();

            if (x >= (Globals.CurrentResolutionWidth / 2) && x <= (MapManager.Map.Width * 32) - (Globals.CurrentResolutionWidth / 2))
            {
                if (x < _view.Center.X)
                {
                    center.X = _view.Center.X - this.Target.PlayerSpeed;
                }
                else if (x > _view.Center.X)
                {
                    center.X = _view.Center.X + this.Target.PlayerSpeed;
                }
            }

            if (y >= (Globals.CurrentResolutionHeight / 2) && y <= (MapManager.Map.Height * 32) - (Globals.CurrentResolutionHeight / 2))
            {
                if (y < _view.Center.Y)
                {
                    center.Y = _view.Center.Y - this.Target.PlayerSpeed;
                }
                else if (y > _view.Center.Y)
                {
                    center.Y = _view.Center.Y + this.Target.PlayerSpeed;
                }
            }

            this.SetCenter(center);
        }
    }
}