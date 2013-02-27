using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;

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