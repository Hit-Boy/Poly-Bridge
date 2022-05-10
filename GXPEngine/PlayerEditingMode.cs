using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class PlayerEditingMode : GameObject
{
    Level level;
    PlatformBoundary boundary;
    RedZone redZone;

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
        mousePos = new Vec2(Input.mouseX, Input.mouseY);
        redZone = game.FindObjectOfType<RedZone>();

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

            // Check if mouse is past the boundary
            if (deltaVec.Length() > boundary.radius)
            {
                float distance = deltaVec.Length();
                deltaVec = mousePos - pointOne;
                deltaVec *= boundary.radius / distance;
                pointTwo = pointOne + deltaVec;
            }
            else
                pointTwo = mousePos;

            if (redZone.HitTestPoint(pointTwo.x, pointTwo.y))
            {
                platformBody.RemoveLastPoint();
            }
            else
            {
                platformBody.AddPoint(new VerletPoint(pointTwo.x, pointTwo.y, false));
                platformBody.AddConstraint(platformBody.point.Count - 2, platformBody.point.Count - 1);
                drawBody.DrawVerlet(platformBody);
            }
            RemoveChild(boundary);
        }

        pressedLastFrame = false;
    }


    public void SetParent()
    {
        level = (Level)this.parent;
    }

    void Update()
    {
        CreateObject();
    }
}