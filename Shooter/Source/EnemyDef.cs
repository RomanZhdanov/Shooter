using PotatoEngine;

namespace Shooter
{
    class EnemyDef
    {
        public string EnemyType { get; set; }
        public double LaunchTime { get; set; }

        public EnemyDef()
        {
            EnemyType = "cnnon_fodder";
            LaunchTime = 0;
        }

        public EnemyDef(string enemyType, double launchTime)
        {
            EnemyType = enemyType;
            LaunchTime = launchTime;
        }
    }
}
