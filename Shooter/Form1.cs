using PotatoEngine;
using PotatoEngine.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tao.DevIl;
using Tao.OpenGl;

namespace Shooter
{
    public partial class Form1 : Form
    {
        bool            _fullscreen     = false;
        FastLoop        _fastLoop;
        StateSystem     _system         = new StateSystem();
        Input           _input          = new Input();
        TextureManager  _textureManager = new TextureManager();
        SoundManager    _soundManager   = new SoundManager();
        LevelManager    _levelManager   = new LevelManager();

        PersistantGameData _persistantGameData = new PersistantGameData();

        PotatoEngine.Font _font;
        PotatoEngine.Font _titleFont;
        PotatoEngine.Font _generalFont;

        public Form1()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();

            _input.Mouse = new Mouse(this, simpleOpenGlControl1);
            _input.Keyboard = new Keyboard(simpleOpenGlControl1);

            InitializeDisplay();
            InitializeSounds();
            InitializeTextures();
            InitializeLevels();
            InitializeGameData();
            InitializeFonts();
            InitializeGameState();

            _fastLoop = new FastLoop(GameLoop);
        }

        private void InitializeGameState()
        {
            // Load game states here   
            _system.AddState("start_menu", new StartMenuState(_font, _titleFont, _generalFont, _input, _system, _soundManager));
            _system.AddState("inner_game", new InnerGameState(_input, _system, _textureManager, _soundManager, _levelManager, _persistantGameData, _font, _titleFont));
            _system.AddState("game_over", new GameOverState(_persistantGameData, _system, _input, _generalFont, _titleFont, _soundManager));
            _system.ChangeState("start_menu");
        }

        private void InitializeSounds()
        {
            // Sounds //
            _soundManager.LoadSound("select", "Assets/Sounds/select.wav");
            _soundManager.LoadSound("select_sound", "Assets/Sounds/blip_select.wav");

            // Music //
            _soundManager.LoadSound("menu_background", "Assets/Sounds/Music/menu_background.wav");
            _soundManager.LoadSound("level_background", "Assets/Sounds/Music/Level_01_2.wav");
        }

        private void InitializeTextures()
        {
            // Init DevIl
            Il.ilInit();
            Ilu.iluInit();
            Ilut.ilutInit();
            Ilut.ilutRenderer(Ilut.ILUT_OPENGL);

            // Fonts //
            _textureManager.LoadTexture("font", "Assets/Textures/Fonts/font.tga");
            _textureManager.LoadTexture("title_font", "Assets/Textures/Fonts/title_font_0.tga");
            _textureManager.LoadTexture("general_font", "Assets/Textures/Fonts/general_font_0.tga");

            // Entities //
            _textureManager.LoadTexture("player_ship", "Assets/Textures/Entities/spacecraft.png");
            _textureManager.LoadTexture("enemy_ship", "Assets/Textures/Entities/ospaceship-main.png");
            _textureManager.LoadTexture("boss", "Assets/Textures/Entities/alien_boss_300x300.png");
            _textureManager.LoadTexture("enemy_heavy", "Assets/Textures/Entities/alien_spacecraft_200x275.png");
            _textureManager.LoadTexture("bullet", "Assets/Textures/Entities/bullet.tga");

            // Backgrounds//
            _textureManager.LoadTexture("background", "Assets/Textures/Backgrounds/background.tga");
            _textureManager.LoadTexture("background_layer_1", "Assets/Textures/Backgrounds/background_p.tga");
            _textureManager.LoadTexture("background_supernova", "Assets/Textures/Backgrounds/supernova.jpg");
            _textureManager.LoadTexture("planet_28", "Assets/Textures/Backgrounds/planet_28.png");
            //_textureManager.LoadTexture("background", "Assets/Textures/Backgrounds/space_background.png");
            //_textureManager.LoadTexture("background_layer_1", "Assets/Textures/Backgrounds/Starfield.png");

            // Animations //
            _textureManager.LoadTexture("explosion", "Assets/Textures/Animations/explode.tga");
            _textureManager.LoadTexture("asteroid", "Assets/Textures/Animations/800x600-asteroid_01_no_moblur.png");
        }

        private void InitializeLevels()
        {
            _levelManager.AddLevel("Level_1", "Data/Levels/Level_01.txt");
            _levelManager.AddLevel("Level_2", "Data/Levels/Level_02.txt");
        }

        private void InitializeGameData()
        {
            _persistantGameData.CurrentLevel = _levelManager.Load("Level_1");
            _persistantGameData.NewGame = true;
        }

        private List<EnemyDef> GetEnemies()
        {
            List<EnemyDef> enemies = new List<EnemyDef>();
            return enemies;
        }

        private void InitializeFonts()
        {
            // Fonts are loaded here
            _font = new PotatoEngine.Font(_textureManager.Get("font"),
                FontParser.Parse("Assets/Textures/Fonts/font.fnt"));
            _titleFont = new PotatoEngine.Font(_textureManager.Get("title_font"), 
                FontParser.Parse("Assets/Textures/Fonts/title_font.fnt"));
            _generalFont = new PotatoEngine.Font(_textureManager.Get("general_font"),
                FontParser.Parse("Assets/Textures/Fonts/general_font.fnt"));
        }

        private void InitializeDisplay()
        {
            if (_fullscreen)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                ClientSize = new Size(1280, 720);
            }

            Setup2DGraphics(ClientSize.Width, ClientSize.Height);
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            Gl.glViewport(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            Setup2DGraphics(ClientSize.Width, ClientSize.Height);
        }

        private void Setup2DGraphics(double width, double height)
        {
            double halfWidth = width / 2;
            double halfHeight = height / 2;
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glOrtho(-halfWidth, halfWidth, -halfHeight, halfHeight, -100, 100);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }

        private void GameLoop(double elapsedTime)
        {
            UpdateInput(elapsedTime);
            _system.Update(elapsedTime);
            _system.Render();
            simpleOpenGlControl1.Refresh();
        }

        private void UpdateInput(double elapsedTime)
        {
            _input.Update(elapsedTime);
        }
    }
}
