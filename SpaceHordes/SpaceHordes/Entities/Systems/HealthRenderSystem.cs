using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Systems
{
    class HealthRenderSystem : EntityProcessingSystem
    {
        SpriteBatch _SpriteBatch;
        SpriteFont _SpriteFont;

        ComponentMapper<Health> healthMapper;
        ComponentMapper<ITransform> transformMapper;
        public HealthRenderSystem(SpriteBatch spriteBatch)
            : base(typeof(Health), typeof(ITransform))
        {
            this._SpriteBatch = spriteBatch;
        }

        public void LoadContent(SpriteFont spriteFont)
        {
            this._SpriteFont = spriteFont;
        }

        public override void Initialize()
        {
            healthMapper = new ComponentMapper<Health>(world);
            transformMapper = new ComponentMapper<ITransform>(world);
        }

        public override void Process(Entity e)
        {
            Health health = healthMapper.Get(e);
            ITransform transform = transformMapper.Get(e);

            //Draw
            _SpriteBatch.DrawString(_SpriteFont, health.ToString(),
                ConvertUnits.ToDisplayUnits(transform.Position) + new Vector2(-25),
                Color.Red);
        }
    }
}
