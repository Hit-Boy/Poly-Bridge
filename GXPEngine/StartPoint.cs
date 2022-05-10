using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

public class StartPoint : Sprite
{
    Vec2 position;
    public StartPoint(TiledObject obj=null) : base("colors.png")
    {
        position = new Vec2(x, y);
    }

    public StartPoint(string imageFile, TiledObject obj = null) : base(imageFile)
    {

    }
}
