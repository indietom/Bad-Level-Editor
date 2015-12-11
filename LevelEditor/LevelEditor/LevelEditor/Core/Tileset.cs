using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor.Core
{
    class Tileset
    {
        public string tilesheetPath;

        GraphicsDevice graphicsDevice;

        Texture2D LoadTilesheet
        {
            get
            {
                try
                {
                    using (Stream s = TitleContainer.OpenStream(@tilesheetPath))
                        return Texture2D.FromStream(graphicsDevice, s);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    return AssetManager.errorImage;
                }
            }
        }

        public Vector2 Position { get; set; }
        Vector2 prevPosition;

        public Texture2D Tilesheet { get; set; }

        public byte TileSize { get; set; }

        bool tileIsPicked;

        public sbyte PickedTile { get; set; }

        public Point[] tileCells;
        public Rectangle[] hitBoxes;
        
        public short AmountOfTiles { get { return (short)((Tilesheet.Width / TileSize) * (Tilesheet.Height / TileSize)); } }

        MouseState mouse;
        MouseState prevMouse;

        public Tileset(string tilesheetPath2, byte tileSize2, GraphicsDevice graphicsDevice2)
        {
            graphicsDevice = graphicsDevice2;

            tilesheetPath = tilesheetPath2;
            Tilesheet = LoadTilesheet;

            TileSize = tileSize2;

            AssignTileCells();

            PickedTile = -1;

            Console.WriteLine(tilesheetPath);

            hitBoxes = new Rectangle[AmountOfTiles];
        }

        public void RefreshTileset()
        {
            hitBoxes = new Rectangle[AmountOfTiles];
            Tilesheet = LoadTilesheet;
            AssignTileCells();
        }
        
        public void AssignTileCells()
        {
            tileCells = new Point[AmountOfTiles];

            byte length = (byte)(Tilesheet.Width / TileSize);
            sbyte jumpLineCount = -1;

            Point sourcePoint = new Point(0, 0);

            for (int i = 0; i < AmountOfTiles; i++)
            {
                jumpLineCount += 1;
                if (jumpLineCount >= length)
                {
                    sourcePoint = new Point(0, sourcePoint.Y + TileSize);
                    jumpLineCount = 0;
                }
                tileCells[i] = sourcePoint;
                sourcePoint.X += TileSize;
            }
        }

        public void Update()
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();

            if (prevPosition != Position)
            {
                for (int i = 0; i < 5; i++)
                {
                    hitBoxes[i] = new Rectangle(0, 0, 0, 0);
                }
            }
            prevPosition = Position;

            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                Console.WriteLine(AmountOfTiles);
                for (int i = 0; i < AmountOfTiles; i++)
                {
                    if (hitBoxes[i].Intersects(new Rectangle((int)mouse.X, (int)mouse.Y, 1, 1)))
                    {
                        PickedTile = (sbyte)i;
                    }
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 tmp = Vector2.Zero;
            sbyte jumpLineCount = -1;

            for (int i = 0; i < AmountOfTiles; i++)
            {
                jumpLineCount += 1;
                if (jumpLineCount >= 3)
                {
                    tmp = new Vector2(0, tmp.Y + TileSize);
                    jumpLineCount = 0;
                }

                if (PickedTile == i)
                    spriteBatch.Draw(Tilesheet, tmp+Position, new Rectangle(tileCells[i].X, tileCells[i].Y, TileSize, TileSize), Color.Black);
                else
                    spriteBatch.Draw(Tilesheet, tmp+Position, new Rectangle(tileCells[i].X, tileCells[i].Y, TileSize, TileSize), Color.White);

                for (int j = 0; j < AmountOfTiles; j++)
                {
                    if (hitBoxes[j] == new Rectangle(0, 0, 0, 0))
                    {
                        hitBoxes[j] = new Rectangle((int)(tmp.X + Position.X), (int)(tmp.Y + Position.Y), TileSize, TileSize);
                        break;
                    }
                }

                tmp += new Vector2(TileSize, 0);
            }
            jumpLineCount = 0;
        }
    }
}
