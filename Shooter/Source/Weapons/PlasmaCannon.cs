using PotatoEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Source.Weapons
{
    class PlasmaCannon : IWeapon
    {
        Texture _bulletTexture;
        BulletManager _bulletManager;

        double _shootCountDown;

        public double MaxTimeToShoot { get; set; }
        public double MinTimeToShoot { get; set; }

        Random _random = new Random();

        public PlasmaCannon(TextureManager textureManager, BulletManager bulletManager)
        {
            _bulletTexture = textureManager.Get("bullet");
            _bulletManager = bulletManager;

            MaxTimeToShoot = 12;
            MinTimeToShoot = 1;

            RestartShootCountDown();
        }

        public bool Ready(double elapsedTime)
        {
            _shootCountDown = _shootCountDown - elapsedTime;

            return _shootCountDown <= 0;
        }

        public void Fire(Vector currentPosition, Vector playerPosition)
        {
            Bullet bullet = new Bullet(_bulletTexture);
            bullet.Speed = 350;

            Vector bulletDir = playerPosition - currentPosition;
            bulletDir = bulletDir.Normalize(bulletDir);
            bullet.Direction = bulletDir;
            bullet.SetPosition(currentPosition);
            bullet.SetColor(new Color(0, 1, 1, 1));
            _bulletManager.EnemyShoot(bullet);

            RestartShootCountDown();
        }

        private void RestartShootCountDown()
        {
            _shootCountDown = MinTimeToShoot + (_random.NextDouble() * MaxTimeToShoot);
        }
    }
}
