using System;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;

public class MyGame : Game
{
	Sound backgroundMusic = new Sound("backgroundMusic.mp3", true, true);
	public int levelTracker = -1;
	public MyGame() : base(800, 600, false)	// Final: 1920, 1080, true
	{
		backgroundMusic.Play();
		LoadLevel("MainMenu.tmx");
	}

	void DestroyAll()
	{
		List<GameObject> children = GetChildren();
		foreach (GameObject child in children)
		{
			child.Destroy();
		}
	}

	public void LoadLevel(string filename, bool restartLevel = false)
	{
		DestroyAll(); //Destroy all children before creating new level
		AddChild(new Level(filename));
		if(!restartLevel)
			++levelTracker;
	}

	public void LoadWinScreen()
    {
		AddChild(new WinScreen());
    }

	void Update()
	{

	}

	static void Main()
	{
		new MyGame().Start();
	}
}