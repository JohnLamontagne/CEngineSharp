using CEngineSharp_Client.Utilities;
using CEngineSharp_Client.World.Entity;
using SFML.Graphics;
using SFML.System;

namespace CEngineSharp_Client.Graphics
{
    public class Camera
    {
        private Vector2f _center;

        public FloatRect ViewRect { get; private set; }

        public IEntity Target { get; set; }

        public View View { get; private set; }

        public Vector2f StationaryBounds { get; set; }

        public bool CanViewOutside { get; set; }

        public Vector2f Center
        {
            get { return _center; }
            set
            {
                _center = value;

                this.ViewRect = new FloatRect()
                {
                    Top = _center.Y - (ServiceLocator.ScreenManager.Resolution.Y / 2),
                    Left = _center.X - (ServiceLocator.ScreenManager.Resolution.X / 2),
                    Width = ServiceLocator.ScreenManager.Resolution.X,
                    Height = ServiceLocator.ScreenManager.Resolution.Y
                };

                this.View = new View(_center, new Vector2f(this.ViewRect.Width, this.ViewRect.Height));
            }
        }

        public Camera(IEntity target)
        {
            _center = new Vector2f(ServiceLocator.ScreenManager.Resolution.X / 2, ServiceLocator.ScreenManager.Resolution.Y / 2);
            this.Target = target;
            this.StationaryBounds = new Vector2f(ServiceLocator.ScreenManager.Resolution.X / 2, ServiceLocator.ScreenManager.Resolution.Y / 2);
            this.CanViewOutside = false;
        }

        public void SnapToTarget()
        {
            this.Center = new Vector2f(this.Target.Position.X, this.Target.Position.Y);
        }

        public void Update(float playerSpeed, GameTime gameTime)
        {
            float delta = playerSpeed * gameTime.UpdateTime;

            var centerX = this.Center.X;
            var centerY = this.Center.Y;

            var targetX = this.Target.Position.X * 32;
            var targetY = this.Target.Position.Y * 32;

            if (targetX >= this.StationaryBounds.X)
            {
                var mapBoundsX = (ServiceLocator.WorldManager.MapManager.Map.Width * 32) - (this.ViewRect.Width / 2);

                if (CanViewOutside || targetX <= mapBoundsX)
                {
                    if (this.Center.X < targetX)
                    {
                        centerX += delta;

                        if (centerX > targetX)
                            centerX = targetX;
                    }

                    if (this.Center.X > targetX)
                    {
                        centerX -= delta;

                        if (centerX < targetX)
                            centerX = targetX;
                    }
                }
            }

            if (targetY >= this.StationaryBounds.Y)
            {
                var mapBoundsY = (ServiceLocator.WorldManager.MapManager.Map.Height * 32) - (this.ViewRect.Height / 2);

                if (CanViewOutside || targetY <= mapBoundsY)
                {
                    if (this.Center.Y < targetY)
                    {
                        centerY += delta;

                        if (centerY > targetY)
                            centerY = targetY;
                    }

                    if (this.Center.Y > targetY)
                    {
                        centerY -= delta;

                        if (centerY < targetY)
                            centerY = targetY;
                    }
                }
            }

            this.Center = new Vector2f(centerX, centerY);
        }
    }
}