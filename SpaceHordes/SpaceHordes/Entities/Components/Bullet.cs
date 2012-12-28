using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Components
{
    class Bullet : Component
    {
        public Bullet(double damage, string damageGroup)
        {
            this.Damage = damage;
            this.DamageGroup = damageGroup;
        }

        /// <summary>
        /// The damage of the bullet on hit.
        /// </summary>
        public double Damage { set; get; }

        //TODO: ADD DAMAGE TYPE <#
        /// <summary>
        /// The group name of which this bullet will damage.
        /// </summary>
        public string DamageGroup { set; get; }
    }
}
