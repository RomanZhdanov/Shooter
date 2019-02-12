using PotatoEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Source.Weapons
{
    interface IWeapon
    {
        bool Ready(double elapsedTime);
        void Fire(Vector currentPosition, Vector playerPosition);
    }
}
