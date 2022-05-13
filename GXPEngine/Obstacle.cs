using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

    public class Obstacle : Sprite{
        public Vec2 position;
        public float radius;

        public Obstacle(TiledObject obj = null) : base("circle.png"){
        alpha = 0f;
        radius = 90;
        position = new Vec2(obj.X + radius, obj.Y + radius);
    }

    }
