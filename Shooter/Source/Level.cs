using PotatoEngine;
using PotatoEngine.Input;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using Shooter.Source;

namespace Shooter
{
    class Level
    {
        RectangleF _playArea; 
        Input _input;
        PersistantGameData _gameData;
        PlayerCharacter _playerCharacter;
        TextureManager _textureManager;
        EffectsManager _effectsManager;
        EnemyManager _enemyManager;
        SoundManager _soundManager;
        ScrollingBackground _background;
        ScrollingBackground _backgroundLayer;
        Sprite _planet_28;
        BulletManager _bulletManager;
        Sound _background_music;

        const int _startLives = 2;
        double _endingTime = 0;
        public bool Finished { get; set; }

        public Level(Input input, TextureManager textureManager, PersistantGameData gameData, SoundManager soundManager, PotatoEngine.Font generalFont)
        {
            _input = input;
            _textureManager = textureManager;
            _soundManager = soundManager;
            _gameData = gameData;

            _background = new ScrollingBackground(_textureManager.Get("background"));
            _background.SetScale(2, 2);
            _background.Speed = 0.15f;

            _backgroundLayer = new ScrollingBackground(_textureManager.Get("background_layer_1"));
            _backgroundLayer.SetScale(2, 2);
            _backgroundLayer.Speed = 0.1f;

            _planet_28 = new Sprite();
            _planet_28.Texture = _textureManager.Get("planet_28");
            _planet_28.SetScale(0.5, 0.5);
            _planet_28.SetPosition(300, -300);

            _playArea = new RectangleF(-1260 / 2, -750 / 2, 1260, 750);
            _bulletManager = new BulletManager(_playArea);
            _effectsManager = new EffectsManager(_textureManager);
            _playerCharacter = new PlayerCharacter(_textureManager, _effectsManager, _bulletManager, _playArea);

            if (_gameData.NewGame)
            {
                _playerCharacter.Lives = _startLives;
                _playerCharacter.Score = 0;
            }
            else
            {
                _playerCharacter.Lives = _gameData.Lives;
                _playerCharacter.Score = _gameData.Score;
            }           

            _enemyManager = new EnemyManager(_textureManager, _effectsManager, _bulletManager, _playerCharacter, _playArea, _gameData.CurrentLevel.Enemies,  -1300);

            Finished = false;
        }
        
        public bool HasPlayerDied()
        {
            return _playerCharacter.IsDead;
        }

        public void Update(double elapsedTime, double gameTime)
        {
            _playerCharacter.Update(elapsedTime);
            _effectsManager.Update(elapsedTime);

            _background.Update((float)elapsedTime);
            _backgroundLayer.Update((float)elapsedTime);

            _enemyManager.Update(elapsedTime, gameTime);         

            UpdateCollisions();
            _bulletManager.Update(elapsedTime);
            _gameData.Score = _playerCharacter.Score;
            _gameData.Lives = _playerCharacter.Lives;
            UpdateInput(elapsedTime);

            if (_enemyManager.HasEnemies())
            {
                if (_background_music == null || !_soundManager.IsSoundPlaying(_background_music))
                {
                    _background_music = _soundManager.PlaySound("level_background", true);
                }
            }
            else
            {
                if (_background_music != null && _soundManager.IsSoundPlaying(_background_music))
                {
                    _soundManager.StopSound(_background_music);
                }

                _endingTime -= elapsedTime;

                if (_endingTime < 0)
                    Finished = true;
            }


            if (_playerCharacter.IsDead)
            {
                if (_background_music != null && _soundManager.IsSoundPlaying(_background_music))
                {
                    _soundManager.StopSound(_background_music);
                }
            }
        }

        private void UpdateInput(double elapsedTime)
        {
            if (_input.Keyboard.IsKeyPressed(Keys.Space))
            {
                _playerCharacter.Fire();
            }

            Vector controlInput = new Vector();

            if (_input.Keyboard.IsKeyHeld(Keys.Left))
            {
                controlInput.X = -1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.Right))
            {
                controlInput.X = 1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.Up))
            {
                controlInput.Y = 1;
            }

            if (_input.Keyboard.IsKeyHeld(Keys.Down))
            {
                controlInput.Y = -1;
            }

            // Get controls and apply to player character
            if (_input.Controller != null)
            {
                double x = _input.Controller.LeftControlStick.X;
                double y = _input.Controller.LeftControlStick.Y * -1;
                controlInput = new Vector(x, y, 0);

                if (_input.Controller.ButtonA.Pressed)
                {
                    _playerCharacter.Fire();
                }
            }

            _playerCharacter.Move(controlInput * elapsedTime);
        }

        public void Render(Renderer renderer)
        {
            _background.Render(renderer);
            //renderer.DrawSprite(_planet_28);
            _backgroundLayer.Render(renderer);
            _enemyManager.Render(renderer);
            _playerCharacter.Render(renderer);
            _bulletManager.Render(renderer);
            _effectsManager.Render(renderer);
        }

        private void UpdateCollisions()
        {
            _bulletManager.UpdatePlayerCollision(_playerCharacter);

            foreach (Enemy enemy in _enemyManager.EnemyList)
            {
                if (enemy.GetBoundingBox().IntersectsWith(_playerCharacter.GetBoundingBox()))
                {
                    enemy.OnCollision(_playerCharacter);
                    _playerCharacter.OnCollision(enemy);
                }
                
                _bulletManager.UpdateEnemyCollisions(enemy);
            }
        }
    }
}
