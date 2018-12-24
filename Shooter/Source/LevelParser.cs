using System;
using System.IO;
using System.Linq;

namespace Shooter
{
    class LevelParser
    {
        static readonly int HeaderSize = 3;

        public static LevelDescription Parse(string filePath)
        {
            LevelDescription _level = new LevelDescription();

            string[] lines = File.ReadAllLines(filePath);

            _level.Name = lines[0];
            _level.Time = double.Parse(lines[1]);
            _level.NextLevel = lines[2];

            for (int i = HeaderSize; i < lines.Length; i++)
            {
                string[] enemiesAndTime = lines[i].Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);

                if (enemiesAndTime.Count() != 0)
                    _level.Enemies.Add(new EnemyDef(enemiesAndTime[0].Trim(), double.Parse(enemiesAndTime[1])));
            }

            return _level;
        }
    }
}
