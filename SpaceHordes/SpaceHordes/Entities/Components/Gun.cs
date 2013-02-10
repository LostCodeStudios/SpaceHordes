using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities;
using SpaceHordes.Entities.Templates;
using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Components
{
    /// <summary>
    /// Gun component
    /// </summary>
    public class Gun : Component
    {
        public Gun(int ammunition, int interval, string bulletTemplateTag)
        {
            Ammunition = ammunition;
            BulletTemplateTag = bulletTemplateTag;
            Interval = interval;
            Elapsed = 0;
        }

        #region Properties
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
                if (ammo >=0)
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

        #endregion

        #region Fields
        private bool _BulletsToFire = false;
        int ammo;
        #endregion
    }
}
