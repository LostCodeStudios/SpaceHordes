using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;

namespace GameLibrary.Entities.Systems
{
    public class RenderSystem : EntityProcessingSystem
    {
        private ComponentMapper<ITransform> transformMapper;
        private ComponentMapper<Sprite> spriteMapper;

        private SpriteBatch spriteBatch;
        private Camera camera;

        public RenderSystem(SpriteBatch spritebatch, Camera camera):
            base(typeof(Sprite), typeof(ITransform))
        {
            this.spriteBatch = spritebatch;
            this.camera = camera;
        }

        public override void Initialize()
        {
            spriteMapper = new ComponentMapper<Sprite>(world);
            transformMapper = new ComponentMapper<ITransform>(world);
        }

        /// <summary>
        /// Renders all entities with a sprite and a transform to the screen.
        /// </summary>
        /// <param name="e"></param>
        public override void Process(Entity e)
        {
            //Get sprite data and transform
            ITransform transform  = transformMapper.Get(e);
            Sprite sprite = spriteMapper.Get(e);


            //Draw to sprite batch
            spriteBatch.Draw(
                sprite.SpriteSheet.Texture, 
                ConvertUnits.ToDisplayUnits(transform.Position),
                sprite.CurrentRectangle,
                sprite.Color,
                transform.Rotation,
                sprite.Origin,
                sprite.Scale,
                SpriteEffects.None, sprite.Layer);
        }

        /// <summary>
        /// Starts/Ends spriteBatch
        /// </summary>
        /// <param name="entities"></param>
        protected override void ProcessEntities(Dictionary<int, Entity> entities) 
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, camera.View);
            base.ProcessEntities(entities);
            spriteBatch.End();
        }
    }
}
