using GameLibrary.Dependencies.Entities;
using System.Collections.Generic;
using System;

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
            this.EffectSprites.Add(effect, tick);
            startingTick.Add(effect, Elapsed);
            Sprites.Push(effect);
        }

        #region Fields

        public Sprite OldSprite;
        public Dictionary<Sprite, int> EffectSprites = new Dictionary<Sprite, int>();
        public Stack<Sprite> Sprites = new Stack<Sprite>();
        public Dictionary<Sprite, int> startingTick = new Dictionary<Sprite, int>();

        public Sprite TopEffect
        {
            get 
            {
                if (EffectSprites.ContainsKey(Sprites.Peek()))
                    return Sprites.Peek();
                else
                {
                    Sprites.Pop();
                    return TopEffect;
                }
            }
        }

        public int Elapsed;

        #endregion Fields

        public void AddEffect(Sprite effect, int tick)
        {
            if (!EffectSprites.ContainsKey(effect))
            {
                this.EffectSprites.Add(effect, tick);
                startingTick.Add(effect, Elapsed);
                Sprites.Push(effect);
            }
            else
            {
                startingTick[effect] = Elapsed;
            }
        }

        public void Remove(Sprite s)
        {
            EffectSprites.Remove(s);
            startingTick.Remove(s);
        }

        public bool HasEffects()
        {
            return (EffectSprites.Count > 0);
        }
    }
}