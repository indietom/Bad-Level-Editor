using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using LevelEditor.Core;
using LevelEditor.Gui;

namespace LevelEditor.FileManager
{
    class Browser
    {
        public enum Type { Folder, File };

        public struct Item
        {
            public Type type;
            public string name;

            public Vector2 Size
            {
                get
                {
                    int depth = name.Split('\\').Count();
                    return AssetManager.font.MeasureString(name.Split('\\')[depth - 1]);
                }
            }

            public Item(string name, Type type)
            {
                this.name = name;
                this.type = type;
            }

            public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
            {
                int depth = name.Split('\\').Count();
                spriteBatch.DrawString(AssetManager.font, name.Split('\\')[depth-1], position, color);
            }
        }

        public Item[] Items { get; set; }

        private int selected;
        private int offset;

        public Item pickedItem { get; private set; }

        private Vector2 position;

        private MouseState mouse;
        private MouseState prevMouse;

        private KeyboardState keyboard;
        private KeyboardState prevKeyboard;

        private string currentPath;

        private TextBox textBox;

        private Button back;

        private List<string> pastPaths;

        public Browser()
        {
            position = new Vector2(300, 200);
            currentPath = @"C:\Users\tom.leonardsson\Pictures\klassicKenny";
            LoadPath(currentPath);

            textBox = new TextBox(position + new Vector2(0, -45), true, false, "URL: ", 0);
            textBox.text = currentPath;

            back = new Button(position + new Vector2(-50, 0), GoBack, "(<-)", "go to last url");

            pastPaths = new List<string>();
        }

        public void LoadPath(string path)
        {
            selected = 0;

            string[] files = Directory.GetFiles(path);
            string[] folders = Directory.GetDirectories(path);

            Items = new Item[files.Count() + folders.Count()];

            for (int i = 0; i < files.Count(); i++)
            {
                Items[i] = new Item(files[i], Type.File);
            }

            for (int i = 0; i < folders.Count(); i++)
            {
                Items[files.Count() + i] = new Item(folders[i], Type.Folder);
            }
        }

        public void Update()
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();

            prevKeyboard = keyboard;
            keyboard = Keyboard.GetState();

            textBox.Update();

            if (textBox.inFocus && keyboard.IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter))
            {
                pastPaths.Add(currentPath);
                currentPath = textBox.text;
                LoadPath(currentPath);
            }

            back.Update();

            if (!textBox.inFocus && keyboard.IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter) && pickedItem.type == Type.Folder)
            {
                pastPaths.Add(currentPath);
                currentPath = pickedItem.name;
                LoadPath(currentPath);
            }

            for (int i = 0; i < Items.Count(); i++)
            {
                if (new Rectangle((int)position.X, i * (int)Items[i].Size.Y + (int)position.Y, (int)Items[i].Size.X, (int)Items[i].Size.Y).Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)))
                {
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
                    {
                        pickedItem = Items[i];
                        selected = i;
                    }
                }
            }
        }

        public void GoBack()
        {
            if (pastPaths.Count() > 0)
            {
                currentPath = pastPaths[pastPaths.Count() - 1];
                LoadPath(currentPath);
            }
        }

        public void GoFoward()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            textBox.Draw(spriteBatch);
            back.Draw(spriteBatch);
            if (Items != null)
            {
                for (int i = 0; i < Items.Count(); i++)
                {
                    Color color = (i == selected) ? Color.Black : Color.White;
                    Items[i].Draw(spriteBatch, position + new Vector2(0, i * Items[i].Size.Y), color);
                }
            }
        }
    }
}
