using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies;
using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Components
{
    public enum GunType
    {
        RED,
        GREEN,
        BLUE,
        WHITE,
        YELLOW
    }

    /// <summary>
    /// Player inventory class
    /// </summary>
    public class Inventory : Component
    {
        public Inventory(uint red = 0, uint green = 0, uint blue = 0, uint yellow = 0)
        {
            RED = new Gun((int)red, 600, 1, "RedBullet");
            GREEN = new Gun((int)green, 600, 1, "GreenBullet");
            BLUE = new Gun((int)blue, 600, 1, "BlueBullet");
            WHITE = new Gun(-1, 600, 1, "WhiteBullet");
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
                switch (_CurrentGunType)
                {
                    case GunType.BLUE:
                        return BLUE;
                        break;
                    case GunType.RED:
                        return RED;
                        break;
                    case GunType.GREEN:
                        return GREEN;
                        break;
                    case GunType.WHITE:
                        return WHITE;
                        break;
                    case GunType.YELLOW: //YELLOW CODE'
                        return null;
                        break;
                    default:
                        return WHITE;
                        break;
                }
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
    }
}
