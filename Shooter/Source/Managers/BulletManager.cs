using PotatoEngine;
using Shooter.Source;
using System.Collections.Generic;
using System.Drawing;

namespace Shooter
{
    class BulletManager
    {
        List<Bullet> _bullets = new List<Bullet>();
        List<Bullet> _enemyBullets = new List<Bullet>();
        RectangleF _bounds;

        public BulletManager(RectangleF playArea)
        {
            _bounds = playArea;
        }

        public void Shoot(Bullet bullet)
        {
            _bullets.Add(bullet);
        }

        public void EnemyShoot(Bullet bullet)
        {
            _enemyBullets.Add(bullet);
        }

        public void Update(double elapsedTime)
        {
            UpdateBulletList(_bullets, elapsedTime);
            UpdateBulletList(_enemyBullets, elapsedTime);
        }

        private void UpdateBulletList(List<Bullet> buletList, double elapsedTime)
        {
            buletList.ForEach(x => x.Update(elapsedTime));
            CheckOutOfBounds(buletList);
            RemoveDeadBullets(buletList);
        }

        private void CheckOutOfBounds(List<Bullet> buletList)
        {
            foreach(Bullet bullet in buletList)
            {
                if(!bullet.GetBoundingBox().IntersectsWith(_bounds))
                {
                    bullet.Dead = true;
                }
            }
        }

        private void RemoveDeadBullets(List<Bullet> bulletList)
        {
            for (int i = bulletList.Count - 1; i >= 0; i--)
            {
                if (bulletList[i].Dead)
                {
                    bulletList.RemoveAt(i);
                }
            }
        }

        internal void Render(Renderer renderer)
        {
            _bullets.ForEach(x => x.Render(renderer));
            _enemyBullets.ForEach(x => x.Render(renderer));
        }

        internal void UpdatePlayerCollision(PlayerCharacter playerCharacter)
        {
            foreach(Bullet bullet in _enemyBullets)
            {
                if (bullet.GetBoundingBox().IntersectsWith(playerCharacter.GetBoundingBox()))
                {
                    bullet.Dead = true;
                    playerCharacter.OnCollision(bullet);
                }
            }
        }

        internal void UpdateEnemyCollisions(Enemy enemy)
        {
            foreach (Bullet bullet in _bullets)
            {
                if (bullet.GetBoundingBox().IntersectsWith(enemy.GetBoundingBox()))
                {
                    bullet.Dead = true;
                    enemy.OnCollision(bullet);
                }
            }
        }
    }
}
