using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using LevelEditor.Core;

namespace LevelEditor.FileManager
{
    class Browser
    {
        private string currentPath;
        private string prevPath;

        private string lastPath;

        public string[] Files { get; private set; }
        public string[] Folders { get; private set; }

        public Rectangle[] hitboxes;

        private List<string> all;

        private int offset;

        private Vector2 position;

        MouseState mouse;
        MouseState prevMouse;

        public Browser()
        {
            currentPath = @"C:\";

            all = new List<string>();

            hitboxes = new Rectangle[5];

            position = new Vector2(300, 0);
        }

        public void Update()
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();

            if (currentPath != prevPath)
            {
                lastPath = @prevPath;
                try
                {
                    Files = Directory.GetFiles(@currentPath);
                    Folders = Directory.GetDirectories(@currentPath);
                }
                catch
                {
                    Console.WriteLine("SOMETHING WENT WRONG");
                }
                prevPath = @currentPath;
                foreach (string f in Files)
                    all.Add(f);
                foreach (string f in Folders)
                    all.Add(f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (offset <= all.Count()-6) offset += 1;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (offset >= 1) offset -= 1;
            }

            for (int i = 0; i < 5; i++)
            {
                hitboxes[i] = new Rectangle((int)position.X, (int)(position.Y + i * 50), 50, 500);
            }
        }

        public string Clicked(bool checkForFolderChange)
        {
            string tmp = "";
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(hitboxes[i]))
                    {
                        if (all.Count > 0 && offset + i <= all.Count())
                        {
                            tmp = @all[i + offset];
                            offset = 0;
                        }

                        if (!tmp.Contains('.') && checkForFolderChange)
                        {
                            currentPath = @tmp;
                            all.Clear();
                        }
                    }
                }
            }
            return tmp;
        }

        public void GoBack()
        {
            currentPath = lastPath;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(AssetManager.font, Clicked(true), new Vector2(100, 100), Color.White);
            for (int i = 0; i < 5; i++)
            {
                if(all.Count() > 0 && offset+i <= all.Count())
                    spriteBatch.DrawString(AssetManager.font, all[i + offset], new Vector2(position.X, position.Y + i * 50), Color.White);
            }
        }
    }
}