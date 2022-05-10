using System;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;

public class MyGame : Game
{
	public int levelTracker = 0;
	public MyGame() : base(800, 600, false)	// Final: 1920, 1080, true
	{
		LoadLevel("Level1.tmx");
	}

	void DestroyAll()
	{
		List<GameObject> children = GetChildren();
		foreach (GameObject child in children)
		{
			child.Destroy();
		}
	}

	public void LoadLevel(string filename)
	{
		DestroyAll(); //Destroy all children before creating new level
		AddChild(new Level(filename));
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