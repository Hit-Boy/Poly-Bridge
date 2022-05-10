using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class PlayerEditingMode : GameObject
{
    Level level;
    PlatformBoundary boundary;

    VerletBody platformBody = new VerletBody();
    VerletDraw drawBody = new VerletDraw(800, 600);

    Vec2 pointOne;
    Vec2 pointTwo;
    Vec2 mousePos;
    Vec2 deltaVec;

    bool pressedLastFrame = false;

    public bool isEditing = true;

    List<Platform> platforms = new List<Platform>();
    public PlayerEditingMode()
    {
        AddChild(drawBody);
    }

    void CreateObject()
    {
        float offset;

        mousePos = new Vec2(Input.mouseX, Input.mouseY);
        if (Input.GetMouseButtonDown(0) && isEditing)
        {
            if (pressedLastFrame == false)
            {
                pointOne = mousePos;
                platformBody.AddPoint(new VerletPoint(pointOne.x, pointOne.y, false));
                boundary = new PlatformBoundary(pointOne);
                AddChild(boundary);
            }
            pressedLastFrame = true;
        }

        if (Input.GetMouseButtonUp(0) && isEditing)
        {
            deltaVec = mousePos - pointOne;

            // Check if mouse is over the radius
            if (deltaVec.Length() > boundary.radius)
            {
                offset = boundary.radius - deltaVec.Length();
                pointTwo = mousePos;
            }
            else
                pointTwo = mousePos;
            
            //pointTwo = mousePos;
            
            platformBody.AddPoint(new VerletPoint(pointTwo.x, pointTwo.y, false));
            platformBody.AddConstraint(platformBody.point.Count - 2, platformBody.point.Count - 1);

            RemoveChild(boundary);
            pressedLastFrame = false;
        }
    }


    public void SetParent()
    {
        level = (Level)this.parent;
    }

    void Update()
    {
        CreateObject();
        drawBody.DrawVerlet(platformBody);

    }
}