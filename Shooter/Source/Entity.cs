using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotatoEngine;
using Tao.OpenGl;
using System.Drawing;

namespace Shooter
{
    class Entity
    {
        protected AnimatedSprite _sprite = new AnimatedSprite();
        double _scale = 0.3;

        public Texture Texture
        {
            get { return _sprite.Texture; }
            set { _sprite.Texture = value; }
        }

        public void SetAnimation(int framesX, int framesY, double speed, bool looping = false)
        {
            _sprite.SetAnimation(framesX, framesY);
            _sprite.Speed = speed;
            _sprite.Looping = looping;
        }

        public double Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public RectangleF GetBoundingBox()
        {
            float width = (float)(_sprite.Texture.Width * _sprite.ScaleX);
            float height = (float)(_sprite.Texture.Height * _sprite.ScaleY);

            return new RectangleF((float)_sprite.GetPosition().X - width / 2,
                (float)_sprite.GetPosition().Y - height / 2,
                width, height);
        }

        // Render a bounding box
        public void Render_Debug()
        {
            Gl.glDisable(Gl.GL_TEXTURE_2D);

            RectangleF bounds = GetBoundingBox();
            Gl.glBegin(Gl.GL_LINE_LOOP);
            {
                Gl.glColor3f(1, 0, 0);
                Gl.glVertex2f(bounds.Left, bounds.Top);
                Gl.glVertex2f(bounds.Right, bounds.Top);
                Gl.glVertex2f(bounds.Right, bounds.Bottom);
                Gl.glVertex2f(bounds.Left, bounds.Bottom);
            }
            Gl.glEnd();
            Gl.glEnable(Gl.GL_TEXTURE_2D);
        }

    }
}
