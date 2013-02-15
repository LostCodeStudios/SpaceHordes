using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Components
{
    struct Bullet : Component
    {
        /// <summary>
        /// Creates a bullet with a on bullet hit call back.
        /// </summary>
        /// <param name="damage">The damage of the bullet</param>
        /// <param name="damageGroup">The damage group of this bullet</param>
        /// <param name="onBulletHit">The event which will be called when the bullet hits it's target.</param>
        public Bullet(double damage, string damageGroup, BulletHitEvent onBulletHit) : this()
        {
            this.Damage = damage;
            this.DamageGroup = damageGroup;
            this.OnBulletHit = onBulletHit;
            this.collisionChecked = 0;
        }

        public Bullet(double damage)
            : this(damage, "", delegate(Entity e) { })
        {
        }

        

        /// <summary>
        /// The damage of the bullet on hit.
        /// </summary>
        public double Damage;

        //TODO: ADD DAMAGE TYPE <#
        /// <summary>
        /// The group name of which this bullet will damage.
        /// </summary>
        public string DamageGroup;

        /// <summary>
        /// When the bullet hits it's target, this event is called.
        /// </summary>
        public BulletHitEvent OnBulletHit;

        public int collisionChecked {set;get;}

        public Entity Firer;
        
    }

    public delegate void BulletHitEvent(Entity hit);
}
