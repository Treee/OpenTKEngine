﻿using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace engine
{
    public class Game : GameWindow
    {
        public Game(GameWorld world)
        {
            World = world;
        }

        public GameWorld World { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Title = "Camera stuff";

            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(Color4.Black);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var keys = OpenTK.Input.Keyboard.GetState();
            var mouse = OpenTK.Input.Mouse.GetState();

            if (keys[Key.Escape])
                Close();

            World.OnUpdate(keys, mouse);
            OpenTK.Input.Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.LoadIdentity();
            World.OnRender();

            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle);

            var projection = Matrix4.CreatePerspectiveFieldOfView((float) (Math.PI / 4.0f), Width / (float) Height, 0.01f, 100000f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }
    }
}
