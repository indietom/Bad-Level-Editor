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
using LevelEditor.FileManager;
using LevelEditor.Scenes;

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

        internal static Browser browser = new Browser();

        internal static Scene currentScene;

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
            //Globals.currentTileset = new Tileset("tiletest.png", 10, GraphicsDevice);
            browser.active = true;
            currentScene = new SetupLevel(GraphicsDevice);
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
            currentScene.Update();

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
                browser.active = false;
                Globals.mapSize = new Point(int.Parse(textBoxes[0].ToString()), int.Parse(textBoxes[1].ToString()));
                Globals.currentTileset = new Tileset(textBoxes[3].ToString(), byte.Parse(textBoxes[2].ToString()), GraphicsDevice);
                Globals.currentTileset.TileSize = byte.Parse(textBoxes[2].ToString());
                Globals.currentTileset.tilesheetPath = textBoxes[3].ToString();
                Globals.currentTileset.RefreshTileset();
                Console.WriteLine(Globals.currentTileset.Tilesheet);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // For map
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, camera.GetTransform(GraphicsDevice));
            currentScene.Draw(spriteBatch);
            spriteBatch.End();

            //For GUI
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null);

            if (browser.active) browser.Draw(spriteBatch);

            currentScene.DrawGui(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
