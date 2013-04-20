using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Render;
using System.Collections.Generic;

namespace GameLibrary.Entities.Systems
{
    /// <summary>
    /// The sprite effect system which handles all sprite effects
    /// </summary>
    public class SpriteEffectSystem : EntityProcessingSystem
    {
        private ComponentMapper<SpriteEffect> seMapper;
        private ComponentMapper<Sprite> sMapper;

        public SpriteEffectSystem()
            : base(typeof(SpriteEffect))
        {
        }

        public override void Initialize()
        {
            seMapper = new ComponentMapper<SpriteEffect>(world);
            sMapper = new ComponentMapper<Sprite>(world);
        }

        /// <summary>
        /// Adds the sprite effect to the entity.
        /// </summary>
        /// <param name="e"></param>
        public override void Added(Entity e)
        {
            lock (e)
            {
                seMapper.Get(e).OldSprite = sMapper.Get(e);
                e.AddComponent<Sprite>(seMapper.Get(e).TopEffect);
                e.Refresh();
            }
        }

        /// <summary>
        /// Proccesses sprite effects.
        /// </summary>
        /// <param name="e"></param>
        public override void Process(Entity e)
        {
            List<Sprite> toRemove = new List<Sprite>();
            Sprite s = sMapper.Get(e);
            SpriteEffect se = seMapper.Get(e);
            lock (e)
            {
                se.Elapsed++;

                foreach (Sprite ss in se.EffectSprites.Keys)
                {
                    if (se.Elapsed - se.startingTick[ss] >= se.EffectSprites[ss])
                    {
                        toRemove.Add(ss);
                    }
                }
            }

            foreach (Sprite ss in toRemove)
            {
                se.Remove(ss);
                if (se.HasEffects())
                    e.AddComponent<Sprite>(seMapper.Get(e).TopEffect);
                else
                {
                    e.AddComponent<Sprite>(se.OldSprite);
                    e.RemoveComponent(ComponentTypeManager.GetTypeFor<SpriteEffect>());
                }
                e.Refresh();
            }
        }
    }
}