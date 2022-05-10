using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class PlayerPlatormPoint : GameObject
{
    Vec2 position;
    public PlayerPlatormPoint(Vec2 pPosition) : base()
    {
        position = pPosition;
        SetXY(position.x, position.y);
    }
}
