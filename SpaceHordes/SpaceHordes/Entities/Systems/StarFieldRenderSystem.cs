using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Systems
{
    public class StarFieldRenderSystem : EntityProcessingSystem
    {
        SpriteBatch spriteBatch;
        public StarFieldRenderSystem(SpriteBatch spriteBatch)
            : base(typeof(Sprite))
        {
            this.spriteBatch = spriteBatch;
        }

        public override void Process()
        {
            spriteBatch.Begin();
            base.Process();
            spriteBatch.End();
        }

        public override void Process(Entity e)
        {
            if (e.Group == "Stars")
            {
                Sprite s = e.GetComponent<Sprite>();

                spriteBatch.Draw(
                    s.SpriteSheet.Texture, 
                    s.Origin, 
                    s.CurrentRectangle, 
                    Color.White);
            }
        }
    }
}
