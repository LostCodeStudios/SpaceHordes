using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;

namespace GameLibrary.Entities.Components.Render
{
    /// <summary>
    /// Replaces the sprite of an entity with another sprite for a specified time.
    /// </summary>
    public class SpriteEffect : Component
    {
        public SpriteEffect(int tickCount, Sprite effect)
        {
            EffectSprite = effect;
            TickCount = tickCount;
        }


        #region Properties
        /// <summary>
        /// The original sprite before the sprite effect.
        /// </summary>
        public Sprite OriginalSprite { internal set; get; }

        /// <summary>
        /// The sprite, which during the given tick period will act as the current sprite for the entity.
        /// </summary>
        public Sprite EffectSprite { internal set; get; }

        /// <summary>
        /// The tick count, during which the enbtity will possess the sprite effect.
        /// </summary>
        public int TickCount {set; get; }

        #endregion
    }
}
