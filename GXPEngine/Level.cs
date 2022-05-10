using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

public class Level : GameObject
{
    TiledLoader loader;
    PlayerEditingMode playerEditingMode = new PlayerEditingMode();

    public string currentLevelName;

    public Level(string filename)
    {
        currentLevelName = filename;
        loader = new TiledLoader(filename);
        loader.OnObjectCreated += OnSpriteCreated; //Subscription to this method allows for initializaiton of buttons

        AddChild(playerEditingMode);
        playerEditingMode.SetParent();
        CreateLevel();
    }

    void CreateLevel()
    {
        loader.addColliders = false;
        loader.LoadImageLayers();

        loader.autoInstance = true;
        loader.addColliders = true;
        loader.LoadObjectGroups();
    }

    void LevelWonScreen()
    {
        ((MyGame)game).LoadWinScreen();
    }

    public void GameStatePlay()
    {
        playerEditingMode.isEditing = false;
    }

    public void GameStateEdit()
    {
        playerEditingMode.isEditing = true;
    }

    void OnSpriteCreated(Sprite spr, TiledObject obj)
    {
        // Create a Button, that wraps a sprite:
        if (obj.Type == "Button")
        {
            AddChild(new Button(spr, obj));
        }
    }

    void Update()
    {
        // Temporary
        if(Input.GetKeyUp(Key.SPACE) && playerEditingMode.isEditing == false)
        {
            LevelWonScreen();
        }
    }
}