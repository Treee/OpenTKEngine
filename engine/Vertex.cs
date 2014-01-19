﻿using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics;

namespace engine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public Vertex(Vector3 position, Color4 color, Vector3 normal)
        {
            Position = position;
            Color = color;
            Normal = normal;
        }

        public Vector3 Position;
        public Color4 Color;
        public Vector3 Normal;

        public const int SizeInBytes = sizeof(float) * 10;
    }
}
