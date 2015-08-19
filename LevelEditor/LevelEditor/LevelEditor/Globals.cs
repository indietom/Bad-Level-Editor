using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

using LevelEditor.Core;

namespace LevelEditor
{
    enum Tools { Pen, Eraser, Box, Line, Fill }

    class Globals
    {
        public static Tools currentTool;

        public static Tileset currentTileset;

        public static byte nextLayerTag;
        public static byte activeTag;

        public static Point mapSize = new Point(10, 10);

        public static float Lerp(float s, float e, float t)
        {
            return s + t * (e - s);
        }

        public static float DistanceTo(Vector2 position, Vector2 position2)
        {
            return (float)Math.Sqrt((position.X - position2.X) * (position.X - position2.X) + (position.Y - position2.Y) * (position.Y - position2.Y));
        }

        public static void LoadProject(string path)
        {
            int lineCount = File.ReadLines(path + ".map").Count();

            Game1.layers.Clear();

            StreamReader sr = new StreamReader(path + ".map");
            currentTileset.TileSize = byte.Parse(sr.ReadLine());
            currentTileset.tilesheetPath = sr.ReadLine();
            mapSize = new Point(int.Parse(sr.ReadLine()), int.Parse(sr.ReadLine()));
            currentTileset.RefreshTileset();

            for (int i = 4; i < lineCount; i++)
            {

            }

            sr.Dispose();
        }

        public static void SaveProject(string path)
        {
            if (!File.Exists(path + ".map"))
                File.Create(path + ".map").Close();

            StreamWriter sw = new StreamWriter(path+".map");

            sw.WriteLine(Globals.currentTileset.TileSize);
            sw.WriteLine(Globals.currentTileset.tilesheetPath);
            sw.WriteLine(Globals.mapSize.X + " " + Globals.mapSize.Y);

            for (int i = 0; i < Game1.layers.Count(); i++)
            {
                sw.WriteLine(Game1.layers[i].ToString());
            }
            sw.Dispose();
        }
    }
}
