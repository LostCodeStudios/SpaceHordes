using GameLibrary.Dependencies.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Entities.Components.Render
{
    public class SpriteEffect : Component
    {
        /// <summary>
        /// Creates a new sprite effect lasting for n time.
        /// </summary>
        /// <param name="effect">The sprite effect.</param>
        /// <param name="tick">n time</param>
        public SpriteEffect(Sprite effect, int tick)
        {
            this.EffectSprite = effect;
            this.Elapsed = tick;
        }


        #region Fields

        public Sprite OldSprite;
        public Sprite EffectSprite;

        internal int Elapsed;

        #endregion
    }
}
