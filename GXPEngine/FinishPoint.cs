using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
public class FinishPoint : Sprite
{
    Vec2 position;
    public FinishPoint(TiledObject obj = null):base("finishPoint.png")
    {
        position = new Vec2(obj.X, obj.Y);
    }

    public FinishPoint(string imageFile, TiledObject obj = null) : base(imageFile)
    {

    }
}