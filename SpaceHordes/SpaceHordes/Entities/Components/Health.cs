using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Components
{
    public class Health : Component
    {
        public Health(double MaxHealth)
        {
            _Health = MaxHealth;
            this.MaxHealth = MaxHealth;
        }

        public void SetHealth(Entity setter, double health)
        {
            if (health < CurrentHealth && OnDamage != null)
            {
                OnDamage(setter);
            }

            _Health = health;
            if (health <= 0 && OnDeath != null && !_DeathEvent)
            {
                OnDeath(setter);
                _DeathEvent = true;
            }
        }


        #region Properties
        /// <summary>
        /// Gets or sets the current health of an entity
        /// </summary>
        public double CurrentHealth
        {
            get
            {
                return _Health;
            }
        }
        private double _Health;

        /// <summary>
        /// Gets or sets the max health of the entity.
        /// </summary>
        public double MaxHealth
        {
            set;
            get;
        }

        /// <summary>
        /// Gets whether or not the entity is alive.
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return CurrentHealth > 0;
            }
        }

        #endregion

        #region EVENTS
        public event Action<Entity> OnDeath;
        public event Action<Entity> OnDamage;

        private bool _DeathEvent = false;

        #endregion
    }
}
