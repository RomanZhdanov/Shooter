using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotatoEngine;

namespace Shooter.Source.Weapons
{
    class Unarmed : IWeapon
    {
        public void Fire(Vector currentPosition, Vector playerPosition)
        {
            // Can't shoot
        }

        public bool Ready(double elapsedTime)
        {
            return false;
        }
    }
}
