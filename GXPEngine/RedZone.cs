using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

class RedZone : Sprite
{
    public RedZone(TiledObject obj = null) : base("redZone.jpg")
    {
        alpha = 0.6f;
    }
}
