using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class MouseFollower : AnimationSprite
{
    int counter = 0;
    int frame = 0;

    Vec2 mousePos;
    Vec2 position;

    public MouseFollower() : base("mouseFollower.png", 4, 1)
    {
        SetScaleXY(0.1f, 0.1f);
    }

    void MoveAnimation()
    {
        counter++;
        if(counter > 4)
        {
            counter = 0;
            frame++;
            if(frame == frameCount)
            {
                frame = 0;
            }
            SetFrame(frame);
        }
    }

    void PositionManager()
    {
        mousePos = new Vec2(Input.mouseX, Input.mouseY);
        position = mousePos;
    }

    void UpdateScreenPosition()
    {
        x = position.x - 55;
        y = position.y - 160;
    }

    void Update()
    {
        MoveAnimation();
        PositionManager();
        UpdateScreenPosition();
    }
}
