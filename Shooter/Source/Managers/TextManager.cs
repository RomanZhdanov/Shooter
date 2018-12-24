using PotatoEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Source.Managers
{
    public class TextManager
    {
        Dictionary<string, Text> _textDictionary = new Dictionary<string, Text>();

        public void AddText(string textId, Text text)
        {
            _textDictionary.Add(textId, text);
        }

        public void Render(Renderer renderer)
        {
            //_textDictionary.ForEach(renderer.DrawText(t => t);
        }
    }
}
