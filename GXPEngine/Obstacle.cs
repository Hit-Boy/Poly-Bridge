using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using GXPEngine;
using GXPEngine.Core;

    public class Obstacle {
        public Vec2 position;
        public float radius;
/*
        public Obstacle(int level) : base("level" + level.ToString(), 3, 2, -1, true, false) {
            SetOrigin(width/2, height/2);
            SetFrame(Utils.Random(0, 6));
            
        }
*/
        public Obstacle(Vec2 position, float radius) {
            this.position = position;
            this.radius = radius;
        }

    }
