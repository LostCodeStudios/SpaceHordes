using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Entities;
using GameLibrary.Helpers;

namespace SpaceHordes.Entities.Systems
{
    public class RenderSystem : EntityProcessingSystem
    {
        private ComponentMapper<Physical> physicalMapper;
        private ComponentMapper<Sprite> spriteMapper;
        private GraphicsDevice device;
        private SpriteBatch spriteBatch;

        public RenderSystem(GraphicsDevice graphics, SpriteBatch spritebatch):
            base(typeof(Sprite), typeof(Physical))
        {
            this.spriteBatch = spritebatch;
            this.device = graphics;
        }


        public override void Initialize()
        {
            spriteMapper = new ComponentMapper<Sprite>(world);
            physicalMapper = new ComponentMapper<Physical>(world);
        }

        public override void Process(Entity e)
        {
            //draw 
            Dictionary<string, Physical> bodies = physicalMapper.Get(e);
            Dictionary<string, Sprite>sprites= spriteMapper.Get(e);

            foreach(string key in sprites.Keys){
                spriteBatch.Draw(sprites[key].SpriteSheet, ConvertUnits.ToDisplayUnits(bodies[key].Position),
                    sprites[key].Source,
                    sprites[key].Color,
                    bodies[key].Rotation,
                    sprites[key].Origin,
                    sprites[key].Scale,
                    SpriteEffects.None,
                    sprites[key].Layer);

            }
                

        }
    }
}
