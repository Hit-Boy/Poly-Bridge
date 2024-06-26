﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

class Button : GameObject
{
    Sprite visualButton;

    Level level;

    string action;

    public Button(Sprite visualButton, TiledObject obj)
    {
        this.visualButton = visualButton;
        action = obj.GetStringProperty("Action");
    }

    void DoAction()
    {
        level = ((MyGame)game).FindObjectOfType<Level>();

        if (action == "Next Level")
        {
            if (level.currentLevelName == "Level4.tmx")
            {
                ((MyGame)game).LoadLevel("EndScreen.tmx", true);
                ((MyGame)game).levelTracker = 0;
            }
            else {
                ((MyGame)game).LoadLevel("Level" + (((MyGame)game).levelTracker + 1) + ".tmx");
            }
        }else if(action == "Run Level")
        {
            level.GameStatePlay();
        }else if(action == "Edit Level")
        {
            level.GameStateEdit();
        }else if(action == "Restart Level")
        {
            ((MyGame)game).LoadLevel("Level" + ((MyGame)game).levelTracker + ".tmx", true);
        }else if(action == "Credits")
        {
            ((MyGame)game).LoadLevel("Credits.tmx", true);
        }else if(action == "Back")
        {
            ((MyGame)game).LoadLevel("MainMenu.tmx", true);
            ((MyGame)game).levelTracker = 0;
        }
    }

    void Update()
    {
        if (visualButton.HitTestPoint(Input.mouseX, Input.mouseY))
        {
            visualButton.SetColor(1, 1, 1);
            if (Input.GetMouseButtonDown(0))
            {
                DoAction();
            }
        }
        else
        {
            visualButton.SetColor(0.9f, 0.9f, 0.9f);
        }
    }
}