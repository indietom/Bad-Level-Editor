using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using LevelEditor.Core;
using LevelEditor.Gui;

namespace LevelEditor
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        internal static Camera camera = new Camera();

        internal static List<Button> buttons = new List<Button>();
        internal static List<Layer> layers = new List<Layer>();
        internal static List<TextBox> textBoxes = new List<TextBox>();

        bool hasAddedMapInterface = false;
        bool hasAddedCreateMapInterFace = false;
        bool creatingNewMap = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            AssetManager.Load(Content);
            Globals.currentTileset = new Tileset("tiletest.png", 10, GraphicsDevice);
            base.Initialize();
        }

        public KeyboardState keyboard;
        public KeyboardState prevKeyboard;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        public void AddLayer()
        {
            if (layers.Count <= 9) 
            layers.Add(new Layer("Layer " + (Globals.nextLayerTag+1)));
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            
            prevKeyboard = keyboard;
            keyboard = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.P)) Globals.currentTool = Tools.Pen;
            if (Keyboard.GetState().IsKeyDown(Keys.E)) Globals.currentTool = Tools.Eraser;
            if (Keyboard.GetState().IsKeyDown(Keys.F)) Globals.currentTool = Tools.Fill;

            foreach (Button b in buttons) b.Update();
            foreach (TextBox t in textBoxes) t.Update();

            if (!creatingNewMap)
            {
                if (!hasAddedMapInterface)
                {
                    Globals.currentTileset.RefreshTileset();
                    textBoxes.Clear();
                    buttons.Clear();
                    buttons.Add(new Button(new Vector2(660, 50), AddLayer, AssetManager.addButton, "Add new layer"));
                    textBoxes.Add(new TextBox(new Vector2(50, 430), true, false, " | SAVE PATH: ", 0));
                    buttons.Add(new Button(new Vector2(+AssetManager.font.MeasureString("SAVE").X / 2, 430 + AssetManager.font.MeasureString("SAVE").Y / 2), SaveMap, "SAVE", "SAVES MAP TO PATH"));
                    hasAddedMapInterface = true;
                }
                camera.Update();

                Globals.currentTileset.Update();

                if (keyboard.IsKeyDown(Keys.F1) && !prevKeyboard.IsKeyDown(Keys.F1)) Globals.SaveProject("ayylmao");

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
                hasAddedCreateMapInterFace = false;
            }
            else
            {
                hasAddedMapInterface = false;

                if (!hasAddedCreateMapInterFace)
                {
                    textBoxes.Clear();
                    buttons.Clear();
                    textBoxes.Add(new TextBox(new Vector2(10, 10), false, true, "MAP WIDTH: ", 0));
                    textBoxes.Add(new TextBox(new Vector2(10, 50), false, true, "MAP HEIGHT: ", 1));
                    textBoxes.Add(new TextBox(new Vector2(10, 90), false, true, "TILE SIZE: ", 2));
                    textBoxes.Add(new TextBox(new Vector2(10, 120), true, false, "TILESET PATH: ", 3));
                    buttons.Add(new Button(new Vector2(10+64, 350), FinishCreatingMap, "CREATE MAP", ""));
                    hasAddedCreateMapInterFace = true;
                }
            }

            base.Update(gameTime);
        }

        public void SaveMap()
        {
            Globals.SaveProject(textBoxes[0].ToString());
        }

        public void FinishCreatingMap()
        {
            if (textBoxes[0].ToString() != "" && textBoxes[1].ToString() != "" && textBoxes[2].ToString() != "")
            {
                creatingNewMap = false;
                Globals.mapSize = new Point(int.Parse(textBoxes[0].ToString()), int.Parse(textBoxes[1].ToString()));
                Globals.currentTileset.TileSize = byte.Parse(textBoxes[2].ToString());
                Globals.currentTileset.tilesheetPath = textBoxes[3].ToString()+".png";
                Globals.currentTileset.RefreshTileset();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // For map
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, camera.GetTransform(GraphicsDevice));
            if (!creatingNewMap)
            {
                spriteBatch.Draw(AssetManager.box, new Rectangle(0, 0, Globals.mapSize.X * Globals.currentTileset.TileSize, Globals.mapSize.Y * Globals.currentTileset.TileSize), Color.White);
                foreach (Layer l in layers) l.Draw(spriteBatch);
            }
            spriteBatch.End();

            //For GUI
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null);

            foreach (Button b in buttons) b.Draw(spriteBatch);
            foreach (TextBox t in textBoxes) t.Draw(spriteBatch);

            if (!creatingNewMap)
            {
                Globals.currentTileset.Draw(spriteBatch);
                foreach (Layer l in layers) l.DrawGui(spriteBatch);
                spriteBatch.DrawString(AssetManager.font, "Press P for pen, E for eraser and F for fill", new Vector2(300, 0), Color.White, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0);
            }
            else
            {
                
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
