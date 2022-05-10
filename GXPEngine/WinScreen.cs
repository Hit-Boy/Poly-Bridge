using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

public class WinScreen : GameObject
{
    TiledLoader loader;

    public WinScreen()
    {
        loader = new TiledLoader("WinScreen.tmx");
        loader.OnObjectCreated += OnSpriteCreated; //Subscription to this method allows for initializaiton of buttons

        CreateWinScreen();
    }

    void CreateWinScreen()
    {
        loader.addColliders = false;
        loader.rootObject = game;
        loader.LoadImageLayers();

        loader.rootObject = this;
        loader.autoInstance = true;
        loader.addColliders = true;
        loader.LoadObjectGroups();
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

    }
}