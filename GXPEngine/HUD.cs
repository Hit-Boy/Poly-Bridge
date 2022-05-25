using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GXPEngine;

class HUD : GameObject
{
    EasyDraw platformCount;
    Level currentLevel;
    Font POORICH;

    public HUD()
    {
        POORICH = Utils.LoadFont("POORICH.ttf", 48);

        platformCount = new EasyDraw(500, 200);
        platformCount.TextFont(POORICH);
        platformCount.Fill(Color.White);
        platformCount.Text("Platforms Left: ");
        platformCount.SetXY(100, 825);

        AddChild(platformCount);
    }

    void Update()
    {
        SetCount();
    }

    void SetCount()
    {
        platformCount.Text(string.Format("Platforms Left: {0}", currentLevel.playerEditingMode.platformCount), true);
    }

    public void SetParent()
    {

        currentLevel = (Level)this.parent;
    }
}