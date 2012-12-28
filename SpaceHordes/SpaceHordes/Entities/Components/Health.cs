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
            this.CurrentHealth = MaxHealth;
            this.MaxHealth = MaxHealth;
        }

        /// <summary>
        /// Gets or sets the current health of an entity
        /// </summary>
        public double CurrentHealth
        {
            set;
            get;
        }

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

        public override string ToString()
        {
            return "HP: " + CurrentHealth + "/" + MaxHealth;
        }
    }
}
