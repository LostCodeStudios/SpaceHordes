using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceHordes.Entities.Components
{
    /// <summary>
    /// Player inventory class
    /// </summary>
    class Inventory
    {
        public Inventory(int red = 0, int green = 0, int blue = 0, int yellow = 0)
        {
            RED = red;
            GREEN = green;
            BLUE = blue;
            YELLOW = yellow;
        }

        //BULLETS\\
        public int RED;
        public int GREEN;
        public int BLUE;
        public int YELLOW;
    }
}
