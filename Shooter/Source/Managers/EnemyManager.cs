using PotatoEngine;
using Shooter.Source;
using Shooter.Source.Enemies;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shooter
{
    class EnemyManager
    {
        List<EnemyDef> _upComingEnemies = new List<EnemyDef>();
        List<Enemy> _enemies = new List<Enemy>();
        TextureManager _textureManager;
        EffectsManager _effectsManager;
        BulletManager _bulletManager;
        PlayerCharacter _playerCharacter;
        RectangleF _bounds;

        int _leftBound;

        public List<Enemy> EnemyList
        {
            get { return _enemies; }
        }

        public EnemyManager(TextureManager textureManager, EffectsManager effectsManager, BulletManager bulletManager, PlayerCharacter playerCharacter, RectangleF playArea, List<EnemyDef> upComingEnemies, int leftBound)
        {
            _textureManager = textureManager;
            _effectsManager = effectsManager;
            _bulletManager = bulletManager;
            _playerCharacter = playerCharacter;
            _leftBound = leftBound;
            _bounds = playArea;

            _upComingEnemies = upComingEnemies;      

            _upComingEnemies.Sort(delegate (EnemyDef firstEnemy, EnemyDef secondEnemy)
            {
                return firstEnemy.LaunchTime.CompareTo(secondEnemy.LaunchTime);
            });

            //Enemy enemy = new Enemy(_textureManager, _effectsManager);
            //_enemies.Add(enemy);
        }

        public bool HasEnemies()
        {
            return _enemies.Count > 0 || _upComingEnemies.Count > 0;
        }

        public void Update(double elapsedTime, double gameTime)
        {
            UpdateEnemySpawns(gameTime);
            _enemies.ForEach(x => x.Update(elapsedTime));
            CheckForOutOfBounds();
            RemoveDeadEnemies();
        }

        private void CheckForOutOfBounds()
        {
            foreach(Enemy enemy in _enemies)
            {
                if (enemy.GetBoundingBox().Right <  _leftBound)
                {
                    enemy.Health = 0;
                }
            }
        }

        private void RemoveDeadEnemies()
        {
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                if (_enemies[i].IsDead)
                {
                    _enemies.RemoveAt(i);
                }
            }
        }

        private void UpdateEnemySpawns(double gameTime)
        {
            if (_upComingEnemies.Count == 0)
            {
                return;
            }

            EnemyDef lastElement = _upComingEnemies[0];

            if (gameTime > lastElement.LaunchTime)
            {
                _upComingEnemies.RemoveAt(0);
                _enemies.Add(CreateEnemyFromDef(lastElement));
            }
        }

        private Enemy CreateEnemyFromDef(EnemyDef definition)
        {
            Enemy enemy = null;            

            if (definition.EnemyType == "cannon_fodder")
            {
                enemy = new CannonFodder(_textureManager, _effectsManager, _bulletManager, _playerCharacter);

                List<Vector> _pathPoints = new List<Vector>();
                _pathPoints.Add(new Vector(1400, 0, 0));
                _pathPoints.Add(new Vector(0, 250, 0));
                _pathPoints.Add(new Vector(-1400, 0, 0));

                enemy.Path = new Path(_pathPoints, 10);
            }
            else if (definition.EnemyType == "cannon_fodder_low")
            {
                enemy = new CannonFodder(_textureManager, _effectsManager, _bulletManager, _playerCharacter);

                List<Vector> _pathPoints = new List<Vector>();
                _pathPoints.Add(new Vector(1400, 0, 0));
                _pathPoints.Add(new Vector(0, -250, 0));
                _pathPoints.Add(new Vector(-1400, 0, 0));

                enemy.Path = new Path(_pathPoints, 10);
            }
            else if (definition.EnemyType == "cannon_fodder_straight")
            {
                enemy = new Cruiser(_textureManager, _effectsManager, _bulletManager, _playerCharacter);

                List<Vector> _pathPoints = new List<Vector>(); 
                _pathPoints.Add(new Vector(1400, 0, 0));
                _pathPoints.Add(new Vector(-1400, 0, 0));
                enemy.Path = new Path(_pathPoints, 16);
            }
            else if (definition.EnemyType == "asteroid")
            {
                enemy = new Asteroid(_textureManager, _effectsManager, _playerCharacter);

                Random random = new Random();
                double y = random.Next((int)_bounds.Top, (int)_bounds.Bottom);

                List<Vector> _pathPoints = new List<Vector>();
                _pathPoints.Add(new Vector(1400, y, 0));
                _pathPoints.Add(new Vector(-1400, y, 0));
                                
                enemy.Path = new Path(_pathPoints, 20);
            }
            else if (definition.EnemyType == "up_1")
            {
                enemy = new CannonFodder(_textureManager, _effectsManager, _bulletManager, _playerCharacter);

                List<Vector> _pathPoints = new List<Vector>();
                _pathPoints.Add(new Vector(500, -375, 0));
                _pathPoints.Add(new Vector(500, 0, 0));
                _pathPoints.Add(new Vector(500, 0, 0));
                _pathPoints.Add(new Vector(-1400, 200, 0));

                enemy.Path = new Path(_pathPoints, 10);
            }
            else if (definition.EnemyType == "down_1")
            {
                enemy = new CannonFodder(_textureManager, _effectsManager, _bulletManager, _playerCharacter);

                List<Vector> _pathPoints = new List<Vector>();
                _pathPoints.Add(new Vector(500, 375, 0));
                _pathPoints.Add(new Vector(500, 0, 0));
                _pathPoints.Add(new Vector(500, 0, 0));
                _pathPoints.Add(new Vector(-1400, -200, 0));

                enemy.Path = new Path(_pathPoints, 10);
            }
            else if (definition.EnemyType == "boss")
            {
                enemy = new Boss(_textureManager, _effectsManager, _bulletManager, _playerCharacter);

                List<Vector> _pathPoints = new List<Vector>();
                _pathPoints.Add(new Vector(1400, 0, 0));
                _pathPoints.Add(new Vector(300, 0, 0));

                enemy.Path = new Path(_pathPoints, 10);              
            }
            else
            {
                System.Diagnostics.Debug.Assert(false, "Uknown enemy type.");
            }

            return enemy;
        }

        public void Render(Renderer renderer)
        {
            _enemies.ForEach(x => x.Render(renderer));
        }
    }
}
