using GameLibrary.Dependencies.Entities;
using System;

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
                Tick = 10;
            }

            _Health = health;
            if (health <= 0 && OnDeath != null && !_DeathEvent)
            {
                OnDeath(setter);
                _DeathEvent = true;
            }

            if (_Health > MaxHealth)
            {
                _Health = MaxHealth; //Duh
            }
        }

        public void Damage(Entity setter, double amount)
        {
            SetHealth(setter, CurrentHealth - amount);
        }

        public void AddHealth(Entity setter, double health)
        {
            SetHealth(setter, _Health + health);
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

        #endregion Properties

        #region EVENTS

        public event Action<Entity> OnDeath;

        public event Action<Entity> OnDamage;

        private bool _DeathEvent = false;
        public int Tick = 0;

        #endregion EVENTS
    }
}