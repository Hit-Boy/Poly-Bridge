using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

    class Mover : Sprite
    {
        public Mover() : base("triangle.png");
        {

        }
        
        public void Update()
        {
            this.x++;
        }
    }

