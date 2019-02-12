using PotatoEngine;
using Shooter.Source.Weapons;
using System;

namespace Shooter.Source
{
    abstract class Enemy : Entity
    {
        static readonly double HitFlashTime = 0.25;

        double _hitFlashCountDown = 0;

        PlayerCharacter _playerCharacter;
        EffectsManager _effectsManager;

        public IWeapon _weapon;

        public int Health { get; set; }
        public int Value { get; set; }
        public Path Path { get; set; }

        public Enemy(EffectsManager effectsManager, PlayerCharacter playerCharacter)
        {
            _effectsManager = effectsManager;
            _playerCharacter = playerCharacter;
        }

        public bool IsDead
        {
            get { return Health == 0; }
        }

        public void SetPosition(Vector position)
        {
            _sprite.SetPosition(position);
        }
                
        public void Update(double elapsedTime)
        {
            if(_weapon.Ready(elapsedTime))
            {
                _weapon.Fire(_sprite.GetPosition(), _playerCharacter.GetPosition());                
            }

            if (Path != null)
            {
                Path.UpdatePosition(elapsedTime, this);
            }

            // Set hit color
            //if (_hitFlashCountDown != 0)
            //{
            //    _hitFlashCountDown = Math.Max(0, _hitFlashCountDown - elapsedTime);
            //    double scaledTime = 1 - (_hitFlashCountDown / HitFlashTime);
            //    _sprite.SetColor(new PotatoEngine.Color(1, 0, (float)scaledTime, 1));
            //}
            //else if (Health < InitHealth)
            //{
            //    _sprite.SetColor(new PotatoEngine.Color(1, 1, 0, 1));
            //}

            _sprite.Update(elapsedTime);
            _sprite.SetScale(Scale, Scale);
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawSprite(_sprite);
            RenderHitbox_Debug();
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
