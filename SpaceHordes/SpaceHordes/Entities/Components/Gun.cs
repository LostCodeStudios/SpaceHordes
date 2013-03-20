using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceHordes.Entities.Components
{
    /// <summary>
    /// Gun component
    /// </summary>
    public class Gun : Component
    {
        public Gun(int ammunition, int interval, int power, string bulletTemplateTag, params Vector2[] offsets)
        {
            Ammunition = ammunition;
            DefaultTemplate = bulletTemplateTag;
            Power = power;
            Interval = interval;
            Elapsed = 0;

            if (offsets.Length == 0)
            {
                GunOffsets.Add(Vector2.Zero);
            }
            else
            {
                foreach (Vector2 offset in offsets)
                {
                    gunOffsets.Add(offset);
                }
            }
        }

        #region Properties

        public List<Vector2> GunOffsets
        {
            get { return gunOffsets; }
        }

        public bool BulletsToFire
        {
            set
            {
                if (ammo < 0)
                    _BulletsToFire = value;
                else if (Ammunition > 0)
                {
                    _BulletsToFire = value;
                }
            }
            get
            {
                return _BulletsToFire;
            }
        }

        public int Ammunition
        {
            set
            {
                if (ammo >= 0)
                    ammo = value;
            }
            get
            {
                if (ammo >= 0)
                    return ammo;
                else
                    return 1000; //if ammo = -1, infinite ammo
            }
        }

        public string BulletTemplateTag
        {
            get
            {
                return DefaultTemplate + Power.ToString();
            }
        }

        public string DefaultTemplate
        {
            get;
            private set;
        }

        public int Interval
        {
            get;
            private set;
        }

        public int Elapsed
        {
            get;
            set;
        }

        public int Power
        {
            get
            { return power; }
            set
            { power = (int)MathHelper.Clamp((float)value, 0f, 3f); }
        }

        private int power;

        public int PowerUpTime
        {
            get;
            set;
        }

        public Vector2 BulletVelocity = Vector2.One;

        #endregion Properties

        #region Fields

        private bool _BulletsToFire = false;
        private int ammo;

        List<Vector2> gunOffsets = new List<Vector2>();

        #endregion Fields

        #region Methods

        public void PowerUp(int time, int power)
        {
            Power = power;
            PowerUpTime = time;
        }

        public void UpdatePower(int elapsed)
        {
            PowerUpTime -= elapsed;

            if (PowerUpTime <= 0)
                Power = 1;
        }

        #endregion Methods
    }
}