using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceHordes.Entities.Systems
{
    public class StarFieldRenderSystem : GroupSystem
    {
        private SpriteBatch spriteBatch;

        public StarFieldRenderSystem(SpriteBatch spriteBatch)
            : base("Stars")
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
            Sprite s = e.GetComponent<Sprite>();

            spriteBatch.Draw(
                s.SpriteSheet.Texture,
                s.Origin,
                s.CurrentRectangle,
                Color.White);
        }
    }
}