using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor.Core
{
    class Camera
    {
        public Vector2 Position {get; set;}
        public Vector2 OffsetedMouse
        {
            get
            {
                return new Vector2(Position.X + Mouse.GetState().X - 400, Position.Y + Mouse.GetState().Y - 240);
            }
        }

        public float Rotation;
        public float Zoom;

        public short moveCameraDistance = 64;

        public bool canMoveCamera;

        public Camera()
        {
            Rotation = 0;
            Zoom = 1;
        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();

            canMoveCamera = (Game1.textBoxes.Where(item => item.inFocus).Count() <= 0);

            if (canMoveCamera)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    if (mouse.X <= moveCameraDistance) Position += new Vector2(-3, 0);
                    if (mouse.X >= 800 - moveCameraDistance) Position += new Vector2(3, 0);
                    if (mouse.Y <= moveCameraDistance) Position += new Vector2(0, -3);
                    if (mouse.Y >= 480 - moveCameraDistance) Position += new Vector2(0, 3);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A)) Position += new Vector2(-3, 0);
                if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D)) Position += new Vector2(3, 0);
                if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W)) Position += new Vector2(0, -3);
                if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S)) Position += new Vector2(0, 3);
            }
        }

        public Matrix GetTransform(GraphicsDevice device2)
        {
            return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) * Matrix.CreateRotationZ(Rotation) * 
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) * Matrix.CreateTranslation(new Vector3(device2.Viewport.Width* 0.5f, device2.Viewport.Height * 0.5f, 0));
        }
    }
}
