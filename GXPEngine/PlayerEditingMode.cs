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
    bool pointCreated = false;
    bool onFixedPoint = false;

    public bool isEditing = true;

    List<LevelPlatformPoint> existingPoints = new List<LevelPlatformPoint>();
    List<Platform> platforms = new List<Platform>();
    public PlayerEditingMode(VerletBody platformBody) {
        this.platformBody = platformBody;
        AddChild(drawBody);

    }

    void FindRedzone() {
        redZone = game.FindObjectOfType<RedZone>();
    }

    void CreateObject()
    {
        int pointDistance = 20;
        existingPoints = game.FindObjectsOfType<LevelPlatformPoint>().ToList();
        mousePos = new Vec2(Input.mouseX, Input.mouseY);
        FindRedzone();

        if (Input.GetMouseButtonDown(0) && isEditing)
        {
            if (pressedLastFrame == false)
            {
                foreach (VerletPoint point in platformBody.point) {
                    deltaVec = mousePos - point.position;
                    if (deltaVec.Length() <= pointDistance)
                    {
                        pointOne.x = point.x;
                        pointOne.y = point.y;
                        platformBody.AddPoint(new VerletPoint(pointOne.x, pointOne.y, false));
                        boundary = new PlatformBoundary(pointOne);
                        AddChild(boundary);
                        pointCreated = true;
                        break;
                    }
                }

                if (!pointCreated) {
                    foreach (LevelPlatformPoint point in existingPoints)
                    {
                        deltaVec = mousePos - point.position;
                        if (deltaVec.Length() <= pointDistance)
                        {
                            pointOne = point.position;
                            platformBody.AddPoint(new VerletPoint(pointOne.x, pointOne.y, true));
                            boundary = new PlatformBoundary(pointOne);
                            AddChild(boundary);
                            pointCreated = true;
                            break;
                        }
                    }
                }
            }
            pressedLastFrame = true;
        }

        if (Input.GetMouseButtonUp(0) && isEditing && pointCreated)
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
            {
                foreach (LevelPlatformPoint point in existingPoints)
                {
                    deltaVec = mousePos - point.position;
                    if (deltaVec.Length() <= pointDistance)
                    {
                        pointTwo = point.position;
                        onFixedPoint = true;
                        break;
                    }
                    else
                    {
                        pointTwo = mousePos;
                    }
                }
            }
            
            if (redZone != null && redZone.HitTestPoint(pointTwo.x, pointTwo.y))
            {
                platformBody.RemoveLastPoint();
            }
            else if(onFixedPoint)
            {
                platformBody.AddPoint(new VerletPoint(pointTwo.x, pointTwo.y, true));
                platformBody.AddConstraint(platformBody.point.Count - 2, platformBody.point.Count - 1);
                drawBody.DrawVerlet(platformBody);
                onFixedPoint = false;
            }
            else
            {
                platformBody.AddPoint(new VerletPoint(pointTwo.x, pointTwo.y, false));
                platformBody.AddConstraint(platformBody.point.Count - 2, platformBody.point.Count - 1);
                drawBody.DrawVerlet(platformBody);
            }
            RemoveChild(boundary);
            pointCreated = false;
        }
        pressedLastFrame = false;
    }

    void DeleteObject()
    {
        int midPointDistance = 30;
        if (Input.GetKeyUp(Key.BACKSPACE) && isEditing)
        {
            mousePos = new Vec2(Input.mouseX, Input.mouseY);

            foreach (VerletConstraint cons in platformBody.constraint.ToList())
            {
                deltaVec = mousePos - cons.midPoint;
                if (deltaVec.Length() <= midPointDistance)
                {
                    platformBody.constraint.Remove(cons);
                    drawBody.DrawVerlet(platformBody);
                }
            }
        }
    }

    public void SetParent()
    {
        level = (Level)this.parent;
    }

    void Update()
    {
        CreateObject();
        DeleteObject();
    }
}