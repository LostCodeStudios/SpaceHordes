using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Entities;

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
        float Rotation { set;  get; }
    }
}
