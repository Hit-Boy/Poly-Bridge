using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

public class StartPoint : Sprite
{
    public Vec2 position;
    public StartPoint(TiledObject obj=null) : base("startPoint.png")
    {
        position = new Vec2(obj.X, obj.Y);
    }

    public StartPoint(string imageFile, TiledObject obj = null) : base(imageFile)
    {

    }
}
