using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies;
using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Components
{
    /// <summary>
    /// Player inventory class
    /// </summary>
    public class Inventory : Component
    {
        public Inventory(int red = 0, int green = 0, int blue = 0, int yellow = 0)
        {
            RED = red;
            GREEN = green;
            BLUE = blue;
            YELLOW = yellow;
        }

        public bool BuildMode = false;

        //BULLETS\\
        public int RED;
        public int GREEN;
        public int BLUE;
        public int YELLOW;
    }
}
