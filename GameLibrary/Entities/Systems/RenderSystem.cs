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
        private ComponentMapper<Physical> physicalMapper;
        private ComponentMapper<Sprite> spriteMapper;
        private SpriteBatch spriteBatch;

        public RenderSystem(SpriteBatch spritebatch):
            base(typeof(Sprite), typeof(Physical))
        {
            this.spriteBatch = spritebatch;
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

            Physical lastKnownBody = null;
            
            foreach(string key in sprites.Keys){
                if (bodies.ContainsKey(key)) //If the key has a corrisponding body
                {
                    spriteBatch.Draw(sprites[key].SpriteSheet, ConvertUnits.ToDisplayUnits(bodies[key].Position),
                        sprites[key].Source,
                        sprites[key].Color,
                        bodies[key].Rotation,
                        sprites[key].Origin,
                        sprites[key].Scale,
                        SpriteEffects.None,
                        sprites[key].Layer);
                    lastKnownBody = bodies[key];
                }
                else if(lastKnownBody != null)//If not corrisponding body, use last known body
                    spriteBatch.Draw(sprites[key].SpriteSheet, ConvertUnits.ToDisplayUnits(lastKnownBody.Position),
                        sprites[key].Source,
                        sprites[key].Color,
                        lastKnownBody.Rotation,
                        sprites[key].Origin,
                        sprites[key].Scale,
                        SpriteEffects.None,
                        sprites[key].Layer);
                else //No known position
                    spriteBatch.Draw(sprites[key].SpriteSheet, ConvertUnits.ToDisplayUnits(Vector2.Zero),
                        sprites[key].Source,
                        sprites[key].Color,
                        0f,
                        sprites[key].Origin,
                        sprites[key].Scale,
                        SpriteEffects.None,
                        sprites[key].Layer);

            }
                

        }
    }
}
