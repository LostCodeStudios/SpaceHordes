using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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
            base.Process();
        }
        Random r = new Random();

        public override void Process(Entity e)
        {
            ITransform t = e.GetComponent<ITransform>();
            t.Position = new Vector2((float)Math.Cos(t.Rotation), (float)Math.Sin(t.Rotation)) * t.Position.Length();

        }
    }
}