using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotatoEngine;

namespace Shooter
{
    class FramesPerSecond
    {
        int _numberOfFrames = 0;
        double _timePassed = 0;
        public double CurrentFPS { get; set; }
        public void Process(double timeElapsed)
        {
            _numberOfFrames++;
            _timePassed += timeElapsed;

            if (_timePassed > 1)
            {
                CurrentFPS = (double)_numberOfFrames / _timePassed;
                _timePassed = 0;
                _numberOfFrames = 0;
            }
        }
    }

    class FPS
    {
        Text _fpsText;
        Font _font;
        Color _color;
        FramesPerSecond _fps = new FramesPerSecond();

        public FPS(Font font, Color color)
        {
            _font = font;
            _color = color;
            _fpsText = new Text("FPS:", _font);
        }

        public void Update(double elapsedTime)
        {
            _fps.Process(elapsedTime);
        }

        public void Render(Renderer renderer)
        {
            _fpsText = new Text("FPS: " + _fps.CurrentFPS.ToString("00.0"), _font);
            _fpsText.SetPosition(-600, 350);
            _fpsText.SetColor(_color);            
            renderer.DrawText(_fpsText);
        }
    }
}
