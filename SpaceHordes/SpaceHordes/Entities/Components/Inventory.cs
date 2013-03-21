using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceHordes.Entities.Components
{
    public enum GunType
    {
        RED,
        GREEN,
        BLUE,
        WHITE
    }

    public enum InvType
    {
        Player,
        Turret,
        Gunner
    }

    /// <summary>
    /// Player inventory class
    /// </summary>
    public class Inventory : Component
    {
        public InvType _type = InvType.Turret;

        public Inventory(uint red = 0, uint green = 0, uint blue = 0, uint yellow = 0, InvType type = InvType.Turret, string key = "")
        {
            _type = type;
            if (type == InvType.Turret)
            {
                WHITE = new Gun(-1, 200, 1, "WhiteBullet");
                _CurrentGunType = GunType.WHITE;
            }
            else if (type == InvType.Player)
            {
                RED = new Gun((int)red, 100, 1, "RedBullet", Vector2.UnitX * 2, -Vector2.UnitX * 2);
                GREEN = new Gun((int)green, 600, 1, "GreenBullet", Vector2.UnitX * 2, -Vector2.UnitX * 2);
                BLUE = new Gun((int)blue, 300, 1, "BlueBullet", Vector2.UnitX * 2, -Vector2.UnitX * 2);
                WHITE = new Gun(-1, 200, 1, "WhiteBullet", Vector2.UnitX * 2, -Vector2.UnitX * 2);
                _CurrentGunType = GunType.WHITE;
                YELLOW = yellow;
            }
            else if (type == InvType.Gunner)
            {
                List<Vector2> offsets = new List<Vector2>();

                switch (key)
                {
                    case "graybulbwithsidegunthings":
                        offsets.Add(new Vector2(14, 1));
                        offsets.Add(new Vector2(14, 21));
                        break;
                    
                    case "blueshipwithbulb":
                        offsets.Add(new Vector2(22, 3));
                        offsets.Add(new Vector2(22, 21));
                        break;

                    case "browntriangleship":
                        offsets.Add(new Vector2(16, 3));
                        offsets.Add(new Vector2(16, 19));
                        break;
                }

                WHITE = new Gun(-1, 200, 1, "WhiteBullet", offsets.ToArray());
                _CurrentGunType = GunType.WHITE;
            }
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
                        returnValue = BLUE;
                        break;

                    case GunType.RED:
                        returnValue = RED;
                        break;

                    case GunType.GREEN:
                        returnValue = GREEN;
                        break;

                    case GunType.WHITE:
                        returnValue = WHITE;
                        break;

                    default:
                        returnValue = null;
                        break;
                }

                return returnValue;
            }

            set
            {
                if (value == BLUE)
                    _CurrentGunType = GunType.BLUE;
                else if (value == RED)
                    _CurrentGunType = GunType.RED;
                else if (value == GREEN)
                    _CurrentGunType = GunType.GREEN;
                else if (value == WHITE)
                    _CurrentGunType = GunType.WHITE;
            }
        }

        public void ChangeGun(Entity e, GunType gun)
        {
            if (e.GetComponent<Gun>() != null)
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

            if (color == Color.Gray)
                CurrentGun.PowerUp(5000, amount);
        }

        public void GiveCrystals(Crystal crystal)
        {
            if (crystal != null)
                GiveCrystals(crystal.Color, crystal.Amount);
        }
    }
}