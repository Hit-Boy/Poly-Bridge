using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class Platform : GameObject
{
    Vec2 start;
    Vec2 end;
    float angle;

    public Platform(Vec2 pStart, Vec2 pEnd, float pAngle) : base()
    {
        start = pStart;
        end = pEnd;
        angle = pAngle;
    }
}