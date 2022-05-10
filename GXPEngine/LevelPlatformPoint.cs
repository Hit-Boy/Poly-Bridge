using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
public class LevelPlatformPoint : Sprite
{
    Vec2 position;
    public LevelPlatformPoint(TiledObject obj = null) : base("circle.png")
    {
        position.x = x;
        position.y = y;
        position = new Vec2(x, y);
    }

    public LevelPlatformPoint(string imageFile, TiledObject obj = null) : base(imageFile)
    {

    }
}
