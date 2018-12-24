using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter
{
    class LevelDescription
    {
        public string LevelId { get; set; }

        public string Name { get; set; }
        
        // Time a level lasts in seconds
        public double Time { get; set; }

        public string NextLevel { get; set; }

        public List<EnemyDef> Enemies { get; set; }

        public LevelDescription()
        {
            Enemies = new List<EnemyDef>();
        }
    }
}
