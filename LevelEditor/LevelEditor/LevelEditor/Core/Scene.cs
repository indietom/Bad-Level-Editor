using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

using LevelEditor.Gui;

namespace LevelEditor.Core
{
    class Scene
    {
        public List<Button> Buttons { get; set; }
        public List<TextBox> TextBoxes { get; set; }

        public Scene()
        {
            Buttons = new List<Button>();
            TextBoxes = new List<TextBox>();
        }

        public virtual void Update()
        {
            foreach (Button b in Buttons) b.Update();
            foreach (TextBox t in TextBoxes) t.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch) {}

        public virtual void DrawGui(SpriteBatch spriteBatch)
        {
            foreach (Button b in Buttons) b.Draw(spriteBatch);
            foreach (TextBox t in TextBoxes) t.Draw(spriteBatch);
        }
    }
}
