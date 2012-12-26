using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Entities;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;

namespace GameLibrary.Entities.Systems
{
    public class RenderSystem : EntityProcessingSystem
    {
        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<Sprite> spriteMapper;
        private SpriteBatch spriteBatch;
        public SpriteSheet SpriteSheet;

        public RenderSystem(SpriteBatch spritebatch):
            base(typeof(Sprite), typeof(Transform))
        {
            this.spriteBatch = spritebatch;
        }

        public override void Initialize()
        {
            spriteMapper = new ComponentMapper<Sprite>(world);
            transformMapper = new ComponentMapper<Transform>(world);
        }


        public override void Process(Entity e)
        {
            //draw 
            Dictionary<string, Transform> transforms  = transformMapper.Get(e);
            Dictionary<string, Sprite>   sprites = spriteMapper.Get(e);

            if (sprites != null)
            {
                foreach (string key in sprites.Keys)
                {
                    if (transforms.ContainsKey(key)) //If the key has a corrisponding body
                    {
                        if (SpriteSheet == null)
                            spriteBatch.Draw(sprites[key].SpriteSheet, ConvertUnits.ToDisplayUnits(transforms[key].Position),
                                sprites[key].Source,
                                sprites[key].Color,
                                transforms[key].Rotation,
                                sprites[key].Origin,
                                sprites[key].Scale,
                                SpriteEffects.None, 0f);
                        else
                            spriteBatch.Draw(SpriteSheet.Texture, ConvertUnits.ToDisplayUnits(transforms[key].Position),
                                sprites[key].Source,
                                sprites[key].Color,
                                transforms[key].Rotation,
                                sprites[key].Origin,
                                sprites[key].Scale,
                                SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}
