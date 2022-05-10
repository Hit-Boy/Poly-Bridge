using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.IO;

public class LevelEditor : GameObject // Might Change
{
    FileIO fileAcess = new FileIO();

    Level level;

    List<string> levelObjects;

    // Lists for each object in the level
    //TODO: Add background image (Index: 0)
    List<string> startPoint = new List<string>(); // Index: 1
    List<string> finishPoint = new List<string>(); // Index: 2
    List<string> platforms = new List<string>(); // Index: 3
    List<string> playerPlatormPoints = new List<string>(); // Index: 4
    List<string> levelPlatformPoints = new List<string>(); // Index: 5

    public LevelEditor()
    {

    }

    // Add every item to it's corresponding list
    void SortListItems()
    {
        levelObjects = fileAcess.ReadLevel(fileAcess.directory + level.currentLevelName + ".txt");

        for(int i=0; i<levelObjects.Count; ++i)
        {
            if (levelObjects[i].Contains("(1)"))
            {
                startPoint.Add(levelObjects[i]);
            } else if (levelObjects[i].Contains("(2)"))
            {
                finishPoint.Add(levelObjects[i]);
            } else if (levelObjects[i].Contains("(3)"))
            {
                platforms.Add(levelObjects[i]);
            } else if (levelObjects[i].Contains("(4)"))
            {
                playerPlatormPoints.Add(levelObjects[i]);
            } else if (levelObjects[i].Contains("(5)"))
            {
                levelPlatformPoints.Add(levelObjects[i]);
            }
        }
    }
    /*
    public void CreateGameObjects()
    {
        SortListItems();

        CreateStartPoint();
        CreateFinishPoint();
        CreatePlatforms();
        CreatePlayerPlatformPoints();
        CreateLevelPlatformPoints();
    }

    void CreateStartPoint()
    {
        AddChild(new StartPoint(new Vec2(float.Parse(GetBetween(startPoint[0], "pos(", ",")), float.Parse(GetBetween(startPoint[0], ",", ")")))));
    }

    void CreateFinishPoint()
    {
        AddChild(new FinishPoint(new Vec2(float.Parse(GetBetween(finishPoint[0], "pos(", ",")), float.Parse(GetBetween(finishPoint[0], ",", ")")))));
    }

    void CreatePlatforms()
    {
        foreach(string platform in platforms)
        {
            AddChild(new Platform(new Vec2(float.Parse(GetBetween(platform, "pos1(", ",")), float.Parse(GetBetween(platform, ",", ")posEnd1"))),
                                  new Vec2(float.Parse(GetBetween(platform, "pos2(", ",,")), float.Parse(GetBetween(platform, ",,", ")posEnd2"))),
                                  float.Parse(GetBetween(platform, "ang", "angEnd"))));
        }
    }

    void CreatePlayerPlatformPoints()
    {
        foreach(string point in playerPlatormPoints)
        {
            AddChild(new PlayerPlatormPoint(new Vec2(float.Parse(GetBetween(point, "pos(", ",")), float.Parse(GetBetween(point, ",", ")")))));
        }
    }

    void CreateLevelPlatformPoints()
    {
        foreach(string point in levelPlatformPoints)
        {
            AddChild(new LevelPlatformPoint(new Vec2(float.Parse(GetBetween(point, "pos(", ",")), float.Parse(GetBetween(point, ",", ")")))));
        }
    }

    public void AddNewObject(string objectType, Vec2 position, Vec2 position2 = new Vec2(), float angle = 0.0001f)
    {
        int index = 0;
        if (objectType == "startPoint")
            index = 1;
        else if (objectType == "finishPoint")
            index = 2;
        else if (objectType == "platform")
            index = 3;
        else if (objectType == "playerPlatformPoint")
            index = 4;
        else if (objectType == "levelPlatformPoint")
            index = 5;

        if(angle == 0.0001f)
        {
            fileAcess.AddToFile(level.currentLevelName, "(" + index + ")" + " " + "pos" + position);
        }
        else
        {
            fileAcess.AddToFile(level.currentLevelName, "(" + index + ")" + " " + "pos1" + position + "posEnd1" + " " + "pos2(" + position2.x + ",," + position2.y +
                                                        ")posEnd2" + " " + "ang" + angle + "angEnd");
        }
    }

    public void SetParent()
    {
        level = (Level)this.parent;
    }

    public string GetNextLevel(string currentLevel)
    {
        float index;
        index = float.Parse(GetBetween(currentLevel, "l", "."));
        return "Level" + (++index) + "."; // Current level number +1
    }

    public static string GetBetween(string strSource, string strStart, string strEnd)
    {
        if(strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            int Start, End;
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }

        return "";
    }
    */
    void Update()
    {

    }
}