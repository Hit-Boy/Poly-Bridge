using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
public class LevelPlatformPoint : Sprite
{
    public Vec2 position;
    public LevelPlatformPoint(TiledObject obj = null) : base("circle.png")
    {
        position = new Vec2(obj.X, obj.Y);
        
    }

    public LevelPlatformPoint(string imageFile, TiledObject obj = null) : base(imageFile)
    {

    }
}
