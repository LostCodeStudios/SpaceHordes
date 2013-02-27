using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;

namespace GameLibrary.Entities.Components
{
    /// <summary>
    /// An interface which specifies if a component posses a transformation.
    /// </summary>
    public interface ITransform : Component
    {
        /// <summary>
        /// The Position of a component
        /// </summary>
        Vector2 Position { set; get; }

        /// <summary>
        /// The rotation of a component.
        /// </summary>
        float Rotation { set; get; }

        void RotateTo(Vector2 direction);
    }
}