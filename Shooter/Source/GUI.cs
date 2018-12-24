using PotatoEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter
{
    class GUI
    {
        string _label;
        Font _font;
        Text _guiText;
        Vector _position;

        public GUI(string label, Font font, Vector position)
        {
            _label = label;
            _font = font;
            _position = position;
            _guiText = new Text(_label + ": ", _font);
        }

        public void Update(int value)
        {
            _guiText = new Text(_label + ": " + value, _font);
            _guiText.SetColor(new Color(1, 1, 1, 1));
            _guiText.SetPosition(_position.X, _position.Y);
        }
        
        public void Render(Renderer renderer)
        {
            renderer.DrawText(_guiText);
        }
    }
}
