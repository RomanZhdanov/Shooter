using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotatoEngine;
using PotatoEngine.Input;
using Tao.OpenGl;

namespace Shooter
{
    class InnerGameState : IGameObject
    {
        Font                _generalFont;
        Font                _titleFont;
        Input               _input;
        Renderer            _renderer = new Renderer();
        StateSystem         _system;
        PersistantGameData  _gameData;
        TextureManager      _textureManager;
        SoundManager        _soundManager;
        LevelManager        _levelManager;
        Level               _level;
        Text                _levelTitle;
        GUI                 _score;
        GUI                 _lives;

        const double _waitTime = 2;

        double _gameTime;
        double _lvlTitleTime;


        public InnerGameState(Input input, StateSystem system, TextureManager textureManager, SoundManager soundManager, LevelManager levelManager, PersistantGameData gameData, Font generalFont, Font titleFont)
        {
            _input = input;
            _system = system;
            _gameData = gameData;
            _titleFont = titleFont;
            _generalFont = generalFont;
            _soundManager = soundManager;
            _levelManager = levelManager;
            _textureManager = textureManager;
            _score = new GUI("Score", generalFont, new Vector(400, 350, 0));
            _lives = new GUI("Lives", generalFont, new Vector(-600, 350, 0));

            OnGameStart();
        }

        private void OnGameStart()
        {
            _level = new Level(_input, _textureManager, _gameData, _soundManager, _generalFont);
            _gameTime = _gameData.CurrentLevel.Time;
            _lvlTitleTime = _waitTime;
            _levelTitle = new Text(_gameData.CurrentLevel.Name, _titleFont);
            _levelTitle.SetColor(new Color(1, 1, 1, 1));
            // Center on the x and place somewhere near the top
            _levelTitle.SetPosition(-_levelTitle.Width / 2, 0);
        }

        public void Render()
        {
            Gl.glClearColor(0, 0, 0, 0);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            if (_lvlTitleTime >= 0)
            {
                _renderer.DrawText(_levelTitle);
            }
            else
            {                
                _level.Render(_renderer);
                _score.Render(_renderer);
                _lives.Render(_renderer);
            }

            _renderer.Render();
        }

        public void Update(double elapsedTime)
        {
            _lvlTitleTime -= elapsedTime;

            if (_lvlTitleTime < 0)
            {
                _gameTime += elapsedTime;
                _level.Update(elapsedTime, _gameTime);
                _score.Update(_gameData.Score);
                _lives.Update(_gameData.Lives);

                //if (_gameTime < 0)
                if (_level.Finished)
                {                    
                    if (_gameData.CurrentLevel.NextLevel != null && _gameData.CurrentLevel.NextLevel != string.Empty)
                    {
                        _gameData.NewGame = false;
                        _gameData.CurrentLevel = _levelManager.Load(_gameData.CurrentLevel.NextLevel);
                    }
                    else
                    {
                        _gameData.JustWon = true;
                        _gameData.NewGame = true;
                        _gameData.CurrentLevel = _levelManager.Load("Level_1");
                        UpdateHighScore(_gameData.Score);
                        _system.ChangeState("game_over");
                    }

                    OnGameStart();
                }

                if (_level.HasPlayerDied())
                {                    
                    _gameData.JustWon = false;
                    _gameData.NewGame = true;
                    _gameData.CurrentLevel = _levelManager.Load("Level_1");
                    OnGameStart();
                    _system.ChangeState("game_over");
                }
            }
        }

        private void UpdateHighScore(int score)
        {
            string path = "Data/HighScores.txt";
            File.WriteAllText(path, score.ToString());
            //List<int> lines = File.ReadAllLines(path).ToList().Select(int.Parse).ToList();

            //if (lines.Count() >= 5)
            //{
            //    highScores.Max();
            //}
            //else
            //{
            //    File.WriteAllText(path, score.ToString());
            //}
        }
    }
}
