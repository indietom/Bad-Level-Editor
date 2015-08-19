using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor.Core
{
    class Layer
    {
        public Vector2 Position { get; set; }

        public bool destroy;

        string name;

        public byte Order { get; set; }

        Rectangle hitbox;

        Map map;

        public Layer(string name2)
        {
            name = name2;

            Position = new Vector2(700, 0);

            map = new Map(Globals.mapSize);

            Order = Globals.nextLayerTag;
            Globals.nextLayerTag += 1;
        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();

            if (Order == Globals.activeTag) map.Update(Globals.currentTileset);
            
            map.Order = Order;

            hitbox = new Rectangle((int)Position.X, (int)(Position.Y + Order * AssetManager.font.MeasureString(name).Y), (int)AssetManager.font.MeasureString(name).X, (int)AssetManager.font.MeasureString(name).Y);

            if(hitbox.Intersects(new Rectangle(mouse.X, mouse.Y, 1, 1)))
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                    Globals.activeTag = Order;
                if (mouse.RightButton == ButtonState.Pressed)
                    destroy = true;
            }
        }

        public override string ToString()
        {
            return "l" + name + Environment.NewLine + map.ToString();
        }

        public void DrawGui(SpriteBatch spriteBatch)
        {
            Color color = Color.White;

            color = (Order == Globals.activeTag) ? Color.Green : color;

            spriteBatch.DrawString(AssetManager.font, name, Position + new Vector2(0, Order * AssetManager.font.MeasureString(name).Y), color);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch, Globals.currentTileset);
        }
    }
}
