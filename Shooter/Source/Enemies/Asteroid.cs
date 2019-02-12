using PotatoEngine;
using Shooter.Source.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Source.Enemies
{
    class Asteroid : Enemy
    {
        static readonly int InitHealth = 100;

        public Asteroid(TextureManager textureManager, EffectsManager effectsManager, PlayerCharacter playerCharacter)
            : base(effectsManager, playerCharacter)
        {
            Scale = 0.15;
            Texture = textureManager.Get("asteroid");
            SetAnimation(8, 8, 0.08, true);;
            Health = InitHealth;
            Value = 50;

            _weapon = new Unarmed();
        }
    }
}
