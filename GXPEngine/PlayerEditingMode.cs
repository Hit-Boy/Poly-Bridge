using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class PlayerEditingMode : GameObject
{
    Level level;
    string selectedPlatform = "platform";

    public bool isEditing = true;

    List<Platform> platforms = new List<Platform>();
    public PlayerEditingMode()
    {

    }

    void CreateObject()
    {
        if(Input.GetMouseButtonDown(0) && isEditing)
        {
            // Platform creation (Create + Add to list so player can delete it if they want to)
            Console.WriteLine("Call me Bob the builder cuz I be building platforms");
        }
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