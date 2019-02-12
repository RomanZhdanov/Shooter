using PotatoEngine;
using Shooter.Source.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Source.Enemies
{
    class CannonFodder : Enemy
    {
        static readonly int InitHealth = 50;

        public CannonFodder(TextureManager textureManager, EffectsManager effectsManager, BulletManager bulletManager, PlayerCharacter playerCharacter)
            : base(effectsManager, playerCharacter)
        {
            _sprite.Texture = textureManager.Get("enemy_ship");
            _sprite.SetRotation(Math.PI);
            _sprite.SetPosition(200, 0);

            _weapon = new PlasmaCannon(textureManager, bulletManager);

            Health = InitHealth;
            Value = 100;
        }
    }
}
