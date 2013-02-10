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
        public Inventory(uint red = 0, uint green = 0, uint blue = 0, uint yellow = 0)
        {
            RED = new Gun((int)red, 60000, "RedBullet1");
            GREEN = new Gun((int)green, 60000, "GreenBullet1");
            BLUE = new Gun((int)blue, 60000, "BlueBullet1");
            WHITE = new Gun(-1, 60000, "WhiteBullet1");
            CurrentGun = WHITE;
            YELLOW = yellow;
        }

        public bool BuildMode = false;

        public Gun RED;
        public Gun GREEN;
        public Gun BLUE;
        public Gun WHITE;
        public Gun CurrentGun;

        public uint YELLOW;
    }
}
