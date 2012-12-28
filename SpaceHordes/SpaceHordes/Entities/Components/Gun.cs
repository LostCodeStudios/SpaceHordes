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
        public Gun(uint ammunition, string bulletTemplateTag)
        {
            Ammunition = ammunition;
            BulletTemplateTag = bulletTemplateTag;
        }

        #region Properties
        public uint BulletsToFire
        {
            set
            {
                if (value > Ammunition)
                    _BulletsToFire = Ammunition;
                else
                    _BulletsToFire = value;
            }
            get
            {
                return _BulletsToFire;
            }
        }
        public uint Ammunition
        {
            set;
            get;
        }

        public string BulletTemplateTag
        {
            get;
            private set;
        }
        #endregion

        #region Fields
        private uint _BulletsToFire = 0;
        #endregion
    }
}
