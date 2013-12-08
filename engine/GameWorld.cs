﻿using System;
using System.Collections.Generic;

using engine.entities;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using SimplexNoise;

namespace engine
{
    public class GameWorld
    {
        public GameWorld()
        {
            Camera = new CameraEntity(Vector3.Zero, Quaternion.Identity);
            Entities = new List<Entity>();
        }

        public CameraEntity Camera { get; set; }
        public List<Entity> Entities { get; set; }

        public void OnUpdate(KeyboardState keyboard, MouseState mouse)
        {
            Camera.Update(keyboard, mouse);
            Entities.ForEach(e => e.Update(keyboard, mouse));
        }

        public void OnRender()
        {
            GL.PushMatrix();
            Camera.Render();

            Entities.ForEach(e => e.Render());

            GL.PopMatrix();
        }
    }

    public static class GameWorldFactory
    {
        public static GameWorld BuildDottedSphere(float size, float variance, float density, int columns = 16, int rows = 16)
        {
            var rand = new Random();
            var sx = (float)rand.NextDouble() * 1000f;
            var sy = (float)rand.NextDouble() * 1000f;
            var sz = (float)rand.NextDouble() * 1000f;

            var world = new GameWorld();

            {
                var v = Noise.Generate(sx, sy + size * density, sz) * variance;
                //v = v * v * (v > 0 ? 1 : -1);
                world.Entities.Add(new PointEntity(new Vector3(0, size + size * v, 0), Quaternion.Identity, Color4.White));
            }
            for (var r = 1; r < rows; r++)
            {
                var rr = r / (float) rows;
                for (var c = 0; c < columns; c++)
                {
                    var cr = c / (float) columns;

                    var x = (float) (Math.Sin(Math.PI * rr) * Math.Cos(2 * Math.PI * cr) * size);
                    var y = (float) (Math.Cos(Math.PI * rr) * size);
                    var z = (float) (Math.Sin(Math.PI * rr) * Math.Sin(2 * Math.PI * cr) * size);

                    var v = Noise.Generate(sx + x * density, sy + y * density, sz + z * density) * variance;
                    //v = v * v * (v > 0 ? 1 : -1);

                    var nx = x + x * v;
                    var ny = y + y * v;
                    var nz = z + z * v;

                    world.Entities.Add(
                        new PointEntity(
                            new Vector3(nx, ny, nz), Quaternion.Identity,
                            new Color4(
                                nx > 0f ? 1f : 0.1f,
                                ny > 0f ? 1f : 0.1f,
                                nz > 0f ? 1f : 0.1f,
                                1f)));
                    // dark blue sphere (sea)
                    //world.Entities.Add(
                    //    new PointEntity(
                    //        new Vector3(x, y, z), Quaternion.Identity, new Color4(0, 0, 150, 25)));
                }
            }
            {
                var v = Noise.Generate(sx, sy - size * density, sz) * variance;
                //v = v * v * (v > 0 ? 1 : -1);
                world.Entities.Add(new PointEntity(new Vector3(0, -(size + size * v), 0), Quaternion.Identity, Color4.White));
            }

            return world;
        }
    }
}
