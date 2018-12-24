using PotatoEngine;
using System;

namespace Shooter
{
    class Enemy : Entity
    {
        static readonly double HitFlashTime = 0.25;
        static readonly int InitHealth = 50;

        double _hitFlashCountDown = 0;
        double _shootCountDown;

        PlayerCharacter _playerCharacter;
        EffectsManager _effectsManager;
        BulletManager _bulletManager;
        Texture _bulletTexture;

        public int Health { get; set; }
        public int Value { get; set; }
        public Path Path { get; set; }
        public double MaxTimeToShoot { get; set; }
        public double MinTimeToShoot { get; set; }

        Random _random = new Random();

        public Enemy(TextureManager textureManager, EffectsManager effectsManager, BulletManager bulletManager, PlayerCharacter playerCharacter)
        {
            _effectsManager = effectsManager;
            _bulletManager = bulletManager;
            _bulletTexture = textureManager.Get("bullet");
            _playerCharacter = playerCharacter;
            MaxTimeToShoot = 12;
            MinTimeToShoot = 1;
            RestartShootCountDown();
            _sprite.Texture = textureManager.Get("enemy_ship");
            _sprite.SetScale(Scale, Scale);
            _sprite.SetRotation(Math.PI);
            _sprite.SetPosition(200, 0);
            
            Health = InitHealth;
        }

        public bool IsDead
        {
            get { return Health == 0; }
        }

        public void SetPosition(Vector position)
        {
            _sprite.SetPosition(position);
        }

        public void RestartShootCountDown()
        {
            _shootCountDown = MinTimeToShoot + (_random.NextDouble() * MaxTimeToShoot);
        }

        public void Update(double elapsedTime)
        {
            _shootCountDown = _shootCountDown - elapsedTime;

            if(_shootCountDown <= 0)
            {
                Bullet bullet = new Bullet(_bulletTexture);
                bullet.Speed = 350;

                Vector currentPosition = _sprite.GetPosition();
                Vector bulletDir = _playerCharacter.GetPosition() - currentPosition;
                bulletDir = bulletDir.Normalize(bulletDir);
                bullet.Direction = bulletDir;
                bullet.SetPosition(_sprite.GetPosition());
                bullet.SetColor(new Color(0, 1, 1, 1));
                _bulletManager.EnemyShoot(bullet);
                RestartShootCountDown();
            }

            if (Path != null)
            {
                Path.UpdatePosition(elapsedTime, this);
            }

            if (_hitFlashCountDown != 0)
            {
                _hitFlashCountDown = Math.Max(0, _hitFlashCountDown - elapsedTime);
                double scaledTime = 1 - (_hitFlashCountDown / HitFlashTime);
                _sprite.SetColor(new PotatoEngine.Color(1, 0, (float)scaledTime, 1));
            }
            else if (Health < InitHealth)
            {
                _sprite.SetColor(new PotatoEngine.Color(1, 1, 0, 1));
            }

            _sprite.Update(elapsedTime);
            _sprite.SetScale(Scale, Scale);
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(_sprite);
            Render_Debug();
        }

        internal void OnCollision(PlayerCharacter player)
        {

        }

        internal void OnCollision(Bullet bullet)
        {
            // If the ship is already dead then ignore any more bullet
            if (Health == 0)
            {
                return;
            }

            Health = Math.Max(0, Health - 25);
            _hitFlashCountDown = HitFlashTime;
            _sprite.SetColor(new PotatoEngine.Color(1, 1, 0, 1));

            if (Health == 0)
            {
                OnDestroyed();
            }
        }

        private void OnDestroyed()
        {
            _playerCharacter.Score += Value;
            _effectsManager.AddExplosion(_sprite.GetPosition());
        }
    }
}
