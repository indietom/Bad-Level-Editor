using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LevelEditor.Core;
using LevelEditor.Gui;

namespace LevelEditor.Scenes
{
    class SetupLevel : Scene
    {
        public SetupLevel(GraphicsDevice device)
            : base()
        {
            TextBoxes.Add(new TextBox(new Vector2(10, 10), false, true, "MAP WIDTH: ", 0));
            TextBoxes.Add(new TextBox(new Vector2(10, 50), false, true, "MAP HEIGHT: ", 1));
            TextBoxes.Add(new TextBox(new Vector2(10, 90), false, true, "TILE SIZE: ", 2));
            TextBoxes.Add(new TextBox(new Vector2(10, 120), true, false, "TILESET PATH: ", 3));
            Buttons.Add(new Button(new Vector2(10 + 64, 350), () => FinishCreatingMap(device), "CREATE MAP", ""));
        }

        public void FinishCreatingMap(GraphicsDevice device)
        {
            if (TextBoxes[0].ToString() != "" && TextBoxes[1].ToString() != "" && TextBoxes[2].ToString() != "")
            {
                Globals.mapSize = new Point(int.Parse(TextBoxes[0].ToString()), int.Parse(TextBoxes[1].ToString()));
                Globals.currentTileset = new Tileset(TextBoxes[3].ToString(), byte.Parse(TextBoxes[2].ToString()), device);
                Globals.currentTileset.TileSize = byte.Parse(TextBoxes[2].ToString());
                Globals.currentTileset.tilesheetPath = TextBoxes[3].ToString();
                Globals.currentTileset.RefreshTileset();
                Console.WriteLine(Globals.currentTileset.Tilesheet);

                Game1.browser.active = false;
                Game1.currentScene = new Editor();
            }
        }
    }
}
