using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Entities.Systems
{
    /// <summary>
    /// The sprite effect system which handles all sprite effects
    /// </summary>
    public class SpriteEffectSystem : EntityProcessingSystem
    {
        ComponentMapper<SpriteEffect> seMapper;
        ComponentMapper<Sprite> sMapper;
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
                e.RemoveComponent(ComponentTypeManager.GetTypeFor<Sprite>());
                e.AddComponent<Sprite>(seMapper.Get(e).EffectSprite);
                e.Refresh();
            }
        }


        /// <summary>
        /// Proccesses sprite effects.
        /// </summary>
        /// <param name="e"></param>
        public override void Process(Entity e)
        {
            lock (e)
            {
                Sprite s = sMapper.Get(e);
                SpriteEffect se = seMapper.Get(e);

                se.Elapsed--;
                if (se.Elapsed <= 0)
                {
                    e.RemoveComponent(ComponentTypeManager.GetTypeFor<Sprite>());
                    e.AddComponent<Sprite>(seMapper.Get(e).OldSprite);
                    e.RemoveComponent(ComponentTypeManager.GetTypeFor<SpriteEffect>());
                    e.Refresh();
                }
            }
        }

    }
}
