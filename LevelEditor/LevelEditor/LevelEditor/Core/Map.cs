using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor.Core
{
    class Map
    {
        Point mapSize;

        public int[,] map;

        MouseState mouse;
        MouseState prevMouse;

        public byte Order { get; set; }

        // --- FLOOD FILL VARIABLES --- //
        sbyte targetTile;

        List<Walker> walkers = new List<Walker>();
        List<Walker> walkersToAdd = new List<Walker>();

        public Map(Point mapSize2)
        {
            mapSize = mapSize2;

            map = new int[mapSize.X, mapSize.Y];
            for (int x = 0; x < mapSize.X; x++)
            {
                for (int y = 0; y < mapSize.Y; y++)
                {
                    map[x, y] = -1;
                }
            }
        }

        public void Update(Tileset tileset)
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();

            foreach (Walker w in walkersToAdd)
                walkers.Add(w);
            walkersToAdd.Clear();

            for (int i = walkers.Count() - 1; i >= 0; i--)
            {
                if (walkers[i].destroy) walkers.RemoveAt(i);
            }

            foreach (Walker w in walkers)
            {
                w.Update(map);

                if (map[w.Position.X, w.Position.Y] == targetTile) map[w.Position.X, w.Position.Y] = tileset.PickedTile;
                else
                {
                    w.destroy = true;
                }

                if (!w.destroy)
                {
                    if (w.addWalker[0])
                    {
                        walkersToAdd.Add(new Walker(new Point(w.Position.X - 1, w.Position.Y), (byte)targetTile));
                    }
                    if (w.addWalker[1])
                    {
                        walkersToAdd.Add(new Walker(new Point(w.Position.X + 1, w.Position.Y), (byte)targetTile));
                    }
                    if (w.addWalker[2])
                    {
                        walkersToAdd.Add(new Walker(new Point(w.Position.X, w.Position.Y - 1), (byte)targetTile));
                    }
                    if (w.addWalker[3])
                    {
                        walkersToAdd.Add(new Walker(new Point(w.Position.X, w.Position.Y + 1), (byte)targetTile));
                    }
                }
                w.destroy = true;
            }

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                for (int x = 0; x < mapSize.X; x++)
                {
                    for (int y = 0; y < mapSize.Y; y++)
                    {
                        Rectangle hitbox = new Rectangle(x * tileset.TileSize, y * tileset.TileSize, tileset.TileSize, tileset.TileSize);
                        Rectangle mouseHitbox = new Rectangle((int)Game1.camera.OffsetedMouse.X, (int)Game1.camera.OffsetedMouse.Y, 1, 1);

                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            if (hitbox.Intersects(mouseHitbox))
                            {
                                if (Globals.currentTool == Tools.Pen && tileset.PickedTile != -1)
                                    map[x, y] = tileset.PickedTile;
                                if (Globals.currentTool == Tools.Eraser)
                                    map[x, y] = -1;
                                if (prevMouse.LeftButton != ButtonState.Pressed && Globals.currentTool == Tools.Fill && tileset.PickedTile != -1)
                                {
                                    targetTile = (sbyte)map[x, y];
                                    if (targetTile != tileset.PickedTile)
                                    {
                                        map[x, y] = targetTile;
                                        walkers.Add(new Walker(new Point(x, y), (byte)targetTile));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            string tmp = "";

            for (int y = 0; y < mapSize.Y; y++)
            {
                for (int x = 0; x < mapSize.X; x++)
                {
                    tmp += map[x, y]+1 + ",";
                }
                tmp += Environment.NewLine;
            }

            return tmp;
        }

        public void Draw(SpriteBatch spriteBatch, Tileset tileSet)
        {
            for (int x = 0; x < mapSize.X; x++)
            {
                for (int y = 0; y < mapSize.Y; y++)
                {
                    if (map[x, y] != -1)
                        spriteBatch.Draw(tileSet.Tilesheet, new Vector2(x * tileSet.TileSize, y * tileSet.TileSize), new Rectangle(tileSet.tileCells[map[x, y]].X, tileSet.tileCells[map[x, y]].Y, tileSet.TileSize, tileSet.TileSize), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, (Order/1000) + 0.01f);
                }
            }
        }
    }

    public class Walker
    {
        public bool[] addWalker = new bool[4];
        public bool destroy;

        public Point Position { get; set; }

        byte targetTile;

        public int[,] map = new int[Globals.mapSize.X, Globals.mapSize.Y];

        public Walker(Point position2, byte targetTile2)
        {
            Position = position2;
            targetTile = targetTile2;
        }

        public void Update(int[,] map2)
        {
            map = map2;

            if (Position.X > 0)
                if (map[Position.X - 1, Position.Y] == targetTile) addWalker[0] = true;

            if (Position.X < map.GetLength(1)-1)
                if (map[Position.X + 1, Position.Y] == targetTile) addWalker[1] = true;

            if (Position.Y > 0)
                if (map[Position.X, Position.Y - 1] == targetTile) addWalker[2] = true;

            if (Position.Y < map.GetLength(0)-1)
                if (map[Position.X, Position.Y + 1] == targetTile) addWalker[3] = true;
        }
    }
}
