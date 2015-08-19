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
    class Button
    {
        string text;
        string infoText;

        Vector2 position;

        Texture2D sprite;

        Color color;

        Action function;

        float scale;

        short displayInfoCount;

        bool makeButtonSmall;
        bool displayInfo;

        MouseState mouse;
        MouseState prevMouse;

        Rectangle hitBox;

        public Button(Vector2 position2, Action function2, Texture2D sprite2, string infoText2)
        {
            position = position2;

            function = function2;

            infoText = infoText2;

            color = Color.White;
            scale = 1;
            sprite = sprite2;
        }

        public Button(Vector2 position2, Action function2, string text2, string infoText2)
        {
            position = position2;

            function = function2;

            infoText = infoText2;

            color = Color.White;
            scale = 1;

            text = text2;
        }

        public void Update()
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();

            if (sprite != null)
                hitBox = new Rectangle((int)(position.X - sprite.Width / 2), (int)(position.Y - sprite.Height / 2), sprite.Width, sprite.Height);
            else
                hitBox = new Rectangle((int)(position.X - AssetManager.font.MeasureString(text).X / 2), (int)(position.Y - AssetManager.font.MeasureString(text).Y / 2), (int)AssetManager.font.MeasureString(text).X, (int)AssetManager.font.MeasureString(text).Y);

            if (!makeButtonSmall)
                scale = Globals.Lerp(scale, 1, 0.05f);
            else
            {
                scale = Globals.Lerp(scale, 0.85f, 0.05f);
                if (scale <= 0.875f) makeButtonSmall = false;
            }

            if (hitBox.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)))
            {
                if (displayInfoCount <= 32)
                    displayInfoCount += 1;
                else
                    displayInfo = true;

                if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                {
                    function();
                    makeButtonSmall = true;
                }
                if (sprite == null) color = Color.Blue;
            }
            else
            {
                displayInfoCount = 0;
                displayInfo = false;
                if (sprite == null) color = Color.White;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (sprite != null)
                spriteBatch.Draw(sprite, position, null, color, 0, new Vector2(sprite.Width / 2, sprite.Height / 2), scale, SpriteEffects.None, 0.9f);
            else
                spriteBatch.DrawString(AssetManager.font, text, position, color, 0, new Vector2(AssetManager.font.MeasureString(text).X / 2, AssetManager.font.MeasureString(text).Y / 2), scale, SpriteEffects.None, 0.9f);

            if (displayInfo)
                spriteBatch.DrawString(AssetManager.font, infoText, new Vector2(mouse.X, mouse.Y + AssetManager.font.MeasureString(infoText).Y), Color.Red, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
        }
    }
}
