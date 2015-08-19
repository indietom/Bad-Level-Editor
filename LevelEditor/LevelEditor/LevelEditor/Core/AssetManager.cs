using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LevelEditor.Core
{
    class AssetManager
    {
        public static Texture2D errorImage, addButton, box;

        public static SpriteFont font;

        public static void Load(ContentManager content)
        {
            errorImage = content.Load<Texture2D>("errorImage");
            addButton = content.Load<Texture2D>("addButton");
            box = content.Load<Texture2D>("box");

            font = content.Load<SpriteFont>("font");
        }
    }
}
