using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class PlatformBoundary : Sprite
{
    public float radius;
    public PlatformBoundary(Vec2 position) : base("boundary.png")
    {
        SetXY(position.x, position.y);
        SetOrigin(width / 2, height / 2);
        SetScaleXY(1f, 1f);
        radius = width / 2;
    }

    void Update()
    {
        rotation += 0.2f;
    }
}