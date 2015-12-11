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
    class Editor : Scene
    {
        internal static List<Layer> layers = new List<Layer>();

        public Editor()
            : base()
        {
            Globals.currentTileset.RefreshTileset();
            TextBoxes.Clear();
            Buttons.Clear();
            Buttons.Add(new Button(new Vector2(660, 50), AddLayer, AssetManager.addButton, "Add new layer"));
            TextBoxes.Add(new TextBox(new Vector2(50, 430), true, false, " | SAVE PATH: ", 0));
            Buttons.Add(new Button(new Vector2(+AssetManager.font.MeasureString("SAVE").X / 2, 430 + AssetManager.font.MeasureString("SAVE").Y / 2), SaveMap, "SAVE", "SAVES MAP TO PATH"));
        }

        public override void Update()
        {
            Game1.camera.Update();

            Globals.currentTileset.Update();
            foreach (Layer l in layers) l.Update();

            for (int i = layers.Count - 1; i >= 0; i--)
            {
                if (Globals.nextLayerTag == layers[i].Order)
                    Globals.nextLayerTag += 1;

                if (layers[i].destroy)
                {
                    Globals.nextLayerTag = layers[i].Order;
                    layers.RemoveAt(i);
                }
            }

            base.Update();
        }

        public void SaveMap()
        {
            Globals.SaveProject(TextBoxes[0].ToString());
        }

        public void AddLayer()
        {
            if (layers.Count <= 9)
                layers.Add(new Layer("Layer " + (Globals.nextLayerTag + 1)));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetManager.box, new Rectangle(0, 0, Globals.mapSize.X * Globals.currentTileset.TileSize, Globals.mapSize.Y * Globals.currentTileset.TileSize), Color.White);
            foreach (Layer l in layers) l.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public override void DrawGui(SpriteBatch spriteBatch)
        {
            Globals.currentTileset.Draw(spriteBatch);
            foreach (Layer l in layers) l.DrawGui(spriteBatch);
            spriteBatch.DrawString(AssetManager.font, "Press P for pen, E for eraser and F for fill", new Vector2(300, 0), Color.White, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0);
            
            base.DrawGui(spriteBatch);
        }
    }
}
