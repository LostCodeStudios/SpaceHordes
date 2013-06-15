using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Systems;
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
        Gunner,
        Cannon
    }

    /// <summary>
    /// Player inventory class
    /// </summary>
    public class Inventory : Component
    {
        public InvType _type = InvType.Turret;

        public bool DisplayTag = false;

        public Inventory()
            : this(0, 0, 0, 0, InvType.Turret, "")
        {
        }

        public Inventory(uint red, uint green, uint blue, uint yellow, InvType type, string key)
        {
            _type = type;
            if (type == InvType.Turret)
            {
                WHITE = new Gun(-1, 200, 1, "WhiteBullet", InvType.Turret);
                _CurrentGunType = GunType.WHITE;
            }
            else if (type == InvType.Player)
            {
                RED = new Gun((int)red, 100, 1, "RedBullet", InvType.Player, Vector2.UnitY, -Vector2.UnitY * 3);
                GREEN = new Gun((int)green, 200, 1, "GreenBullet", InvType.Player, Vector2.UnitY, -Vector2.UnitY * 3);
                BLUE = new Gun((int)blue, 300, 1, "BlueBullet", InvType.Player, Vector2.UnitY, -Vector2.UnitY * 3);
                WHITE = new Gun(-1, 200, 1, "WhiteBullet", InvType.Player, Vector2.UnitY, -Vector2.UnitY * 3);

                _CurrentGunType = GunType.WHITE;
                YELLOW = yellow;
            }
            else if (type == InvType.Gunner)
            {
                List<Vector2> offsets = new List<Vector2>();

                switch (key)
                {
                    case "graybulbwithsidegunthings":
                        offsets.Add(new Vector2(0, -12));
                        offsets.Add(new Vector2(0, 8));
                        break;

                    case "blueshipwithbulb":
                        offsets.Add(new Vector2(0, -10));
                        offsets.Add(new Vector2(0, 8));
                        break;

                    case "browntriangleship":
                        offsets.Add(new Vector2(0, -10));
                        offsets.Add(new Vector2(0, 7));
                        break;
                }

                WHITE = new Gun(-1, 1000, 1, "EnemyBullet", InvType.Gunner, offsets.ToArray());
                _CurrentGunType = GunType.WHITE;
            }
            else if (type == InvType.Cannon)
            {
                WHITE = new Gun(-1, 2500, 1, "", InvType.Cannon, new Vector2[] { });
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
            GiveCrystals(color, amount, false);
        }

        public void GiveCrystals(Color color, int amount, bool surgeCrystal)
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
            {
                if (!surgeCrystal)
                {
                    CurrentGun.PowerUp(5000, amount);
                }
                else
                {
                    CurrentGun.PowerUp((int)DirectorSystem.StateDurations[(int)SpawnState.Surge] * 1000 - DirectorSystem.ElapsedSurge, amount);
                }
            }
        }

        public void GiveCrystals(Crystal crystal)
        {
            if (crystal != null)
            {
                GiveCrystals(crystal.Color, crystal.Amount, crystal.SurgeCrystal);
            }
        }
    }
}