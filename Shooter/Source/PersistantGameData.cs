using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter
{
    class PersistantGameData
    {
        public bool NewGame { get; set; }
        public bool JustWon { get; set; }
        public int Score { get; set; }
        public int Lives { get; set; }
        public LevelDescription CurrentLevel { get; set; }

        public PersistantGameData()
        {
            JustWon = false;
        }
    }
}
