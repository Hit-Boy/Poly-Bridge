using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

    public class Obstacle : Sprite{
        public Vec2 position;
        public float radius;

    public Obstacle(TiledObject obj = null) : base("obs.png")
    {
        radius = 100;
        position = new Vec2(obj.X + radius, obj.Y + radius);
        alpha = obj.GetIntProperty("alpha");
    }

    }
