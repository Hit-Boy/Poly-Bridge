using System;
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
            ((MyGame)game).LoadLevel("Level" + (((MyGame)game).levelTracker + 1) + ".tmx");
        }else if(action == "Run Level")
        {
            level.GameStatePlay();
        }else if(action == "Edit Level")
        {
            level.GameStateEdit();
        }
    }

    void Update()
    {
        if(visualButton.HitTestPoint(Input.mouseX, Input.mouseY))
        {
            visualButton.SetColor(1, 1, 1);
            if (Input.GetMouseButtonDown(0))
            {
                DoAction();
            }
        }
        else
        {
            visualButton.SetColor(0.7f, 0.7f, 0.7f);
        }
    }
}