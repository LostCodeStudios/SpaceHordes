using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Render;
using GameLibrary.Entities.Components;

namespace GameLibrary.Entities.Systems
{
    /// <summary>
    /// handles the processing of sprite effects
    /// </summary>
    public class SpriteEffectSystem : EntityProcessingSystem
    {
        /// <summary>
        /// Initializes the sprite effect system
        /// </summary>
        public SpriteEffectSystem() :base(typeof(SpriteEffect))
        {
        }

        /// <summary>
        /// Called when a sprite effect is added to an entity.
        /// </summary>
        /// <param name="e"></param>
        public override void Added(Entity e)
        {
            SpriteEffect se = e.GetComponent<SpriteEffect>();
            se.OriginalSprite = e.GetComponent<Sprite>();
            e.RemoveComponent<Sprite>(e.GetComponent<Sprite>());
            e.AddComponent<Sprite>(se.EffectSprite);
            e.Refresh();
            
            base.Added(e);
        }

        /// <summary>
        /// Processes all of the sprite effects and their corrisponding entities.
        /// </summary>
        /// <param name="e"></param>
        public override void Process(Entity e)
        {
            SpriteEffect se = e.GetComponent<SpriteEffect>();

            if (se.TickCount > 0)
            {
                se.TickCount--;
            }
            else //Remove if effect is finished.
            {
                e.RemoveComponent<Sprite>(e.GetComponent<Sprite>());
                e.AddComponent<Sprite>(se.OriginalSprite);
                e.RemoveComponent<SpriteEffect>(se);
                e.Refresh();
            }
        }
    }
}
