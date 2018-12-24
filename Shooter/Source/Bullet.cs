using PotatoEngine;
using System;

namespace Shooter
{
    class Bullet : Entity
    {
        public bool Dead { get; set; }
        public Vector Direction { get; set; }
        public double Speed { get; set; }

        public double X
        {
            get { return _sprite.GetPosition().X; }
        }

        public double Y
        {
            get { return _sprite.GetPosition().Y; }
        }

        public void SetPosition(Vector position)
        {
            _sprite.SetPosition(position);
        }

        public void SetColor(Color color)
        {
            _sprite.SetColor(color);
        }

        public Bullet(Texture texture)
        {
            _sprite.Texture = texture;

            Dead = false;
            Direction = new Vector(1, 0, 0);
            Speed = 512;
        }

        public void Render(Renderer renderer)
        {
            if (Dead)
            {
                return;
            }
            renderer.DrawSprite(_sprite);
            //Render_Debug();
        }

        public void Update(double elapsedTime)
        {
            if (Dead)
            {
                return;
            }

            Vector position = _sprite.GetPosition();
            position += Direction * Speed * elapsedTime;
            _sprite.SetPosition(position);
        }
    }
}
