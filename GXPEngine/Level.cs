using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

public class Level : GameObject
{
    Sound winMusic = new Sound("LevelPassed.mp3");
    TiledLoader loader;
    public PlayerEditingMode playerEditingMode;

    VerletBody body;
    Draw canvas;
    Mover mover;
    CollisionResolver collisionResolver;
    List<Obstacle> obstacles;

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
        playerEditingMode = new PlayerEditingMode(body);
        
        canvas = new Draw(800, 600);
        AddChild(canvas);
        AddChild(playerEditingMode);
        playerEditingMode.SetParent();
        CreateLevel();

        obstacles = new List<Obstacle>();
        startPoint = game.FindObjectOfType<StartPoint>();
        
        if(currentLevelName != "MainMenu.txt" || currentLevelName != "Credits.txt" || currentLevelName != "WinScreen.txt" 
           || currentLevelName != "EndGame.txt")
            if(startPoint != null)
                mover = new Mover(startPoint.position.x, startPoint.position.y);
        
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

        loader.LoadTileLayers();
        loader.autoInstance = true;
        loader.addColliders = true;
        loader.LoadObjectGroups();

    }

    void LevelWonScreen()
    {
        ((MyGame)game).LoadWinScreen();
        winMusic.Play();
    }

    public void GameStatePlay()
    {
        // Game is in Play Mode
        playerEditingMode.isEditing = false;
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
            int iterationCount = 64;
            if (gravity) {
                body.AddAcceleration(new Vec2(0, 0.2f));
                mover.AddAcceleration(new Vec2(0, 0.2f));
                mover.MoveMover();
                body.UpdateVerlet();

                for (int i = 0; i < iterationCount; i++) {
                    body.UpdateConstraints();
                    CheckAllCollisions();
                }

                DrawAll();
            }
            
        }

        if (Input.GetKeyUp(Key.SPACE) && !playerEditingMode.isEditing)
            LevelWonScreen();
    }
    
    private void CheckAllCollisions() {
        collisionResolver.VerletObstacleCollisionCheck();
        collisionResolver.ObstacleMoverCollisionCheck();
        collisionResolver.VerletMoverCollisionCheck();
        collisionResolver.VerletBoundaries();
    }
    
}