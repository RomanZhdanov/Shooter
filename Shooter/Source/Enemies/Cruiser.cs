using PotatoEngine;
using Shooter.Source.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Source.Enemies
{
    class Cruiser : Enemy
    {
        static readonly int InitHealth = 100;

        public Cruiser(TextureManager textureManager, EffectsManager effectsManager, BulletManager bulletManager, PlayerCharacter playerCharacter)
            : base(effectsManager, playerCharacter)
        {
            Scale = 0.7;

            _sprite.Texture = textureManager.Get("enemy_heavy");            
            _sprite.SetScale(Scale, Scale);
            _sprite.SetRotation(Math.PI);
            _sprite.SetPosition(200, 0);

            _weapon = new PlasmaCannon(textureManager, bulletManager);

            Health = InitHealth;
            Value = 200;
        }
    }
}
