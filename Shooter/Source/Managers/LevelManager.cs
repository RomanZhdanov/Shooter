using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter
{
    class LevelManager
    {
        Dictionary<string, string> _levelsDatabase = new Dictionary<string, string>();
               
        public LevelManager()
        {

        }

        public LevelDescription Load(string levelId)
        {
            return LevelParser.Parse(_levelsDatabase[levelId]);
        }

        public void AddLevel(string levelId, string path)
        {
            _levelsDatabase.Add(levelId, path);
        }
    }
}
