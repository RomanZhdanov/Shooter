using PotatoEngine;
using PotatoEngine.Input;
using System.Windows.Forms;
using Tao.OpenGl;

namespace Shooter
{
    class GameOverState : IGameObject
    {
        const double _timeOut = 10;

        double _countDown = _timeOut;

        StateSystem         _system;
        SoundManager        _soundManager;
        Sound               _backgtound_music;
        Input               _input;
        Font                _generalFont;
        Font                _titleFont;
        PersistantGameData  _gameData;
        Renderer            _renderer = new Renderer();

        Text _titleWin;
        Text _blurbWin;

        Text _titleLose;
        Text _blurbLose;

        Text _score;

        public GameOverState(PersistantGameData data, StateSystem system, Input input, Font generalFont, Font titleFont, SoundManager soundManager)
        {
            _gameData = data;
            _system = system;
            _input = input;
            _generalFont = generalFont;
            _titleFont = titleFont;
            _soundManager = soundManager;

            _titleWin = new Text("Level Complete", _titleFont);
            _blurbWin = new Text("Congratulations, you won!", _generalFont);

            _titleLose = new Text("Game Over", _titleFont);
            _blurbLose = new Text("You lose... Please try again.", _generalFont);

            _score = new Text("Your Score: " + _gameData.Score, _generalFont);

            FormatText(_titleWin, 300);
            FormatText(_blurbWin, 200);

            FormatText(_titleLose, 300);
            FormatText(_blurbLose, 200);

            FormatText(_score, 100);
        }

        private void FormatText(Text text, int yPosition)
        {
            text.SetPosition(-text.Width / 2, yPosition);
            text.SetColor(new Color(0, 0, 0, 1));
        }

        public void Render()
        {
            Gl.glClearColor(1, 1, 1, 0);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            if (_gameData.JustWon)
            {
                _renderer.DrawText(_titleWin);
                _renderer.DrawText(_blurbWin);
            }
            else
            {
                _renderer.DrawText(_titleLose);
                _renderer.DrawText(_blurbLose);
            }

            _renderer.DrawText(_score);
            _renderer.Render();
        }

        public void Update(double elapsedTime)
        {
            _score = new Text("Your Score: " + _gameData.Score, _generalFont);
            FormatText(_score, 100);

            if (_backgtound_music == null || !_soundManager.IsSoundPlaying(_backgtound_music))
            {
                _backgtound_music = _soundManager.PlaySound("menu_background", true);
            }

            _countDown -= elapsedTime;

            if (_input.Controller != null)
            {
                if (_input.Controller.ButtonA.Pressed) Finish();
            }

            if (_countDown <= 0 || _input.Keyboard.IsKeyPressed(Keys.Enter))
            {
                Finish();
            }
        }

        private void Finish()
        {
            _gameData.JustWon = false;
            _gameData.Score = 0;
            _gameData.Lives = 2;
            _system.ChangeState("start_menu");
            _countDown = _timeOut;
        }
    }
}
