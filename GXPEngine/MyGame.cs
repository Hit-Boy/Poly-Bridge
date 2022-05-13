using System;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;

public class MyGame : Game
{
	Sound backgroundMusic = new Sound("backgroundMusic.mp3", true, true);
	MouseFollower mouseFollower;
	public int levelTracker = -1;
	public MyGame() : base(1920, 1080, false)	// Final: 1920, 1080, true
	{
		mouseFollower = new MouseFollower();
		backgroundMusic.Play();
		LoadLevel("MainMenu.tmx");
		AddChild(mouseFollower);
	}

	void DestroyAll()
	{
		List<GameObject> children = GetChildren();
		foreach (GameObject child in children)
		{
			if(!(child is MouseFollower))
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
		SetChildIndex(mouseFollower, 1);
	}

	static void Main()
	{
		new MyGame().Start();
	}
}