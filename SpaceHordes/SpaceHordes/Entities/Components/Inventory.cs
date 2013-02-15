using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Components
{
    public enum GunType
    {
        RED,
        GREEN,
        BLUE,
        WHITE
    }

    /// <summary>
    /// Player inventory class
    /// </summary>
    public class Inventory : Component
    {
        public Inventory(uint red = 0, uint green = 0, uint blue = 0, uint yellow = 0)
        {
            RED = new Gun((int)red, 600, 1, "RedBullet");
            GREEN = new Gun((int)green, 100, 1, "GreenBullet");
            BLUE = new Gun((int)blue, 300, 1, "BlueBullet");
            WHITE = new Gun(-1, 200, 1, "WhiteBullet");
            _CurrentGunType = GunType.WHITE;
            YELLOW = yellow;
        }

        public bool BuildMode = false;

        public Gun RED;
        public Gun GREEN;
        public Gun BLUE;
        public Gun WHITE;
        public uint YELLOW;

        private GunType _CurrentGunType;
        public Gun CurrentGun
        {
            get
            {
                Gun returnValue;

                switch (_CurrentGunType)
                {
                    case GunType.BLUE:
                        returnValue =  BLUE;
                        break;
                    case GunType.RED:
                        returnValue = RED;
                        break;
                    case GunType.GREEN:
                        returnValue = GREEN;
                        break;
                    case GunType.WHITE:
                        returnValue =  WHITE;
                        break;
                    default:
                        returnValue = null;
                        break;
                }

                return returnValue;
            }
        }

        public void ChangeGun(Entity e, GunType gun)
        {
            if(e.GetComponent<Gun>() != null)
            {
                CurrentGun.Ammunition = e.GetComponent<Gun>().Ammunition;
                e.RemoveComponent<Gun>(e.GetComponent<Gun>());
            }
            _CurrentGunType = gun;
            e.AddComponent<Gun>(CurrentGun);
        }

        public void GiveCrystals(Color color, int amount)
        {
            if (color == Color.Red)
                RED.Ammunition += amount;

            if (color == Color.Green)
                GREEN.Ammunition += amount;

            if (color == Color.Blue)
                BLUE.Ammunition += amount;

            if (color == Color.Yellow)
                YELLOW += (uint)amount;
        }

        public void GiveCrystals(Crystal crystal)
        {
            GiveCrystals(crystal.Color, crystal.Amount);
        }
    }
}
