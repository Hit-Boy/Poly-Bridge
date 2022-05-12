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

    int pointOne;
    int pointTwo;
    Vec2 existingPoint;
    Vec2 hoveringPoint;
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
        foreach (LevelPlatformPoint existPoint in existingPoints) {
            platformBody.AddPoint(new VerletPoint (existPoint.position.x, existPoint.position.y, true));
        }

        mousePos = new Vec2(Input.mouseX, Input.mouseY);
        FindRedzone();

        if (Input.GetMouseButtonDown(0) && isEditing)
        {
            if (pressedLastFrame == false)
            {
                for(int i = 0; i < platformBody.point.Count; i++) {
                    deltaVec = mousePos - platformBody.point[i].position;
                    if (deltaVec.Length() <= pointDistance)
                    {
                        pointOne = i;
                        hoveringPoint = platformBody.point[i].position;
                        boundary = new PlatformBoundary(platformBody.point[i].position);
                        AddChild(boundary);
                        pointCreated = true;
                        break;
                    }
                }

                if (!pointCreated) {
                    for (int i = 0; i < existingPoints.Count; i++)
                    {
                        deltaVec = mousePos - existingPoints[i].position;
                        if (deltaVec.Length() <= pointDistance)
                        {
                            pointOne = i;
                            hoveringPoint = existingPoints[i].position;
                            boundary = new PlatformBoundary(existingPoints[i].position);
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
            deltaVec = mousePos - hoveringPoint;

            // Check if mouse is past the boundary
            if (deltaVec.Length() > boundary.radius)
            {
                float distance = deltaVec.Length();
                deltaVec = mousePos - hoveringPoint;
                deltaVec *= boundary.radius / distance;
                existingPoint = hoveringPoint + deltaVec;
            }
            else
            {
                pointCreated = false;
                for (int i = 0; i < existingPoints.Count; i++)
                {
                    deltaVec = mousePos - existingPoints[i].position;
                    if (deltaVec.Length() <= pointDistance)
                    {
                        pointTwo = i;
                        onFixedPoint = true;
                        pointCreated = true;
                        break;
                    }
                }

                if (!pointCreated && redZone != null && redZone.HitTestPoint(existingPoint.x, existingPoint.y)) {
                    platformBody.AddPoint(new VerletPoint(existingPoint.x, existingPoint.y, false));
                    pointTwo = platformBody.point.Count - 1;
                }
            }
            platformBody.AddConstraint(pointOne, pointTwo);
            drawBody.DrawVerlet(platformBody);
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