using PotatoEngine;
using System;
using System.Drawing;

namespace Shooter
{
    class PlayerCharacter : Entity
    {

        BulletManager _bulletManager;
        EffectsManager _effectsManager;
        Texture _bulletTexture;
        RectangleF _bounds;
        Vector _gunOffset = new Vector(55, 0, 0);

        double _speed = 600; // pixels per second

        static readonly double FireRecovery = 0.17;
        static readonly double Invulnarable = 2;
        double _fireRecoveryTime = FireRecovery;
        double _invulnerabilityTime = 0;
        private bool _dead = false;

        public bool IsDead
        {
            get { return _dead; }
        }

        public int Score { get; set; }
        public int Lives { get; set; }

        public PlayerCharacter(TextureManager textureManager, EffectsManager effectsManager, BulletManager bulletManager, RectangleF playArea)
        {
            _sprite.Texture = textureManager.Get("player_ship");
            _sprite.SetScale(0.6, 0.6);
            _effectsManager = effectsManager;
            _bulletManager = bulletManager;
            _bulletTexture = textureManager.Get("bullet");
            _bounds = playArea;
        }

        public void Move(Vector amount)
        {
            amount *= _speed;
            Vector newPosition = _sprite.GetPosition() + amount;
            RectangleF futurePosition = GetBoundingBox(newPosition);
            float width = (float)(_sprite.Texture.Width * _sprite.ScaleX);
            float height = (float)(_sprite.Texture.Height * _sprite.ScaleY);

            if (futurePosition.Right > _bounds.Right)
            {
                newPosition.X = (double)_bounds.Right - width / 2;
            }
            if (futurePosition.Left < _bounds.Left)
            {
                newPosition.X = (double)_bounds.Left + width / 2;
            }
            if (futurePosition.Top < _bounds.Top)
            {
                newPosition.Y = (double)_bounds.Top + height;
            }
            if (futurePosition.Bottom > _bounds.Bottom)
            {
                newPosition.Y = (double)_bounds.Bottom - height;
            }

            _sprite.SetPosition(newPosition);
        }

        public RectangleF GetBoundingBox(Vector position)
        {
            float width = (float)(_sprite.Texture.Width * _sprite.ScaleX);
            float height = (float)(_sprite.Texture.Height * _sprite.ScaleY);

            return new RectangleF((float)_sprite.GetPosition().X - width / 2,
                (float)_sprite.GetPosition().Y - height / 2,
                width, height);
        }

        public void Fire()
        {
            if (_fireRecoveryTime > 0)
            {
                return;
            }
            else
            {
                _fireRecoveryTime = FireRecovery;
            }

            Bullet bullet = new Bullet(_bulletTexture);
            bullet.SetColor(new PotatoEngine.Color(0, 1, 0, 1));
            bullet.SetPosition(_sprite.GetPosition() + _gunOffset);
            _bulletManager.Shoot(bullet);
        }

        public void Update(double elapsedTime)
        {
            _fireRecoveryTime = Math.Max(0, (_fireRecoveryTime - elapsedTime));
            _invulnerabilityTime = Math.Max(0, (_invulnerabilityTime - elapsedTime));

            if(_invulnerabilityTime > 0)
            {
                _sprite.SetColor(new PotatoEngine.Color(0, 1, 0, (float)0.5));
            }
            else
            {
                _sprite.SetColor(new PotatoEngine.Color(1, 1, 1, 1));
            }
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(_sprite);
            //Render_Debug();
        }

        public Vector GetPosition()
        {
            return _sprite.GetPosition();
        }

        internal void OnCollision(Enemy enemy)
        {
            if (_invulnerabilityTime == 0)
            {
                Lives--;
                OnDestroyed();
                if (Lives < 0)
                {
                    _dead = true;
                }
            }
        }

        internal void OnCollision(Bullet bullet)
        {
            if (_invulnerabilityTime == 0)
            {
                Lives--;
                OnDestroyed();
                if (Lives < 0)
                {
                    _dead = true;
                }
            }
        }

        private void OnDestroyed()
        {
            _effectsManager.AddExplosion(_sprite.GetPosition());
            _sprite.SetPosition(new Vector(0, 0, 0));
            _invulnerabilityTime = Invulnarable;
        }
    }
}
