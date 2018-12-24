using PotatoEngine;
using PotatoEngine.Input;
using System;
using Tao.OpenGl;

namespace Shooter
{
    public class StartMenuState : IGameObject
    {
        Renderer _renderer = new Renderer();
        StateSystem _system;
        SoundManager _soundManager;
        Sound _backgtound_music;
        PotatoEngine.Font _generalFont;
        Input _input;
        VerticalMenu _menu;
        Text _title;
        FPS _fps;

        public StartMenuState(PotatoEngine.Font font, PotatoEngine.Font titleFont, PotatoEngine.Font generalFont, Input input, StateSystem system, SoundManager soundManager)
        {
            _input = input;
            _generalFont = generalFont;
            _system = system;
            _soundManager = soundManager;
            _fps = new FPS(font, new Color(0, 0, 0, 1));

            InitializeMenu();

            _title = new Text("The Darkest Matter", titleFont);
            _title.SetColor(new Color(0, 0, 0, 1));
            // Center on the x and place somewhere near the top
            _title.SetPosition(-_title.Width / 2, 300);
        }

        private void InitializeMenu()
        {
            _menu = new VerticalMenu(0, 150, _input, _soundManager);

            Button start_game = new Button(
                delegate (object o, EventArgs e)
                {
                    // Do start functionality
                    _soundManager.StopSound(_backgtound_music);                    
                    _system.ChangeState("inner_game");
                },
                new Text("START", _generalFont));
            Button options = new Button(
                delegate (object o, EventArgs e)
                {
                    // Do start functionality
                    _soundManager.StopSound(_backgtound_music);
                    _system.ChangeState("inner_game");
                },
                new Text("OPTIONS", _generalFont));
            Button hight_scores = new Button(
                delegate (object o, EventArgs e)
                {
                    // Do start functionality
                    _soundManager.StopSound(_backgtound_music);
                    _system.ChangeState("inner_game");
                },
                new Text("HIGH SCORES", _generalFont));
            Button exit_game = new Button(
                delegate (object o, EventArgs e)
                {
                    // Quit
                    System.Windows.Forms.Application.Exit();
                },
                new Text("EXIT", _generalFont));

            _menu.AddButton(start_game);
            _menu.AddButton(options);
            //_menu.AddButton(hight_scores);
            _menu.AddButton(exit_game);
        }

        public void Render()
        {
            Gl.glClearColor(1, 1, 1, 0);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            _fps.Render(_renderer);
            _renderer.DrawText(_title);
            _menu.Render(_renderer);
            _renderer.Render();
        }

        public void Update(double elapsedTime)
        {
            if (_backgtound_music == null || !_soundManager.IsSoundPlaying(_backgtound_music))
            {
                _backgtound_music = _soundManager.PlaySound("menu_background", true);
            }

            _menu.HandleInput();
            _fps.Update(elapsedTime);
        }
    }
}
