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

    int pointOneIndex;
    int pointTwoIndex;
    Vec2 pointTwoPos;
    Vec2 pointOnePos;
    Vec2 mousePos;
    Vec2 deltaVec;

    bool pressedLastFrame = false;
    bool pointCreated = false;
    bool pointFound = false;
    bool firstPointsDraw = false;

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
        if (!firstPointsDraw) {
            foreach (LevelPlatformPoint existPoint in existingPoints)
            {
                platformBody.AddPoint(new VerletPoint(existPoint.position.x, existPoint.position.y, true));
                if (platformBody.point.Count == 2)
                {
                    drawBody.DrawVerlet(platformBody);
                    firstPointsDraw = true;
                }
            }
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
                        pointOneIndex = i;
                        pointOnePos = platformBody.point[i].position;
                        boundary = new PlatformBoundary(platformBody.point[i].position);
                        AddChild(boundary);
                        pointCreated = true;
                        break;
                    }
                }
            }
            pressedLastFrame = true;
        }

        if (Input.GetMouseButtonUp(0) && isEditing && pointCreated)
        {
            pointDistance = 30;
            deltaVec = mousePos - pointOnePos;

            // Check if mouse is past the boundary
            if (deltaVec.Length() > boundary.radius)
            {
                float distance = deltaVec.Length();
                deltaVec = mousePos - pointOnePos;
                deltaVec *= boundary.radius / distance;
                pointTwoPos = pointOnePos + deltaVec;
            }
            else
            {
                pointTwoPos = mousePos;
            }

            for (int j = 0; j < platformBody.point.Count; j++)
            {
                deltaVec = pointTwoPos - platformBody.point[j].position;
                if (deltaVec.Length() < pointDistance)
                {
                    pointTwoIndex = j;
                    pointFound = true;
                    break;
                }
            }

            if (!pointFound)
            {
                platformBody.point.Add(new VerletPoint(pointTwoPos.x, pointTwoPos.y, false));
                pointTwoIndex = platformBody.point.Count - 1;
            }
            pointFound = false;
            if (redZone == null || redZone != null && !redZone.HitTestPoint(pointTwoPos.x, pointTwoPos.y))
            {
                platformBody.AddConstraint(pointOneIndex, pointTwoIndex);
            }
            drawBody.DrawVerlet(platformBody);
            RemoveChild(boundary);
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