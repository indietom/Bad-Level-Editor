using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using LevelEditor.Core;

namespace LevelEditor.Gui
{
    class TextBox
    {
        private KeyboardState keyboard;
        private KeyboardState prevKeyboard;

        private MouseState mouse;

        public string text { get; set; }
        private string label;

        public int tag;

        public bool inFocus;
        public bool allowSpaces;
        public bool numbersOnly;
        public bool destroy;

        private Rectangle hitBox;

        private Color color;

        public Vector2 Postion { get; set; }

        public TextBox(Vector2 position2, bool allowSpaces2, bool numbersOnly2, string label2, int tag2)
        {
            Postion = position2;

            allowSpaces = allowSpaces2;
            numbersOnly = numbersOnly2;

            label = label2;

            text = "";

            tag = tag2;
        }

        public void Update()
        {
            prevKeyboard = keyboard;
            keyboard = Keyboard.GetState();

            mouse = Mouse.GetState();

            hitBox = new Rectangle((int)Postion.X, (int)Postion.Y, (int)(AssetManager.font.MeasureString(text).X + AssetManager.font.MeasureString(label).X), (int)(AssetManager.font.MeasureString(text).Y + +AssetManager.font.MeasureString(label).Y));

            if (hitBox.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && mouse.LeftButton == ButtonState.Pressed)
            {
                foreach (TextBox t in Game1.textBoxes)
                {
                    if (t != this)
                    {
                        t.inFocus = false;
                    }
                    else
                        break;
                }
                inFocus = true;
            }

            if (!hitBox.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)) && mouse.LeftButton == ButtonState.Pressed)
            {
                inFocus = false;
            }

            if (inFocus)
            {
                color = Color.Blue;

                if(!Keyboard.GetState().IsKeyDown(Keys.LeftShift)) text += PressedKeys();

                if (!numbersOnly)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && keyboard.IsKeyDown(Keys.D7) && !prevKeyboard.IsKeyDown(Keys.D7))
                    {
                        text += @"\";
                    }
                }

                if (text.Length >= 1 && keyboard.IsKeyDown(Keys.Back) && !prevKeyboard.IsKeyDown(Keys.Back))
                    text = RemoveChar(text.Length - 1, text);

                if (keyboard.IsKeyDown(Keys.Space) && !prevKeyboard.IsKeyDown(Keys.Space) && allowSpaces) text += ' ';
            }
            else
            {
                color = Color.White;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(AssetManager.font, label, Postion, color, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(AssetManager.font, text, Postion + new Vector2(AssetManager.font.MeasureString(label).X, 0), color, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(AssetManager.font, "_", Postion + new Vector2(AssetManager.font.MeasureString(label).X + AssetManager.font.MeasureString(text).X, 0), color, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }

        public override string ToString()
        {
            return text;
        }

        public string PressedKeys()
        {
            string tmp = "";

            for (int i = 0; i < keyboard.GetPressedKeys().Count(); i++)
            {
                if (keyboard != prevKeyboard)
                {
                    if (numbersOnly)
                        if (keyboard.GetPressedKeys()[i].ToString().Length == 2)
                            tmp += keyboard.GetPressedKeys()[i].ToString()[1];
                    if (!numbersOnly)
                    {
                        if (keyboard.GetPressedKeys()[i].ToString().Length == 1)
                            tmp += keyboard.GetPressedKeys()[i].ToString();
                        if (keyboard.GetPressedKeys()[i].ToString().Length == 2)
                        {
                            tmp += keyboard.GetPressedKeys()[i].ToString()[1];
                        }
                    }
                }
            }
            return tmp;
        }

        public string RemoveChar(int index, string orginal)
        {
            string tmp = "";

            for (int i = 0; i < orginal.Length; i++)
                if (i != index)
                    tmp += orginal[i];

            return tmp;
        }
    }
}
