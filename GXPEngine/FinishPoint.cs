using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
public class FinishPoint : Sprite
{
    Vec2 position;
    public FinishPoint(TiledObject obj = null):base("checkers.png")
    {
        position = new Vec2(x, y);
    }

    public FinishPoint(string imageFile, TiledObject obj = null) : base(imageFile)
    {

    }
}