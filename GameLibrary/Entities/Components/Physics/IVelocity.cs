using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Entities;

namespace GameLibrary.Entities.Components
{
    /// <summary>
    /// A component of the entities velocity.
    /// </summary>
    public interface IVelocity : Component
    {
        /// <summary>
        /// The linear velocity of an entity.
        /// </summary>
        Vector2 LinearVelocity { set; get; }

        /// <summary>
        /// The angular velocity of an entity.
        /// </summary>
        float AngularVelocity { set; get; }
    }
}
