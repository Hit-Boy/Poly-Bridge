using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
using System.Drawing;

public class Level : GameObject
{
    Sound winMusic = new Sound("LevelPassed.mp3");
    HUD hud = new HUD();
    TiledLoader loader;
    public PlayerEditingMode playerEditingMode;

    VerletBody body;
    Draw canvas;
    Mover mover;
    CollisionResolver collisionResolver;
    List<Obstacle> obstacles;

    bool won = false;

    private StartPoint startPoint;
    private Vec2 startPointPosition;

    private bool gravity = true;
    
    public string currentLevelName;

    public Level(string filename)
    {
        currentLevelName = filename;
        loader = new TiledLoader(filename);
        loader.OnObjectCreated += OnSpriteCreated; //Subscription to this method allows for initializaiton of buttons
        
        body = new VerletBody();
        AddChild(body);
        playerEditingMode = new PlayerEditingMode(body);
        
        canvas = new Draw(1920, 1080);
        AddChild(canvas);
        AddChild(playerEditingMode);
        playerEditingMode.SetParent();
        CreateLevel();

        obstacles = game.FindObjectsOfType<Obstacle>().ToList();
        startPoint = game.FindObjectOfType<StartPoint>();

        if (currentLevelName != "MainMenu.tmx" && currentLevelName != "Credits.tmx" && currentLevelName != "WinScreen.tmx"
           && currentLevelName != "EndGame.tmx")
        {
            AddChild(hud);
            hud.SetParent();
            if (startPoint != null)
            {
                mover = new Mover(startPoint.position.x, startPoint.position.y, 20);
                AddChild(mover);
            }
        }

        if(mover != null)
            collisionResolver = new CollisionResolver(body, mover, obstacles);
    }
    

    void DrawAll() {
        canvas.Clear (System.Drawing.Color.Transparent);
        canvas.DrawVerlet(body);
        canvas.DrawMover(mover);
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
        winMusic.Play();
        won = true;
    }

    public void GameStatePlay()
    {
        // Game is in Play Mode
        playerEditingMode.isEditing = false;
        playerEditingMode.ClearCanvas();
    }

    public void GameStateEdit()
    {
        // Game is in Edit Mode
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
        
        if (!playerEditingMode.isEditing) {
            collisionResolver.collisionThisFrame = false;
            int iterationCount = 20;
            if (gravity) {
                body.AddAcceleration(new Vec2(0, 0.2f));
                mover.AddAcceleration(new Vec2(0, 0.2f));
                mover.MoveMover();
                body.UpdateVerlet();

                for (int i = 0; i < iterationCount; i++) {
                    body.UpdateConstraints();
                    CheckAllCollisions();
                }

                if (collisionResolver.CheckFinish())
                    if(!won)
                        LevelWonScreen();
                UpdateAllSprites();
                //DrawAll();
            }
            
        }

        if (Input.GetKeyUp(Key.SPACE) && !playerEditingMode.isEditing)
            LevelWonScreen();
    }
    
    private void CheckAllCollisions() {
        collisionResolver.VerletObstacleCollisionCheck();
        collisionResolver.ObstacleMoverCollisionCheck();
        collisionResolver.VerletMoverCollisionCheck();
    }

    private void UpdateAllSprites() {
        foreach (VerletConstraint c in body.constraint) {
            c.UpdateConstraintSprite();
        }
        mover.UpdateMoverSprite();
        
    }
}