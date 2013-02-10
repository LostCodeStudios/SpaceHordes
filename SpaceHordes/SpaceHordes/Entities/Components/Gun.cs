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
        public Gun(int ammunition, string bulletTemplateTag)
        {
            Ammunition = ammunition;
            BulletTemplateTag = bulletTemplateTag;
        }

        #region Properties
        public uint BulletsToFire
        {
            set
            {
                if (ammo < 0)
                    _BulletsToFire = value;
                else if (value > Ammunition)
                {
                    _BulletsToFire = (uint)Ammunition;
                }
                else
                    _BulletsToFire = value;
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
        #endregion

        #region Fields
        private uint _BulletsToFire = 0;
        int ammo;
        #endregion
    }
}
